using BudgetManager.Logic;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace BudgetManager.Views
{
    public partial class GraphsForm : Form
    {
        DateTime mStartDate { get; }

        public GraphsForm(DateTime startDate)
        {
            InitializeComponent();

            graphSelector.Items.Clear();

            graphSelector.Items.Add("All");
            graphSelector.Items.Add("Net Worth");

            graphSelector.SelectedIndex = 0;

            mStartDate = startDate;
        }

        void graphDrawButton_Click(object sender, EventArgs e)
        {
            string graphSelection = (string)graphSelector.SelectedItem;

            BackgroundWorker bw = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };

            bw.DoWork += delegate (object o, DoWorkEventArgs args)
            {
                SetUiEnabled(false);

                BackgroundWorker worker = o as BackgroundWorker;
                worker?.ReportProgress(0, "Loading data");

                DrawGraph(graphSelection);

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
                graphDrawButton.Enabled = enabled;
            }

            if (graphDrawButton.InvokeRequired)
            {
                graphDrawButton.Invoke((MethodInvoker)doEnable);
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

        void DrawGraph(string graphType)
        {
            void doGraph()
            {
                mainPlot.Series = new SeriesCollection();

                Dictionary<string, LineSeries> plots = BudgetAnalyzer.GeneratePlots(StatementManager.AllStatements, mStartDate);

                foreach (string account in plots.Keys)
                {
                    if (account.Equals("Net") || graphType.Equals("All"))
                    {
                        mainPlot.Series.Add(plots[account]);
                    }
                }

                mainPlot.LegendLocation = LegendLocation.Right;
            }

            if (mainPlot.InvokeRequired)
            {
                mainPlot.Invoke((MethodInvoker)doGraph);
            }
            else
            {
                doGraph();
            }
        }
    }
}
