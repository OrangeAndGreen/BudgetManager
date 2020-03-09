using System;
using System.Windows.Forms;

namespace BudgetManager.Views
{
    public partial class MainForm_Dark : Form
    {
        MainFormController mController;
        public MainForm_Dark()
        {
            InitializeComponent();

            mController = new MainFormController(warningButton, mergeButton, graphsButton, transactionsButton, analysisButton, identifyButton, statusStrip1, menuStrip1, statusText, summaryLabel);
        }

        void warningButton_Click(object sender, EventArgs e)
        {
            mController.warningButton_Click(sender, e);
        }

        void mergeButton_Click(object sender, EventArgs e)
        {
            mController.mergeButton_Click(sender, e);
        }

        void identifyButton_Click(object sender, EventArgs e)
        {
            mController.identifyButton_Click(sender, e);
        }

        void graphsButton_Click(object sender, EventArgs e)
        {
            mController.graphsButton_Click(sender, e);
        }

        void transactionsButton_Click(object sender, EventArgs e)
        {
            mController.transactionsButton_Click(sender, e);
        }

        void analysisButton_Click(object sender, EventArgs e)
        {
            mController.analysisButton_Click(sender, e);
        }

        void loadDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mController.loadDirectoryToolStripMenuItem_Click(sender, e);
        }
    }
}
