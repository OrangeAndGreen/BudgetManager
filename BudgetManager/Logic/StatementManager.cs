using BudgetManager.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BudgetManager.Logic
{
    public static class StatementManager
    {
        enum ReadState
        {
            Unknown = 0,
            Header = 1,
            Interim = 2,
            Credit = 3,
            Check = 4,
            Debit = 5,
            Balance = 6
        }

        public static List<Account> Accounts { get; set; }
        public static Dictionary<string, List<Statement>> AllStatements { get; set; }
        public static List<AmazonStatement> AmazonStatements { get; set; }

        public static List<string> Load(string directory)
        {
            List<string> warnings = new List<string>();
            string[] directories = Directory.GetDirectories(directory);

            Accounts = new List<Account>();
            AllStatements = new Dictionary<string, List<Statement>>();
            AmazonStatements = new List<AmazonStatement>();
            foreach (string subDirectory in directories)
            {
                string directoryName = Path.GetFileNameWithoutExtension(subDirectory);
                bool isAmazon = directoryName.Equals(Constants.AmazonLabel);

                if (!isAmazon)
                {
                    AllStatements[directoryName] = new List<Statement>();
                }

                foreach (string file in Directory.GetFiles(subDirectory))
                {
                    if(isAmazon)
                    {
                        AmazonStatement statement = ReadAmazonCSV(file);

                        if (statement != null)
                        {
                            AmazonStatements.Add(statement);

                            Console.WriteLine("   Read Amazon statement {0} with {1} transactions", Path.GetFileName(file), statement.Transactions.Count);
                        }
                    }
                    else
                    {
                        Statement statement = LoadStatementFromCache(file) ?? ReadStatementFile(file, directoryName);

                        if (statement == null)
                            continue;

                        AllStatements[directoryName].Add(statement);

                        double balance = statement.StartingBalance;
                        foreach (Transaction entry in statement.Transactions)
                        {
                            if (entry.TypeId <= 0)
                            {
                                TransactionType transactionType = TypeManager.Identify(entry.Description);
                                if (transactionType != null)
                                {
                                    entry.TypeId = transactionType.Id;
                                }
                            }

                            balance += entry.CheckingAmount + entry.SavingsAmount + entry.CreditAmount;
                        }

                        if (Math.Abs(balance - statement.EndingBalance) > 0.01)
                        {
                            string warning = $"Ending balance mismatch: {balance} vs. stated {statement.EndingBalance}";
                            warnings.Add(warning);
                            Console.WriteLine(warning);
                        }

                        SaveToCache(statement);

                        Console.WriteLine("   Read statement {0} with {1} transactions (${2} --> ${3}, error={4:0.00})", Path.GetFileName(file), statement.Transactions.Count, statement.StartingBalance, statement.EndingBalance, balance - statement.EndingBalance);
                    }
                }

                if (isAmazon)
                {
                    Console.WriteLine("Finished reading {0} directory", directoryName);
                }
                else
                {
                    Console.WriteLine("Finished reading {0} directory ({1} --> {2})", directoryName, AllStatements[directoryName].First().StartingBalance, AllStatements[directoryName].Last().EndingBalance);
                }
            }

            return warnings;
        }

        public static Statement ReadStatementFile(string filepath, string account)
        {
            if (!filepath.EndsWith(".pdf") && !filepath.EndsWith(".csv"))
                return null;

            string[] accountParts = account.Split('-');
            Enum.TryParse(accountParts[0].Trim(), out AccountType accountEnum);

            Statement ret = new Statement(filepath)
            {
                AccountName = account,
                AccountLabel = accountParts.Length > 1 ? accountParts[1].Trim() : null,
                AccountType = accountEnum
            };

            switch (ret.AccountType)
            {
                case AccountType.Checking:
                    ret.Transactions.AddRange(ReadUsaaCheckingPDF(ret));
                    break;
                case AccountType.Credit:
                    if (ret.AccountLabel.Equals("USAA"))
                    {
                        ret.Transactions.AddRange(ReadUsaaCreditPDF(ret));
                    }
                    else if (ret.AccountLabel.Equals("Chase"))
                    {
                        ret.Transactions.AddRange(ReadChaseCreditPDF(ret));
                    }
                    else if (ret.AccountLabel.Equals("BJs"))
                    {
                        ret.Transactions.AddRange(ReadBjsCreditPDF(ret));
                    }
                    break;
                case AccountType.Savings:
                    ret.Transactions.AddRange(ReadUsaaSavingsPDF(ret));
                    break;
                default:
                    Console.WriteLine($"Unrecognized account type: {ret.AccountType}");
                    break;
            }

            foreach(Transaction t in ret.Transactions)
            {
                t.Description = t.Description == null ? string.Empty : t.Description.Trim();
            }

            return ret;
        }

        #region Specific File Readers

        public static List<Transaction> ReadUsaaCheckingPDF(Statement statement)
        {
            List<Transaction> ret = new List<Transaction>();

            string pdfText = FileHelpers.GetPdfText(statement.Filepath);
            string[] lines = pdfText.Split('\n');

            ReadState state = ReadState.Header;
            int linesInEntry = 0;
            Transaction curEntry = null;
            DateTime? statementDate = null;
            foreach (string line in lines)
            {
                string trimmed = line.Trim();
                DateTime? entryDate = TryToScanDate(trimmed, null, statementDate);

                Dictionary<string, ReadState> stateTransitions = new Dictionary<string, ReadState>
                {
                    {"DEPOSITS AND OTHER CREDITS", ReadState.Credit },
                    {"CHECKS", ReadState.Check },
                    {"OTHER DEBITS", ReadState.Debit },
                    {"USAA CLASSIC CHECKING", ReadState.Balance },
                    {"ACCOUNT BALANCE SUMMARY", ReadState.Interim }
                };

                if (CheckForStateTransition(trimmed, stateTransitions, true, ref state))
                {
                    if (curEntry != null)
                    {
                        ret.Add(curEntry);
                        curEntry = null;
                    }
                }

                switch (state)
                {
                    case ReadState.Header:
                        if (entryDate.HasValue && !statementDate.HasValue)
                        {
                            statementDate = entryDate;
                        }
                        break;
                    case ReadState.Balance:
                        List<string> balanceParts = trimmed.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        if (balanceParts.Count == 7)
                        {
                            if (double.TryParse(balanceParts[0], out double start) &&
                            double.TryParse(balanceParts.Last(), out double end))
                            {
                                statement.StartingBalance = start;
                                statement.EndingBalance = end;
                            }
                        }

                        break;
                    case ReadState.Credit:
                    case ReadState.Check:
                    case ReadState.Debit:
                        string[] entries = new string[] { trimmed };
                        bool singleLineEntry = false;
                        if (state == ReadState.Check)
                        {
                            //Look for multiline check entries
                            string lineFooter = "USAA FEDERAL SAVINGS BANK";
                            if (trimmed.EndsWith(lineFooter))
                            {
                                trimmed = trimmed.Substring(0, trimmed.Length - lineFooter.Length);
                                entries[0] = trimmed;
                                singleLineEntry = true;
                            }

                            List<string> entryParts = trimmed.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            if (entryParts.Count >= 6)
                            {
                                entries = new string[]
                                {
                                    string.Join(" ", entryParts.GetRange(0, 3)),
                                    string.Join(" ", entryParts.GetRange(3, 3))
                                };
                            }
                        }

                        foreach (string entry in entries)
                        {
                            entryDate = TryToScanDate(entry, null, statementDate);
                            if (entryDate.HasValue)
                            {
                                //Starting a new entry
                                if (curEntry != null)
                                {
                                    ret.Add(curEntry);
                                }

                                curEntry = new Transaction
                                {
                                    Date = entryDate.Value,
                                    Accounts = new List<string> { statement.AccountName },
                                    Type = state.ToString()
                                };

                                List<string> entryParts = entry.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                entryParts.RemoveAt(0);

                                string amountString = "";
                                if (entryParts.Count > 0)
                                {
                                    //amountString = entryParts[0];
                                    //entryParts.RemoveAt(0);

                                    if (state == ReadState.Check)
                                    {
                                        //Amount comes last for checks
                                        amountString = entryParts.Last();
                                        entryParts.RemoveAt(entryParts.Count - 1);

                                        ////Swap amount and description for checks
                                        //string temp = amountString;
                                        //amountString = typeString;
                                        //typeString = temp;
                                    }
                                    else
                                    {
                                        amountString = entryParts[0];
                                        entryParts.RemoveAt(0);
                                    }
                                }

                                string typeString = FileHelpers.RemoveTrailingNumbers(string.Join(" ", entryParts));

                                double.TryParse(amountString, out double amount);
                                int multiplier = state == ReadState.Credit ? 1 : -1;
                                curEntry.CheckingAmount = amount * multiplier;

                                curEntry.FullType = typeString;

                                linesInEntry = 1;

                                if (singleLineEntry)
                                {
                                    ret.Add(curEntry);
                                    curEntry = null;
                                }
                            }
                            else if (!entry.Equals("FEDERAL"))
                            {
                                //Either continuing an entry or left the section
                                linesInEntry++;
                                //int allowableLines = state == ReadState.Credit ? 3 : 2;
                                if (linesInEntry > 3)
                                {
                                    if (curEntry != null)
                                    {
                                        //Commit the previous entry
                                        ret.Add(curEntry);
                                        curEntry = null;

                                        state = ReadState.Interim;
                                        linesInEntry = 0;
                                    }
                                }
                                else if (curEntry != null)
                                {
                                    curEntry.Description += " " + FileHelpers.RemoveTrailingNumbers(trimmed);
                                }
                            }
                            else
                            {
                                state = ReadState.Interim;
                            }
                        }

                        if (singleLineEntry)
                        {
                            state = ReadState.Interim;
                        }

                        break;
                }

                //Console.WriteLine("{0}: {1} ({2})", state.ToString(), line, entryDate.HasValue ? entryDate.ToString() : "-");
            }

            ret.OrderBy(e => e.Date);

            return ret;
        }

        public static List<Transaction> ReadUsaaCreditPDF(Statement statement)
        {
            List<Transaction> ret = new List<Transaction>();

            string pdfText = FileHelpers.GetPdfText(statement.Filepath);
            string[] lines = pdfText.Split('\n');

            ReadState state = ReadState.Header;
            DateTime? statementDate = null;
            Transaction curEntry = null;
            foreach (string line in lines)
            {
                string trimmed = line.Trim();
                DateTime? entryDate = TryToScanDate(trimmed, "Statement Closing Date", statementDate);

                Dictionary<string, ReadState> stateTransitions = new Dictionary<string, ReadState>
                {
                    {"Payments and Credits", ReadState.Credit },
                    {"Transactions", ReadState.Debit },
                    {"Interest Charged", ReadState.Debit },
                    {"Summary of Account Activity", ReadState.Balance },
                    {"Fees", ReadState.Debit }
                };

                CheckForStateTransition(trimmed, stateTransitions, false, ref state);

                switch (state)
                {
                    case ReadState.Header:
                        if (entryDate.HasValue && !statementDate.HasValue)
                        {
                            statementDate = entryDate;
                        }
                        break;
                    case ReadState.Balance:
                        string searchHeader = "Previous Balance $";
                        if (trimmed.StartsWith(searchHeader))
                        {
                            double.TryParse(trimmed.Substring(searchHeader.Length), out double start);
                            statement.StartingBalance = -1 * start;
                        }
                        else
                        {
                            searchHeader = "New Balance $";
                            if (trimmed.StartsWith(searchHeader))
                            {
                                double.TryParse(trimmed.Substring(searchHeader.Length), out double end);
                                statement.EndingBalance = -1 * end;
                            }
                        }

                        break;
                    case ReadState.Credit:
                    case ReadState.Check:
                    case ReadState.Debit:
                        if (curEntry == null)
                        {
                            if (entryDate.HasValue)
                            {
                                //Starting a new entry
                                List<string> entryParts = trimmed.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                if (entryParts.Count >= 5)
                                {
                                    curEntry = new Transaction
                                    {
                                        Date = entryDate.Value,
                                        Accounts = new List<string> { statement.AccountName },
                                        Type = state.ToString()
                                    };

                                    entryParts.RemoveAt(0);
                                    entryParts.RemoveAt(0);

                                    if (!entryParts[0].Equals("Interest"))
                                    {
                                        entryParts.RemoveAt(0);
                                    }

                                    string amountString = entryParts.Last().Substring(1);
                                    if (amountString.EndsWith("-"))
                                    {
                                        amountString = amountString.Substring(0, amountString.Length - 1);
                                    }

                                    bool gotAmount = double.TryParse(amountString, out double amount);

                                    if (gotAmount)
                                    {
                                        entryParts.RemoveAt(entryParts.Count - 1);
                                    }

                                    curEntry.Description = FileHelpers.RemoveTrailingNumbers(string.Join(" ", entryParts));

                                    if (gotAmount)
                                    {
                                        int multiplier = state == ReadState.Credit ? 1 : -1;
                                        curEntry.CreditAmount = amount * multiplier;

                                        ret.Add(curEntry);
                                        curEntry = null;
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Look for a line containing the amount
                            if (trimmed.StartsWith("$") && double.TryParse(trimmed.Substring(1), out double amount))
                            {
                                int multiplier = state == ReadState.Credit ? 1 : -1;
                                curEntry.CreditAmount = amount * multiplier;

                                ret.Add(curEntry);
                                curEntry = null;
                            }
                            else
                            {
                                curEntry.Description += " " + FileHelpers.RemoveTrailingNumbers(trimmed);
                            }
                        }
                        break;
                }

                //Console.WriteLine("{0}: {1} ({2})", state.ToString(), line, entryDate.HasValue ? entryDate.ToString() : "-");
            }

            ret.OrderBy(e => e.Date);

            return ret;
        }

        public static List<Transaction> ReadUsaaSavingsPDF(Statement statement)
        {
            List<Transaction> ret = new List<Transaction>();

            string pdfText = FileHelpers.GetPdfText(statement.Filepath);
            string[] lines = pdfText.Split('\n');

            ReadState state = ReadState.Header;
            int linesInEntry = 0;
            Transaction curEntry = null;
            DateTime? statementDate = null;
            foreach (string line in lines)
            {
                string trimmed = line.Trim();
                DateTime? entryDate = TryToScanDate(trimmed, null, statementDate);

                Dictionary<string, ReadState> stateTransitions = new Dictionary<string, ReadState>
                {
                    {"DEPOSITS AND OTHER CREDITS", ReadState.Credit },
                    {"CHECKS", ReadState.Check },
                    {"OTHER DEBITS", ReadState.Debit },
                    {"ACCOUNT BALANCE SUMMARY", ReadState.Interim }
                };

                if (CheckForStateTransition(trimmed, stateTransitions, false, ref state))
                {
                    if (curEntry != null)
                    {
                        ret.Add(curEntry);
                        curEntry = null;
                    }
                }

                string searchHeader = "USAA SAVINGS";
                if (state == ReadState.Header && trimmed.StartsWith(searchHeader))
                {
                    state = ReadState.Balance;
                }

                switch (state)
                {
                    case ReadState.Header:
                        if (entryDate.HasValue && !statementDate.HasValue)
                        {
                            statementDate = entryDate;
                        }
                        break;
                    case ReadState.Balance:
                        List<string> balanceParts = trimmed.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        if (balanceParts.Count == 7)
                        {
                            if (double.TryParse(balanceParts[0], out double start))
                            {
                                statement.StartingBalance = start;
                            }

                            if (double.TryParse(balanceParts.Last(), out double end))
                            {
                                statement.EndingBalance = end;
                            }
                        }
                        break;
                    case ReadState.Credit:
                    case ReadState.Check:
                    case ReadState.Debit:
                        if (entryDate.HasValue)
                        {
                            //Starting a new entry
                            if (curEntry != null)
                            {
                                ret.Add(curEntry);
                            }

                            curEntry = new Transaction
                            {
                                Date = entryDate.Value,
                                Accounts = new List<string> { statement.AccountName },
                                Type = state.ToString()
                            };

                            List<string> entryParts = trimmed.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            entryParts.RemoveAt(0);

                            string amountString = "";
                            if (entryParts.Count > 0)
                            {
                                amountString = entryParts[0];
                                entryParts.RemoveAt(0);
                            }

                            string typeString = FileHelpers.RemoveTrailingNumbers(string.Join(" ", entryParts));

                            if (state == ReadState.Check)
                            {
                                //Swap amount and description for checks
                                string temp = amountString;
                                amountString = typeString;
                                typeString = temp;
                            }

                            double.TryParse(amountString, out double amount);
                            int multiplier = state == ReadState.Credit ? 1 : -1;
                            curEntry.SavingsAmount = amount * multiplier;

                            curEntry.FullType = typeString;

                            linesInEntry = 1;
                        }
                        else
                        {
                            //Either continuing an entry or left the section
                            linesInEntry++;
                            if (linesInEntry > 3)
                            {
                                if (curEntry != null)
                                {
                                    //Commit the previous entry
                                    ret.Add(curEntry);
                                    curEntry = null;

                                    state = ReadState.Interim;
                                    linesInEntry = 0;
                                }
                            }
                            else if (curEntry != null)
                            {
                                curEntry.Description = FileHelpers.RemoveTrailingNumbers(trimmed);
                            }
                        }
                        break;
                }

                //Console.WriteLine("{0}: {1} ({2})", state.ToString(), line, entryDate.HasValue ? entryDate.ToString() : "-");
            }

            ret.OrderBy(e => e.Date);

            return ret;
        }

        public static List<Transaction> ReadBjsCreditPDF(Statement statement)
        {
            List<Transaction> ret = new List<Transaction>();

            string pdfText = FileHelpers.GetPdfText(statement.Filepath);
            string[] lines = pdfText.Split('\n');

            ReadState state = ReadState.Header;
            DateTime? statementDate = null;
            Transaction curEntry = null;
            foreach (string line in lines)
            {
                string trimmed = line.Trim();
                DateTime? entryDate = TryToScanDate(trimmed, "Statement closing date", statementDate);

                Dictionary<string, ReadState> stateTransitions = new Dictionary<string, ReadState>
                {
                    {"TRANS DATE TRANSACTION DESCRIPTION/LOCATION AMOUNT", ReadState.Credit }
                };

                CheckForStateTransition(trimmed, stateTransitions, false, ref state);

                switch (state)
                {
                    case ReadState.Header:
                        if (entryDate.HasValue && !statementDate.HasValue)
                        {
                            statementDate = entryDate;
                        }
                        string searchHeader = "Previ ous balance $";
                        if (trimmed.StartsWith(searchHeader))
                        {
                            double.TryParse(trimmed.Substring(searchHeader.Length), out double start);
                            statement.StartingBalance = -1 * start;
                        }
                        else
                        {
                            searchHeader = "New balance $";
                            if (trimmed.StartsWith(searchHeader))
                            {
                                double.TryParse(trimmed.Substring(searchHeader.Length), out double end);
                                statement.EndingBalance = -1 * end;
                            }
                        }
                        break;
                    case ReadState.Credit:
                    case ReadState.Check:
                    case ReadState.Debit:
                        if (entryDate.HasValue)
                        {
                            //Starting a new entry
                            if (curEntry != null)
                            {
                                ret.Add(curEntry);
                                curEntry = null;
                            }

                            List<string> entryParts = trimmed.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            if (entryParts.Count >= 3)
                            {
                                curEntry = new Transaction
                                {
                                    Date = entryDate.Value,
                                    Accounts = new List<string> { statement.AccountName },
                                    Type = state.ToString()
                                };

                                entryParts.RemoveAt(0);

                                string amountString = entryParts.Last();
                                double.TryParse(amountString, out double amount);

                                if (amount != 0)
                                {
                                    entryParts.RemoveAt(entryParts.Count - 1);
                                }

                                curEntry.CreditAmount = -1 * amount;
                                if (curEntry.CreditAmount < 0)
                                {
                                    curEntry.Type = "Debit";
                                }

                                curEntry.Description = FileHelpers.RemoveTrailingNumbers(string.Join(" ", entryParts));

                                //ret.Add(curEntry);
                            }
                        }
                        else if (trimmed.StartsWith("Interest charge on purchases"))
                        {
                            List<string> entryParts = trimmed.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            string amountString = entryParts.Last().Substring(1);
                            double.TryParse(amountString, out double amount);
                            if (amount != 0)
                            {
                                amount *= -1;
                                entryParts.RemoveAt(entryParts.Count - 1);

                                curEntry = new Transaction
                                {
                                    Date = statementDate.Value,
                                    Accounts = new List<string> { statement.AccountName },
                                    Type = "Debit",
                                    CreditAmount = amount,
                                    Description = string.Join(" ", entryParts)
                                };

                                ret.Add(curEntry);
                                curEntry = null;
                            }
                        }
                        else if (!trimmed.StartsWith("(CONTINUED)") && !trimmed.StartsWith("Total for"))
                        {
                            if (curEntry != null)
                            {
                                if (double.TryParse(trimmed, out double amount))
                                {
                                    curEntry.CreditAmount = -1 * amount;
                                    if (curEntry.CreditAmount < 0)
                                    {
                                        curEntry.Type = "Debit";
                                    }

                                    if (curEntry != null)
                                    {
                                        ret.Add(curEntry);
                                        curEntry = null;
                                    }
                                }
                                else
                                {
                                    curEntry.Description = string.Format("{0} {1}", curEntry.Description, trimmed);
                                }
                            }
                        }
                        else
                        {
                            if (curEntry != null)
                            {
                                ret.Add(curEntry);
                                curEntry = null;
                            }
                        }
                        break;
                }

                //Console.WriteLine("{0}: {1} ({2})", state.ToString(), line, entryDate.HasValue ? entryDate.ToString() : "-");
            }

            ret.OrderBy(e => e.Date);

            return ret;
        }

        public static List<Transaction> ReadChaseCreditPDF(Statement statement)
        {
            List<Transaction> ret = new List<Transaction>();

            string pdfText = FileHelpers.GetPdfText(statement.Filepath);
            string[] lines = pdfText.Split('\n');

            ReadState state = ReadState.Header;
            DateTime? statementDate = null;
            foreach (string line in lines)
            {
                string trimmed = line.Trim();
                DateTime? entryDate = TryToScanDate(trimmed, "Opening/Closing Date", statementDate);

                Dictionary<string, ReadState> stateTransitions = new Dictionary<string, ReadState>
                {
                    {"TransactionMerchantNameorTransactionDescription$Amount", ReadState.Credit },
                    {"TransactionMerchantNameorTransactionDescription$AmountRewards", ReadState.Header }
                };

                //Special logic: spacing for the target line was funny sometimes (double or missing)
                //So we'll strip all spacing from the string
                string stripped = trimmed.Replace(" ", "");

                CheckForStateTransition(stripped, stateTransitions, false, ref state);

                switch (state)
                {
                    case ReadState.Header:
                        if (entryDate.HasValue && !statementDate.HasValue)
                        {
                            statementDate = entryDate;
                        }
                        string searchHeader = "Previous Balance $";
                        if (trimmed.StartsWith(searchHeader))
                        {
                            double.TryParse(trimmed.Substring(searchHeader.Length), out double start);
                            statement.StartingBalance = -1 * start;
                        }
                        else
                        {
                            searchHeader = "Previous Balance -$";
                            if (trimmed.StartsWith(searchHeader))
                            {
                                double.TryParse(trimmed.Substring(searchHeader.Length), out double start);
                                statement.StartingBalance = start;
                            }
                            else
                            {
                                searchHeader = "New Balance $";
                                if (trimmed.StartsWith(searchHeader))
                                {
                                    double.TryParse(trimmed.Substring(searchHeader.Length), out double end);
                                    statement.EndingBalance = -1 * end;
                                }
                                else
                                {
                                    searchHeader = "New Balance -$";
                                    if (trimmed.StartsWith(searchHeader))
                                    {
                                        double.TryParse(trimmed.Substring(searchHeader.Length), out double end);
                                        statement.EndingBalance = end;
                                    }
                                }
                            }
                        }
                        break;
                    case ReadState.Credit:
                    case ReadState.Check:
                    case ReadState.Debit:
                        if (entryDate.HasValue)
                        {
                            //Starting a new entry
                            List<string> entryParts = trimmed.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            if (entryParts.Count >= 3)
                            {
                                Transaction curEntry = new Transaction
                                {
                                    Date = entryDate.Value,
                                    Accounts = new List<string> { statement.AccountName },
                                    Type = state.ToString()
                                };

                                entryParts.RemoveAt(0);

                                string amountString = entryParts.Last();
                                entryParts.RemoveAt(entryParts.Count - 1);

                                double.TryParse(amountString, out double amount);
                                curEntry.CreditAmount = -1 * amount;
                                if (curEntry.CreditAmount < 0)
                                {
                                    curEntry.Type = "Debit";
                                }

                                curEntry.Description = FileHelpers.RemoveTrailingNumbers(string.Join(" ", entryParts));

                                ret.Add(curEntry);
                            }

                        }
                        break;
                }

                //Console.WriteLine("{0}: {1} ({2})", state.ToString(), line, entryDate.HasValue ? entryDate.ToString() : "-");
            }

            ret.OrderBy(e => e.Date);

            return ret;
        }

        public static AmazonStatement ReadAmazonCSV(string filepath)
        {
            AmazonStatement ret = new AmazonStatement(filepath);

            string[] lines = File.ReadAllText(filepath).Split('\n');

            AmazonTransaction prevEntry = null;
            foreach (string line in lines)
            {
                List<string> parts = FileHelpers.ParseCSVLine(line);

                if (parts.Count < 30)
                {
                    continue;
                }

                DateTime? date = TryToScanDate(parts[0], null, null);

                if (!date.HasValue)
                {
                    continue;
                }

                string amountStr = parts[29].Substring(1);
                double.TryParse(amountStr, out double amount);

                //Build an entry for this line by itself
                AmazonTransaction subEntry = new AmazonTransaction
                {
                    Date = date.Value,
                    Amount = -1 * amount,
                    Description = parts[2],
                    Category = parts[3],
                    OrderNumber = parts[1]
                };

                //Special code here so we can combine multiple transactions with the same order number into one final transaction with sub-transactions

                AmazonTransaction wrapper = null;
                if (prevEntry != null)
                {
                    //If we had an entry in progress and this entry shares the same order number, add this entry as a sub-transaction
                    if (prevEntry.Items[0].OrderNumber.Equals(subEntry.OrderNumber))
                    {
                        wrapper = prevEntry;
                        wrapper.Amount += subEntry.Amount;
                        wrapper.Category += ";" + subEntry.Category;
                        wrapper.Description += ";" + subEntry.Description;
                    }
                    else
                    {
                        //Commit the previous entry, the new entry starts a new order
                        ret.Transactions.Add(prevEntry);
                        prevEntry = null;
                    }
                }

                if (prevEntry == null)
                {
                    //Create a new wrapper to hold the sub-transactions
                    wrapper = new AmazonTransaction
                    {
                        Date = date.Value,
                        Amount = -1 * amount,
                        Description = parts[2],
                        Category = parts[3],
                        OrderNumber = parts[1],
                    };
                }

                wrapper.Items.Add(subEntry);
                prevEntry = wrapper;
            }

            if (prevEntry != null)
            {
                ret.Transactions.Add(prevEntry);
            }

            return ret;
        }

        #endregion

        static DateTime? TryToScanDate(string line, string statementDateHeader, DateTime? statementDate)
        {
            if (!statementDate.HasValue && statementDateHeader != null)
            {
                if (line.Contains(statementDateHeader))
                {
                    line = line.Substring(statementDateHeader.Length).Trim();
                    if (line.Contains(" - "))
                    {
                        line = line.Split('-')[1].Trim();
                    }
                }
                else
                {
                    return null;
                }
            }

            string firstChunk = line.Split(' ')[0];

            string[] parts = firstChunk.Split('/');
            if (parts.Length < 2)
            {
                return null;
            }

            int.TryParse(parts[0], out int month);
            int.TryParse(parts[1], out int day);

            if (month == 0 || day == 0)
            {
                return null;
            }

            int year;
            if (parts.Length >= 3)
            {
                int.TryParse(parts[2], out year);
            }
            else
            {
                if (statementDate == null)
                {
                    return null;
                }

                year = statementDate.Value.Year;
                if (statementDate.Value.Month == 1 && month == 12)
                {
                    year--;
                }
            }

            if (year < 1000)
            {
                year += 2000;
            }

            return new DateTime(year, month, day);
        }

        static bool CheckForStateTransition(string line, Dictionary<string, ReadState> transitions, bool allowPartial, ref ReadState currentState)
        {
            foreach (KeyValuePair<string, ReadState> transition in transitions)
            {
                if ((allowPartial && line.StartsWith(transition.Key)) || line.Equals(transition.Key))
                {
                    currentState = transition.Value;
                    return true;
                }
            }

            return false;
        }

        #region Cache

        public static Statement LoadStatementFromCache(string pdfPath)
        {
            VerifyCacheDirectoryExists();

            string cachePath = CachePathForPdf(pdfPath);

            if (!File.Exists(cachePath))
            {
                return null;
            }

            //Console.WriteLine("Loading from cache: " + pdfPath);

            return LoadStatementFromCacheFile(cachePath);
        }

        public static Statement LoadStatementFromCacheFile(string filepath)
        {
            using (TextReader reader = File.OpenText(filepath))
            {
                string[] lines = reader.ReadToEnd().Split('\n');

                Statement ret = new Statement(filepath);

                foreach (string line in lines)
                {
                    string[] parts = line.Split('=');

                    if (parts.Length != 2)
                    {
                        continue;
                    }

                    string value = parts[1].Trim();
                    switch (parts[0].Trim())
                    {
                        case "AccountName":
                            ret.AccountName = value;
                            break;
                        case "AccountType":
                            Enum.TryParse(value, out AccountType accountEnum);
                            ret.AccountType = accountEnum;
                            break;
                        case "AccountLabel":
                            ret.AccountLabel = value;
                            break;
                        case "StartingBalance":
                            ret.StartingBalance = double.Parse(value);
                            break;
                        case "EndingBalance":
                            ret.EndingBalance = double.Parse(value);
                            break;
                        case "Transaction":
                            ret.Transactions.Add(CreateTransactionFromSummary(value));
                            break;
                    }
                }

                return ret;
            }
        }

        public static Transaction CreateTransactionFromSummary(string summary)
        {
            string[] parts = summary.Split(',');

            if (parts.Length != 10)
            {
                return null;
            }

            Transaction ret = new Transaction
            {
                Date = DateTime.Parse(parts[0]),
                Accounts = parts[1].Split('/').ToList(),
                TypeId = int.Parse(parts[2]),
                CheckingAmount = double.Parse(parts[3]),
                SavingsAmount = double.Parse(parts[4]),
                CreditAmount = double.Parse(parts[5]),
                Type = parts[6],
                FullType = string.IsNullOrEmpty(parts[7]) ? string.Empty : parts[7].Trim().Replace(';', ','),
                Category = string.IsNullOrEmpty(parts[8]) ? string.Empty: parts[8].Trim().Replace(';', ','),
                Description = string.IsNullOrEmpty(parts[9]) ? string.Empty : parts[9].Trim().Replace(';', ','),
            };

            return ret;
        }

        public static void SaveAllToCache()
        {
            foreach (string accountKey in AllStatements.Keys)
            {
                foreach (Statement statement in AllStatements[accountKey])
                {
                    SaveToCache(statement);
                }
            }
        }

        public static void SaveToCache(Statement statement)
        {
            VerifyCacheDirectoryExists();

            string cachePath = CachePathForPdf(statement.Filepath);

            if (File.Exists(cachePath))
            {
                File.Delete(cachePath);
            }

            //Console.WriteLine("Caching: " + statement.Filepath);

            using (TextWriter writer = File.CreateText(cachePath))
            {
                writer.WriteLine($"AccountName = {statement.AccountName}");
                writer.WriteLine($"AccountType = {statement.AccountType}");
                writer.WriteLine($"AccountLabel = {statement.AccountLabel}");
                writer.WriteLine($"StartingBalance = {statement.StartingBalance}");
                writer.WriteLine($"EndingBalance = {statement.EndingBalance}");

                foreach (Transaction entry in statement.Transactions)
                {
                    writer.WriteLine($"Transaction = {entry.ToString()}");
                }
            }
        }

        static void VerifyCacheDirectoryExists()
        {
            if (!Directory.Exists(Constants.CacheDirectory))
            {
                Directory.CreateDirectory(Constants.CacheDirectory);
            }
        }

        static string CachePathForPdf(string pdfPath)
        {
            string name = Path.GetFileNameWithoutExtension(pdfPath);

            return Path.Combine(Constants.CacheDirectory, name + ".txt");
        }

        #endregion
    }
}
