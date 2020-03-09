using BudgetManager.Data;
using BudgetManager.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BudgetManager.Views
{
    public partial class IdentificationForm : Form
    {
        Action mUpdateCallback { get; set; }
        TransactionType mLoadedType { get; set; }
        Dictionary<string, int> mExistingTypeLookups { get; set; }

        public IdentificationForm(Action updateCallback)
        {
            InitializeComponent();
            mUpdateCallback = updateCallback;

            RefreshUi();
        }

        void RefreshUi()
        {
            existingGrid.Rows.Clear();
            existingGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            existingGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            unidentifiedGrid.Rows.Clear();
            unidentifiedGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            unidentifiedGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] existinColumnNames = {
                "Count",
                "Vendor",
                "Category",
                "Pattern"
            };
            string[] unidentifiedColumnNames = {
                "Count",
                "Description"
            };

            existingGrid.ColumnCount = existinColumnNames.Length;
            unidentifiedGrid.ColumnCount = unidentifiedColumnNames.Length;

            for (int i = 0; i < existinColumnNames.Length; i++)
            {
                existingGrid.Columns[i].Name = existinColumnNames[i];
            }

            for (int i = 0; i < unidentifiedColumnNames.Length; i++)
            {
                unidentifiedGrid.Columns[i].Name = unidentifiedColumnNames[i];
            }

            Dictionary<string, int> existingCounts = new Dictionary<string, int>();
            Dictionary<string, int> unidentified = new Dictionary<string, int>();

            //List and map existing transaction types
            foreach (int id in TypeManager.Types.Keys)
            {
                TransactionType curType = TypeManager.Types[id];
                string description = $"{curType.Vendor}: {curType.Identifier}";
                existingCounts[description] = 0;
            }

            //Find and list unidentified transactions
            foreach(string accountKey in StatementManager.AllStatements.Keys)
            {
                foreach(Statement statement in StatementManager.AllStatements[accountKey])
                {
                    foreach(Transaction transaction in statement.Transactions)
                    {
                        if(transaction.TypeId <= 0)
                        {
                            string description = string.IsNullOrEmpty(transaction.Description) ? "(blank)" : transaction.Description;
                            if (unidentified.ContainsKey(description))
                            {
                                unidentified[description]++;
                            }
                            else
                            {
                                unidentified[description] = 1;
                            }
                        }
                        else
                        {
                            TransactionType curType = TypeManager.Get(transaction.TypeId);
                            string description = $"{curType.Vendor}: {curType.Identifier}";
                            existingCounts[description]++;
                        }
                    }
                }
            }

            mExistingTypeLookups = new Dictionary<string, int>();
            foreach (int id in TypeManager.Types.Keys)
            {
                TransactionType curType = TypeManager.Types[id];
                string description = $"{curType.Vendor}: {curType.Identifier}";
                mExistingTypeLookups[description] = curType.Id;

                existingGrid.Rows.Add(new [] { $"{existingCounts[description]:000}", curType.Vendor, curType.Category, curType.Identifier });
            }

            foreach (string key in unidentified.Keys)
            {
                unidentifiedGrid.Rows.Add(new[] { $"{unidentified[key]:000}", key });
            }

            unidentifiedGrid.Sort(unidentifiedGrid.Columns["Count"], ListSortDirection.Descending);
            existingGrid.Sort(existingGrid.Columns["Vendor"], ListSortDirection.Ascending);

            UpdateSaveAndDelete();
        }

        private void existingGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = existingGrid.SelectedRows[0];
            string description = $"{row.Cells[1].Value}: {row.Cells[3].Value}";
            unidentifiedGrid.ClearSelection();

            TransactionType toLoad = TypeManager.Get(mExistingTypeLookups[description]);

            LoadType(toLoad);
        }

        private void unidentifiedGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = unidentifiedGrid.Rows[e.RowIndex];
                string description = $"{row.Cells[1].Value}";
                existingGrid.ClearSelection();

                LoadType(new TransactionType()
                {
                    Vendor = string.Empty,
                    Category = string.Empty,
                    Identifier = description.Trim().Split(' ')[0],
                    IdentifierMustStart = true,
                    IdentifierCaseSensitive = true
                });
            }
        }

        void vendorText_TextChanged(object sender, EventArgs e)
        {
            UpdateSaveAndDelete();
        }

        void categoryText_TextChanged(object sender, EventArgs e)
        {
            UpdateSaveAndDelete();
        }

        void identifierText_TextChanged(object sender, EventArgs e)
        {
            UpdateSaveAndDelete();
            UpdateMatchesList();
        }

        void caseSensitiveCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSaveAndDelete();
            UpdateMatchesList();
        }

        void startsWithCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSaveAndDelete();
            UpdateMatchesList();
        }

        void saveButton_Click(object sender, EventArgs e)
        {
            TransactionType toSave = new TransactionType()
            {
                Id = mLoadedType != null && mLoadedType.Id > 0 ? mLoadedType.Id : -1,
                Vendor = vendorText.Text,
                Category = categoryText.Text,
                Identifier = identifierText.Text,
                IdentifierCaseSensitive = caseSensitiveCheckbox.Checked,
                IdentifierMustStart = startsWithCheckbox.Checked
            };

            TypeManager.Add(toSave);
            mLoadedType = toSave;

            //Scan all transactions and see where the new type can be applied
            List<Transaction> matches = FindMatches(toSave.Identifier, toSave.IdentifierMustStart, toSave.IdentifierCaseSensitive);
            foreach (Transaction transaction in matches)
            {
                transaction.TypeId = toSave.Id;
            }
            StatementManager.SaveAllToCache();

            mUpdateCallback?.Invoke();

            RefreshUi();

            string description = $"{toSave.Vendor}: {toSave.Identifier}";
            foreach(DataGridViewRow row in existingGrid.Rows)
            {
                if(row.Cells[1].Value != null && row.Cells[1].Value.Equals(description))
                {
                    row.Selected = true;
                    break;
                }
            }
        }

        void deleteButton_Click(object sender, EventArgs e)
        {
            //TODO
        }

        void LoadType(TransactionType transactionType)
        {
            mLoadedType = transactionType;

            vendorText.Text = transactionType.Vendor;
            categoryText.Text = transactionType.Category;
            identifierText.Text = transactionType.Identifier;
            startsWithCheckbox.Checked = transactionType.IdentifierMustStart;
            caseSensitiveCheckbox.Checked = transactionType.IdentifierCaseSensitive;

            saveButton.Enabled = false;
            deleteButton.Enabled = transactionType.Id > 0;

            UpdateMatchesList();
            UpdateSaveAndDelete();
        }

        bool IsDirty()
        {
            if (mLoadedType == null)
            {
                return false;
            }

            return !vendorText.Text.Equals(mLoadedType.Vendor) ||
                   !categoryText.Text.Equals(mLoadedType.Category) ||
                   !identifierText.Text.Equals(mLoadedType.Identifier) ||
                    startsWithCheckbox.Checked != mLoadedType.IdentifierMustStart ||
                    caseSensitiveCheckbox.Checked != mLoadedType.IdentifierCaseSensitive;
        }

        void UpdateSaveAndDelete(bool forceNoSave = false)
        {
            saveButton.Enabled =!forceNoSave && ((mLoadedType != null && mLoadedType.Id <= 0) || IsDirty());
            deleteButton.Enabled = mLoadedType != null && mLoadedType.Id > 0;
        }

        static List<Transaction> FindMatches(string identifier, bool startsWith, bool caseSensitive)
        {
            List<Transaction> list = new List<Transaction>();
            foreach (string accountKey in StatementManager.AllStatements.Keys)
            {
                foreach (Statement statement in StatementManager.AllStatements[accountKey])
                {
                    foreach (Transaction transaction in statement.Transactions)
                    {
                        if (transaction.Description != null && TransactionType.DoesIdentifierMatch(identifier, transaction.Description, caseSensitive, startsWith))
                        {
                            list.Add(transaction);
                        }
                    }
                }
            }

            return list;
        }

        void UpdateMatchesList()
        {
            transactionsGrid.Rows.Clear();
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

            List<Transaction> matches = FindMatches(identifierText.Text, startsWithCheckbox.Checked, caseSensitiveCheckbox.Checked);

            matches.Sort((s1, s2) => s1.Description.CompareTo(s2.Description));

            //List<string[]> rows = new List<string[]>();
            //List<bool> conflicts = new List<bool>();
            bool foundConflicts = false;
            foreach (Transaction transaction in matches)
            {
                //rows.Add(transaction.TableEntry);
                //conflicts.Add(transaction.TypeId > 0);

                transactionsGrid.Rows.Add(transaction.TableEntry);
                if (transaction.TypeId > 0 && (mLoadedType == null || mLoadedType.Id != transaction.TypeId) )
                {
                    foundConflicts = true;
                    transactionsGrid.Rows[transactionsGrid.Rows.Count - 2].DefaultCellStyle.BackColor = Constants.ErrorColor;
                }
            }

            //transactionsGrid.Rows.AddRange(rows.ToArray());
            UpdateSaveAndDelete(foundConflicts);
        }
    }
}
