using BudgetManager.Data;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetManager
{
    public static class BudgetAnalyzer
    {
        public static List<Transaction> UnifyTransactions(Dictionary<string, List<Statement>> allStatements)
        {
            //Extract all entries from statements
            Dictionary<string, List<Transaction>> allEntries = new Dictionary<string, List<Transaction>>();
            foreach (string key in allStatements.Keys)
            {
                allEntries[key] = new List<Transaction>();
                foreach (Statement statement in allStatements[key])
                {
                    allEntries[key].AddRange(statement.Transactions);
                }

                allEntries[key] = allEntries[key].OrderBy(e => e.Date).ToList();
            }

            //Integrate entries that span more than one account (i.e. transfers)
            Console.WriteLine("Combining entries");
            string checkingKey = "Checking - USAA";
            string amazonKey = "Amazon";
            List<Transaction> checkingList = allEntries[checkingKey];
            foreach (string account in allEntries.Keys)
            {
                if (!account.Equals(checkingKey) && !account.Equals(amazonKey))
                {
                    List<Transaction> accountList = allEntries[account];
                    for (int i = 0; i < accountList.Count; i++)
                    {
                        int searchIndex = accountList.Count - 1 - i;
                        Transaction possibleMatch = accountList[searchIndex];
                        int index = FindMatch(possibleMatch, checkingList, 7, false);

                        if (index >= 0)
                        {
                            checkingList[index].MergeEntry(possibleMatch);
                            accountList.RemoveAt(searchIndex);
                        }
                    }
                }
            }

            //Combine all entries (except Amazon) into one big list
            List<Transaction> combined = new List<Transaction>();
            foreach (string account in allEntries.Keys)
            {
                if (!account.Equals(amazonKey))
                {
                    combined.AddRange(allEntries[account]);
                }

                //Console.WriteLine("'{0}' transactions:", account);
                //foreach (Transaction entry in allEntries[account])
                //{
                //    Console.WriteLine("   {0}", entry);
                //}
            }

            //Integrate Amazon entries into the list
            Console.WriteLine("Adding Amazon Info");
            List<Transaction> amazonList = allEntries[amazonKey];
            foreach (Transaction amazonItem in amazonList)
            {
                int index = FindMatch(amazonItem, combined, 45, true);
                if (index >= 0)
                {
                    combined[index].Accounts.Add(amazonKey);
                    combined[index].AmazonCategory = amazonItem.AmazonCategory;
                    combined[index].AmazonDescription = amazonItem.AmazonDescription;
                }
                else
                {
                    //Console.WriteLine("Failed to integrate Amazon entry: {0}", amazonItem);
                    combined.Add(amazonItem);
                }
            }

            return combined.OrderBy(e => e.Date).ToList();
        }

        public static int FindMatch(Transaction transaction, List<Transaction> checkingList, int windowDays, bool matchAny)
        {
            string matchStarter = null;
            if (!matchAny)
            {
                Dictionary<string, string> matchingLabels = new Dictionary<string, string>
                {
                    { "USAA CREDIT CARD PAYMENT", "CREDIT CARD ENDING IN" },
                    { "Payment Thank You", "CHASE CREDIT CRD" },
                    { "PAYMENT-THANK YOU", "COMENITY PAY" },
                    { "USAA FUNDS TRANSFER CR", "USAA FUNDS TRANSFER DB" },
                    { "USAA FUNDS TRANSFER DB", "USAA FUNDS TRANSFER CR" }
                };

                string myLabel = transaction.Description;
                if (transaction.Description == null || transaction.Description.Length == 0)
                {
                    myLabel = transaction.FullType;
                }

                foreach (string key in matchingLabels.Keys)
                {
                    if (myLabel.StartsWith(key))
                    {
                        matchStarter = matchingLabels[key];
                        break;
                    }
                }

                if (matchStarter == null)
                {
                    return -1;
                }
            }


            for (int i = 0; i < checkingList.Count; i++)
            {
                Transaction searchEntry = checkingList[i];
                string searchLabel = searchEntry.Description;
                if (searchEntry.Description == null || searchEntry.Description.Length == 0)
                {
                    searchLabel = searchEntry.FullType;
                }

                if (searchLabel != null)
                {
                    searchLabel = searchLabel.Trim();

                    double myAmount = transaction.CheckingAmount == 0 ? (transaction.SavingsAmount == 0 ? transaction.CreditAmount : transaction.SavingsAmount) : transaction.CheckingAmount;
                    double theirAmount = searchEntry.CheckingAmount == 0 ? (searchEntry.SavingsAmount == 0 ? searchEntry.CreditAmount : searchEntry.SavingsAmount) : searchEntry.CheckingAmount;
                    double daysDiff = Math.Abs((searchEntry.Date - transaction.Date).TotalDays);

                    if (searchEntry.Accounts.Count == 1 &&
                        (Math.Abs(theirAmount + myAmount) < 0.01 || (matchAny && Math.Abs(myAmount - theirAmount) < 0.01)) &&
                        daysDiff < windowDays &&
                        (matchAny || searchLabel.StartsWith(matchStarter))
                        )
                    {
                        //Console.WriteLine("   Combining {0}", this);
                        //Console.WriteLine("        into {0}", searchEntry);
                        return i;
                    }
                }
            }

            Console.WriteLine("Failed to find match for entry: {0}", transaction);
            return -1;
        }

        //TODO Vendor-specific code
        public static void AnalyzeInfo(Dictionary<string, List<Statement>> allStatements)
        {
            List<string> descriptions = new List<string>();
            List<string> amazonDescriptions = new List<string>();
            List<string> amazonCategories = new List<string>();
            foreach (string account in allStatements.Keys)
            {
                foreach (Statement statement in allStatements[account])
                {
                    foreach (Transaction entry in statement.Transactions)
                    {
                        if (!descriptions.Contains(entry.Description))
                        {
                            descriptions.Add(entry.Description);
                        }

                        if (!amazonDescriptions.Contains(entry.AmazonDescription))
                        {
                            amazonDescriptions.Add(entry.AmazonDescription);
                        }

                        if (!amazonCategories.Contains(entry.AmazonCategory))
                        {
                            amazonCategories.Add(entry.AmazonCategory);
                        }
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine("Descriptions:");
            descriptions.Sort();
            foreach (string description in descriptions)
            {
                Console.WriteLine(description);
            }
            Console.WriteLine();

            Console.WriteLine("Amazon Categories:");
            amazonCategories.Sort();
            foreach (string category in amazonCategories)
            {
                Console.WriteLine(category);
            }
            Console.WriteLine();

            Console.WriteLine("Amazon Descriptions:");
            amazonDescriptions.Sort();
            foreach (string description in amazonDescriptions)
            {
                Console.WriteLine(description);
            }
        }

        //TODO Vendor-specific code
        public static Dictionary<string, LineSeries> GeneratePlots(Dictionary<string, List<Statement>> allStatements, DateTime startDate)
        {
            Dictionary<string, LineSeries> ret = new Dictionary<string, LineSeries>();
            double netStart = 0;
            double balance = 0;
            List<Transaction> netEntries = new List<Transaction>();
            var mapper = Mappers.Xy<KeyValuePair<double, double>>().X(v => v.Key).Y(v => v.Value);
            foreach (string account in allStatements.Keys)
            {
                if (!account.Equals("Amazon"))
                {
                    balance = -1000000;

                    List<Transaction> allEntries = new List<Transaction>();
                    foreach (Statement statement in allStatements[account])
                    {
                        if (balance == -1000000)
                        {
                            balance = statement.StartingBalance;
                            netStart += statement.StartingBalance;
                        }

                        foreach (Transaction entry in statement.Transactions)
                        {
                            allEntries.Add(entry);
                            netEntries.Add(entry);
                        }
                    }

                    allEntries = allEntries.OrderBy(e => e.Date).ToList();


                    ChartValues<KeyValuePair<double, double>> entries = new ChartValues<KeyValuePair<double, double>>();
                    foreach (Transaction entry in allEntries)
                    {
                        double amount = entry.CheckingAmount == 0 ? (entry.SavingsAmount == 0 ? entry.CreditAmount : entry.SavingsAmount) : entry.CheckingAmount;
                        balance += amount;
                        entries.Add(new KeyValuePair<double, double>((entry.Date - startDate).TotalDays, balance));
                    }

                    ret[account] = new LineSeries(mapper)
                    {
                        Title = account,//.Replace(' ', '_').Replace('-','_');
                        Values = entries
                    };
                }
            }

            netEntries = netEntries.OrderBy(e => e.Date).ToList();
            ChartValues<KeyValuePair<double, double>> netPlot = new ChartValues<KeyValuePair<double, double>>();
            balance = netStart;
            foreach (Transaction entry in netEntries)
            {
                double amount = entry.CheckingAmount == 0 ? (entry.SavingsAmount == 0 ? entry.CreditAmount : entry.SavingsAmount) : entry.CheckingAmount;
                balance += amount;
                netPlot.Add(new KeyValuePair<double, double>((entry.Date - startDate).TotalDays, balance));
            }

            ret["Net"] = new LineSeries(mapper)
            {
                Title = "Net",
                Values = netPlot
            };

            return ret;
        }

        public static string AnalyzeVendors(Dictionary<string, List<Statement>> allStatements)
        {
            //Goal: identify unique vendors from transaction descriptions

            List<string> lines = new List<string>();

            foreach(string key in allStatements.Keys)
            {
                //lines.Add($"{key}:");
                for (int statementIndex = 0; statementIndex < allStatements[key].Count; statementIndex++)
                {
                    Statement statement = allStatements[key][statementIndex];
                    //Compare each description with all descriptions after it
                    for(int i=0; i<statement.Transactions.Count; i++)
                    {
                        int lastNumMatches = 0;
                        string bestMatch = null;
                        int bestNumMatches = 0;
                        string originalDescription = statement.Transactions[i].Description;
                        if (originalDescription != null)
                        {
                            string[] parts = originalDescription.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                            //Check all of the subsets of the split up description
                            for (int subsetLength = 1; subsetLength <= parts.Length; subsetLength++)
                            {
                                List<string> subsetParts = new List<string>();
                                for (int subsetCounter = 0; subsetCounter < subsetLength; subsetCounter++)
                                {
                                    subsetParts.Add(parts[subsetCounter]);
                                }

                                string subset = string.Join(" ", subsetParts);

                                //Now look for other transactions that start with this text
                                int matches = 0;
                                foreach (string searchKey in allStatements.Keys)
                                {
                                    for (int searchStatementIndex = 0; searchStatementIndex < allStatements[searchKey].Count; searchStatementIndex++)
                                    {
                                        Statement searchStatement = allStatements[searchKey][searchStatementIndex];
                                        for (int j = 0; j < searchStatement.Transactions.Count; j++)
                                        {
                                            if (searchStatement.Transactions[j].Description != null &&
                                                searchStatement.Transactions[j].Description.Trim().StartsWith(subset))
                                            {
                                                matches++;
                                            }
                                        }
                                    }
                                }

                                //TODO DAV: Don't always want to add like this, this code is for testing the algorithm...

                                if(matches == lastNumMatches)
                                {
                                    lines.RemoveAt(lines.Count - 1);
                                }

                                //lines.Add($"{originalDescription} >> {subset} with {matches} matches");
                                string line = $"{subset}: {matches}";
                                if (!lines.Contains(line))
                                {
                                    lines.Add(line);
                                }

                                if (matches > 0 && matches >= bestNumMatches)
                                {
                                    bestMatch = subset;
                                    bestNumMatches = matches;
                                }

                                lastNumMatches = matches;
                            }
                        }

                        if (bestMatch != null)
                        {
                            //lines.Add($"{originalDescription} >> {bestMatch} with {bestNumMatches} matches");
                        }
                    }
                }
            }
            lines.Sort();
            return string.Join(Environment.NewLine, lines);
        }
    }
}
