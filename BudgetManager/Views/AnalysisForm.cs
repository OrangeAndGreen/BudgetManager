using BudgetManager.Logic;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BudgetManager.Views
{
    public partial class AnalysisForm : Form
    {
        public AnalysisForm()
        {
            InitializeComponent();
        }

        void vendorAnalysisButton_Click(object sender, EventArgs e)
        {
            BackgroundWorker bw = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };

            bw.DoWork += delegate (object o, DoWorkEventArgs args)
            {
                SetUiEnabled(false);

                BackgroundWorker worker = o as BackgroundWorker;
                if (worker != null)
                {
                    worker.ReportProgress(0, "Analyzing vendors");
                }

                UpdateResults(BudgetAnalyzer.AnalyzeVendors(StatementManager.AllStatements));

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

        void SetUiEnabled(bool enabled)
        {
            void doEnable()
            {
                vendorAnalysisButton.Enabled = enabled;
            }

            if (vendorAnalysisButton.InvokeRequired)
            {
                vendorAnalysisButton.Invoke((MethodInvoker)delegate
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
                statusStrip1.Invoke((MethodInvoker)delegate
                {
                    doUpdate();
                });
            }
            else
            {
                doUpdate();
            }
        }

        void UpdateResults(string results)
        {
            void doUpdate()
            {
                analysisTextbox.Text = results;
            }

            if (analysisTextbox.InvokeRequired)
            {
                analysisTextbox.Invoke((MethodInvoker)delegate
                {
                    doUpdate();
                });
            }
            else
            {
                doUpdate();
            }
        }
    }
}
