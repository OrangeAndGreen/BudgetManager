using System;
using System.Collections.Generic;

namespace BudgetManager.Data
{
    public class Transaction
    {
        public DateTime Date { get; set; }
        public List<string> Accounts { get; set; }
        public double CheckingAmount { get; set; }
        public double SavingsAmount { get; set; }
        public double CreditAmount { get; set; }
        public string Type { get; set; }
        public string FullType { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }

        public List<Transaction> Items { get; set; }
        public string OrderNumber { get; set; }
        public List<AmazonTransaction> AmazonTransactions { get; set; }

        public Transaction()
        {
            Accounts = new List<string>();
            Items = new List<Transaction>();
            AmazonTransactions = new List<AmazonTransaction>();
        }

        public int TypeId { get; set; }

        public string[] TableEntry
        {
            get
            {
                return new[] {
                    Date.ToString("yyyy/MM/dd"),
                    CheckingAmount == 0 ? string.Empty : $"{CheckingAmount:0.00}",
                    CreditAmount == 0 ? string.Empty : $"{CreditAmount:0.00}",
                    SavingsAmount == 0 ? string.Empty : $"{SavingsAmount:0.00}",
                    string.Join(", ", Accounts),
                    Description
                };
            }
        }

        public override string ToString()
        {
            string fullType = FullType == null ? "" : FullType.Replace(',', ';');
            string description = Description == null ? "" : Description.Replace(',', ';');
            string category = Category == null ? "" : Category.Replace(',', ';');
            return $"{Date.ToShortDateString()},{string.Join("/", Accounts)},{TypeId},{CheckingAmount},{SavingsAmount},{CreditAmount},{Type},{fullType},{category},{description}";
        }

        public void MergeEntry(Transaction slave)
        {
            Accounts.AddRange(slave.Accounts);
            SavingsAmount = slave.SavingsAmount;
            CreditAmount = slave.CreditAmount;
        }
    }
}
