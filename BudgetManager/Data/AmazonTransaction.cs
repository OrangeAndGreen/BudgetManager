using System;
using System.Collections.Generic;

namespace BudgetManager.Data
{
    public class AmazonTransaction
    {
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }

        public List<AmazonTransaction> Items { get; set; }
        public string OrderNumber { get; set; }

        public int TypeId { get; set; }

        public AmazonTransaction()
        {
            Items = new List<AmazonTransaction>();
        }

        public string[] TableEntry
        {
            get
            {
                return new[] {
                    Date.ToShortDateString(),
                    Description
                };
            }
        }

        public override string ToString()
        {
            string description = Description == null ? "" : Description.Replace(',', ';');
            string category = Category == null ? "" : Category.Replace(',', ';');
            return $"{Date.ToShortDateString()},{TypeId},{Amount},{category},{description}";
        }
    }
}
