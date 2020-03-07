using BudgetManager.Data;
using BudgetManager.Logic;
using System;
using System.Collections.Generic;
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
            //List and map existing transaction types
            List<string> list = new List<string>();
            mExistingTypeLookups = new Dictionary<string, int>();
            foreach (int id in TypeManager.Types.Keys)
            {
                TransactionType curType = TypeManager.Types[id];
                string description = $"{curType.Vendor}: {curType.Identifier}";
                list.Add(description);
                mExistingTypeLookups[description] = curType.Id;
            }

            transactionTypeList.Items.Clear();
            list.Sort();
            foreach (string str in list)
            {
                transactionTypeList.Items.Add(str);
            }

            //Find and list unidentified transactions
            list = new List<string>();
            foreach(string accountKey in StatementManager.AllStatements.Keys)
            {
                foreach(Statement statement in StatementManager.AllStatements[accountKey])
                {
                    foreach(Transaction transaction in statement.Transactions)
                    {
                        if(transaction.TypeId <= 0 && !string.IsNullOrEmpty(transaction.Description))
                        {
                            list.Add($"{transaction.Description}");
                        }
                    }
                }
            }

            unidentifiedTransactionsList.Items.Clear();
            list.Sort();
            foreach (string str in list)
            {
                unidentifiedTransactionsList.Items.Add(str);
            }

            UpdateSaveAndDelete();
        }

        void transactionTypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string description = (string)transactionTypeList.SelectedItem;

            if (description != null)
            {
                unidentifiedTransactionsList.ClearSelected();

                TransactionType toLoad = TypeManager.Get(mExistingTypeLookups[description]);

                LoadType(toLoad);
            }
        }

        void unidentifiedTransactionsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string description = (string)unidentifiedTransactionsList.SelectedItem;

            if (description != null)
            {
                transactionTypeList.ClearSelected();

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
