using BudgetManager.Data;
using BudgetManager.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace BudgetManager.Views
{
    public class MainFormController
    {
        Button warningButton { get; set; }
        Button cleanButton { get; set; }
        Button graphsButton { get; set; }
        Button transactionsButton { get; set; }
        Button analysisButton { get; set; }
        Button identifyButton { get; set; }
        StatusStrip statusStrip1 { get; set; }
        MenuStrip menuStrip1 { get; set; }
        ToolStripStatusLabel statusText { get; set; }
        Label summaryLabel { get; set; }

        string mDirectory;
        List<string> mLoadWarnings { get; set; }
        List<Transaction> mUnifiedTransactions { get; set; }
        DateTime mStartDate { get; set; }

        public MainFormController(Button warningButtonIn,
                                    Button cleanButtonIn,
                                    Button graphsButtonIn,
                                    Button transactionsButtonIn,
                                    Button analysisButtonIn,
                                    Button identifyButtonIn,
                                    StatusStrip statusStrip1In,
                                    MenuStrip menuStrip1In,
                                    ToolStripStatusLabel statusTextIn,
                                    Label summaryLabelIn)
        {
            warningButton = warningButtonIn;
            cleanButton = cleanButtonIn;
            graphsButton = graphsButtonIn;
            transactionsButton = transactionsButtonIn;
            analysisButton = analysisButtonIn;
            identifyButton = identifyButtonIn;
            statusStrip1 = statusStrip1In;
            menuStrip1 = menuStrip1In;
            statusText = statusTextIn;
            summaryLabel = summaryLabelIn;

            warningButton.Visible = false;

            TypeManager.Load();

            mDirectory = Properties.Settings.Default.Directory;
            if (mDirectory == null)
            {
                ChooseDirectory();
            }
            else
            {
                LoadDirectory();
            }
        }

        public void loadDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChooseDirectory();
        }

        public void identifyButton_Click(object sender, EventArgs e)
        {
            new IdentificationForm(ShowSummary).Show();
        }

        public void cleanButton_Click(object sender, EventArgs e)
        {

        }

        public void graphsButton_Click(object sender, EventArgs e)
        {
            new GraphsForm(mStartDate).Show();
        }

        public void transactionsButton_Click(object sender, EventArgs e)
        {
            new TransactionsForm(mUnifiedTransactions).Show();
        }

        public void analysisButton_Click(object sender, EventArgs e)
        {
            new AnalysisForm().Show();
        }

        public void warningButton_Click(object sender, EventArgs e)
        {
            if (mLoadWarnings != null)
            {
                MessageBox.Show(string.Join(Environment.NewLine, mLoadWarnings), "Warnings", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void SetUiEnabled(bool enabled)
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

        public void UpdateStatus(string status)
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

        public void ChooseDirectory()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                Description = "Select Budget directory"
            };

            if (!string.IsNullOrEmpty(mDirectory))
            {
                dialog.SelectedPath = mDirectory;
            }

            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                mDirectory = dialog.SelectedPath;
                Properties.Settings.Default.Directory = mDirectory;
                Properties.Settings.Default.Save();

                LoadDirectory();
            }
        }

        public void LoadDirectory()
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

        public void ShowSummary()
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
            foreach (string key in StatementManager.AllStatements.Keys)
            {
                numStatements += StatementManager.AllStatements[key].Count;
                foreach (Statement statement in StatementManager.AllStatements[key])
                {
                    numTransactions += statement.Transactions.Count;

                    foreach (Transaction transaction in statement.Transactions)
                    {
                        if (transaction.TypeId <= 0)
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
