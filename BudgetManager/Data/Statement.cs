using System.Collections.Generic;

namespace BudgetManager.Data
{
    public class Statement
    {
        public string Filepath { get; set; }
        public string AccountName { get; set; }
        public AccountType AccountType { get; set; }
        public string AccountLabel { get; set; }

        public double StartingBalance { get; set; }
        public double EndingBalance { get; set; }

        public List<Transaction> Transactions { get; set; }

        public Statement(string filepath)
        {
            Filepath = filepath;

            Transactions = new List<Transaction>();
        }
    }
}
