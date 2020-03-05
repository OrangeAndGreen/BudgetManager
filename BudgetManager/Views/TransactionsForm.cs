using BudgetManager.Data;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BudgetManager.Views
{
    public partial class TransactionsForm : Form
    {
        List<Transaction> mTransactions { get; set; }

        public TransactionsForm(List<Transaction> transactions)
        {
            InitializeComponent();
            mTransactions = transactions;

            DrawTransactions();
        }

        void DrawTransactions()
        {
            transactionsGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            transactionsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] columnNames = {
                "Date",
                "Checking",
                "Credit",
                "Savings",
                "Accounts",
                "Description"
            };

            transactionsGrid.ColumnCount = columnNames.Length;

            for (int i = 0; i < columnNames.Length; i++)
            {
                transactionsGrid.Columns[i].Name = columnNames[i];
            }

            foreach (Transaction entry in mTransactions)
            {
                transactionsGrid.Rows.Add(entry.TableEntry);
            }
        }
    }
}
