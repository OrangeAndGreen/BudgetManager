using BudgetManager.Data;
using BudgetManager.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace BudgetManager.Views
{
    public partial class MainForm : Form
    {
        string mDirectory;
        List<string> mLoadWarnings { get; set; }
        List<Transaction> mUnifiedTransactions { get; set; }
        DateTime mStartDate { get; set; }

        public MainForm()
        {
            InitializeComponent();

            warningButton.Visible = false;

            TypeManager.Load();

            mDirectory = Properties.Settings.Default.Directory;
            if(mDirectory == null)
            {
                ChooseDirectory();
            }
            else
            {
                LoadDirectory();
            }
        }

        void loadDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChooseDirectory();
        }

        void identifyButton_Click(object sender, EventArgs e)
        {
            new IdentificationForm(ShowSummary).Show();
        }

        void cleanButton_Click(object sender, EventArgs e)
        {

        }

        void graphsButton_Click(object sender, EventArgs e)
        {
            new GraphsForm(mStartDate).Show();
        }

        void transactionsButton_Click(object sender, EventArgs e)
        {
            new TransactionsForm(mUnifiedTransactions).Show();
        }

        void analysisButton_Click(object sender, EventArgs e)
        {
            new AnalysisForm().Show();
        }

        private void warningButton_Click(object sender, EventArgs e)
        {
            if(mLoadWarnings != null)
            {
                MessageBox.Show(string.Join(Environment.NewLine, mLoadWarnings), "Warnings", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        void SetUiEnabled(bool enabled)
        {
            void doEnable()
            {
                identifyButton.Enabled = enabled;
                cleanButton.Enabled = enabled;
                graphsButton.Enabled = enabled;
                transactionsButton.Enabled = enabled;
                analysisButton.Enabled = enabled;
                menuStrip1.Enabled = enabled;
            }

            if (graphsButton.InvokeRequired)
            {
                graphsButton.Invoke((MethodInvoker)delegate
                {
                    doEnable();
                });
            }
            else
            {
                doEnable();
            }
        }

        void UpdateStatus(string status)
        {
            void doUpdate()
            {
                statusText.Text = status;
            }

            if (statusStrip1.InvokeRequired)
            {
                statusStrip1.Invoke((MethodInvoker)doUpdate);
            }
            else
            {
                doUpdate();
            }
        }

        void ChooseDirectory()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                Description = "Select Budget directory"
            };

            if (!string.IsNullOrEmpty(mDirectory))
            {
                dialog.SelectedPath = mDirectory;
            }

            DialogResult result = dialog.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                mDirectory = dialog.SelectedPath;
                Properties.Settings.Default.Directory = mDirectory;
                Properties.Settings.Default.Save();

                LoadDirectory();
            }
        }

        void LoadDirectory()
        {
            BackgroundWorker bw = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };

            bw.DoWork += delegate (object o, DoWorkEventArgs args)
            {
                SetUiEnabled(false);

                BackgroundWorker worker = o as BackgroundWorker;
                worker?.ReportProgress(0, "Loading data");

                mLoadWarnings = StatementManager.Load(mDirectory);

                worker?.ReportProgress(0, "Unifying transactions");

                mUnifiedTransactions = BudgetAnalyzer.UnifyTransactions(StatementManager.AllStatements, StatementManager.AmazonStatements);
                mStartDate = mUnifiedTransactions[0].Date;

                ShowSummary();

                SetUiEnabled(true);
            };

            bw.ProgressChanged += delegate (object o, ProgressChangedEventArgs args)
            {
                UpdateStatus((string)args.UserState);
            };

            bw.RunWorkerCompleted += delegate
            {
                UpdateStatus("Done");
            };

            bw.RunWorkerAsync();
        }

        void ShowSummary()
        {
            //List<string> lines = new List<string>();

            //lines.Add("Statements:");
            //foreach (string key in StatementManager.AllStatements.Keys)
            //{
            //    lines.Add($"   {key}:");
            //    foreach (BudgetStatement statement in StatementManager.AllStatements[key])
            //    {
            //        DateTime firstDate = statement.Transactions[0].Date;
            //        //string dateString = $"{firstDate.Year}-{firstDate.Month}";
            //        lines.Add($"      {firstDate.ToShortDateString()}: {statement.Transactions.Count} transactions");
            //    }
            //}

            int numAccounts = StatementManager.AllStatements.Keys.Count;
            int numStatements = 0;
            int numTransactions = 0;
            int numToIdentify = 0;
            foreach(string key in StatementManager.AllStatements.Keys)
            {
                numStatements += StatementManager.AllStatements[key].Count;
                foreach(Statement statement in StatementManager.AllStatements[key])
                {
                    numTransactions += statement.Transactions.Count;

                    foreach(Transaction transaction in statement.Transactions)
                    {
                        if(transaction.TypeId <= 0)
                        {
                            numToIdentify++;
                        }
                    }
                }
            }

            string summary = $"{numTransactions} transactions in {numStatements} statements from {numAccounts} accounts ({numToIdentify} need identification)";

            void doUpdate()
            {
                summaryLabel.Text = summary;
                identifyButton.Enabled = numToIdentify > 0;
                warningButton.Visible = mLoadWarnings != null && mLoadWarnings.Count > 0;
            }

            if (summaryLabel.InvokeRequired)
            {
                summaryLabel.Invoke((MethodInvoker)doUpdate);
            }
            else
            {
                doUpdate();
            }
        }
    }
}
