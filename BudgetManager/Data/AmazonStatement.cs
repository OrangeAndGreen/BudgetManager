using System.Collections.Generic;

namespace BudgetManager.Data
{
    public class AmazonStatement
    {
        public string Filepath { get; set; }

        public List<AmazonTransaction> Transactions { get; set; }

        public AmazonStatement(string filepath)
        {
            Filepath = filepath;

            Transactions = new List<AmazonTransaction>();
        }
    }
}
