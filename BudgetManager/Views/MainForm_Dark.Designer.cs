namespace BudgetManager.Views
{
    partial class MainForm_Dark
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.darkLabel1 = new DarkUI.Controls.DarkLabel();
            this.summaryLabel = new DarkUI.Controls.DarkLabel();
            this.statusStrip1 = new DarkUI.Controls.DarkStatusStrip();
            this.identifyButton = new DarkUI.Controls.DarkButton();
            this.cleanButton = new DarkUI.Controls.DarkButton();
            this.graphsButton = new DarkUI.Controls.DarkButton();
            this.transactionsButton = new DarkUI.Controls.DarkButton();
            this.analysisButton = new DarkUI.Controls.DarkButton();
            this.warningButton = new DarkUI.Controls.DarkButton();
            this.statusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.menuStrip1 = new DarkUI.Controls.DarkMenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // darkLabel1
            // 
            this.darkLabel1.AutoSize = true;
            this.darkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.darkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel1.Location = new System.Drawing.Point(7, 27);
            this.darkLabel1.Name = "darkLabel1";
            this.darkLabel1.Size = new System.Drawing.Size(332, 25);
            this.darkLabel1.TabIndex = 0;
            this.darkLabel1.Text = "Welcome to the Budget Manager!";
            // 
            // summaryLabel
            // 
            this.summaryLabel.AutoSize = true;
            this.summaryLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.summaryLabel.Location = new System.Drawing.Point(12, 64);
            this.summaryLabel.Name = "summaryLabel";
            this.summaryLabel.Size = new System.Drawing.Size(10, 13);
            this.summaryLabel.TabIndex = 1;
            this.summaryLabel.Text = "-";
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.statusStrip1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusText,
            this.toolStripStatusLabel2,
            this.progressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 199);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(0, 5, 0, 3);
            this.statusStrip1.Size = new System.Drawing.Size(710, 27);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "darkStatusStrip1";
            // 
            // identifyButton
            // 
            this.identifyButton.Location = new System.Drawing.Point(12, 91);
            this.identifyButton.Name = "identifyButton";
            this.identifyButton.Padding = new System.Windows.Forms.Padding(5);
            this.identifyButton.Size = new System.Drawing.Size(100, 100);
            this.identifyButton.TabIndex = 3;
            this.identifyButton.Text = "Identify";
            this.identifyButton.Click += new System.EventHandler(this.identifyButton_Click);
            // 
            // cleanButton
            // 
            this.cleanButton.Location = new System.Drawing.Point(158, 91);
            this.cleanButton.Name = "cleanButton";
            this.cleanButton.Padding = new System.Windows.Forms.Padding(5);
            this.cleanButton.Size = new System.Drawing.Size(100, 100);
            this.cleanButton.TabIndex = 4;
            this.cleanButton.Text = "Clean";
            this.cleanButton.Click += new System.EventHandler(this.cleanButton_Click);
            // 
            // graphsButton
            // 
            this.graphsButton.Location = new System.Drawing.Point(304, 91);
            this.graphsButton.Name = "graphsButton";
            this.graphsButton.Padding = new System.Windows.Forms.Padding(5);
            this.graphsButton.Size = new System.Drawing.Size(100, 100);
            this.graphsButton.TabIndex = 5;
            this.graphsButton.Text = "Graphs";
            this.graphsButton.Click += new System.EventHandler(this.graphsButton_Click);
            // 
            // transactionsButton
            // 
            this.transactionsButton.Location = new System.Drawing.Point(450, 91);
            this.transactionsButton.Name = "transactionsButton";
            this.transactionsButton.Padding = new System.Windows.Forms.Padding(5);
            this.transactionsButton.Size = new System.Drawing.Size(100, 100);
            this.transactionsButton.TabIndex = 6;
            this.transactionsButton.Text = "Transactions";
            this.transactionsButton.Click += new System.EventHandler(this.transactionsButton_Click);
            // 
            // analysisButton
            // 
            this.analysisButton.Location = new System.Drawing.Point(596, 91);
            this.analysisButton.Name = "analysisButton";
            this.analysisButton.Padding = new System.Windows.Forms.Padding(5);
            this.analysisButton.Size = new System.Drawing.Size(100, 100);
            this.analysisButton.TabIndex = 7;
            this.analysisButton.Text = "Analysis";
            this.analysisButton.Click += new System.EventHandler(this.analysisButton_Click);
            // 
            // warningButton
            // 
            this.warningButton.Location = new System.Drawing.Point(646, 27);
            this.warningButton.Name = "warningButton";
            this.warningButton.Padding = new System.Windows.Forms.Padding(5);
            this.warningButton.Size = new System.Drawing.Size(50, 50);
            this.warningButton.TabIndex = 8;
            this.warningButton.Text = "!!!";
            this.warningButton.Click += new System.EventHandler(this.warningButton_Click);
            // 
            // statusText
            // 
            this.statusText.Name = "statusText";
            this.statusText.Size = new System.Drawing.Size(12, 11);
            this.statusText.Text = "-";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(565, 11);
            this.toolStripStatusLabel2.Spring = true;
            // 
            // progressBar
            // 
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 10);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.menuStrip1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(3, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(710, 24);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "darkMenuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadDirectoryToolStripMenuItem});
            this.fileToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadDirectoryToolStripMenuItem
            // 
            this.loadDirectoryToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.loadDirectoryToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.loadDirectoryToolStripMenuItem.Name = "loadDirectoryToolStripMenuItem";
            this.loadDirectoryToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.loadDirectoryToolStripMenuItem.Text = "Load Directory";
            this.loadDirectoryToolStripMenuItem.Click += new System.EventHandler(this.loadDirectoryToolStripMenuItem_Click);
            // 
            // MainForm_Dark
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(710, 226);
            this.Controls.Add(this.warningButton);
            this.Controls.Add(this.analysisButton);
            this.Controls.Add(this.transactionsButton);
            this.Controls.Add(this.graphsButton);
            this.Controls.Add(this.cleanButton);
            this.Controls.Add(this.identifyButton);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.summaryLabel);
            this.Controls.Add(this.darkLabel1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm_Dark";
            this.Text = "MainForm_Dark";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DarkUI.Controls.DarkLabel darkLabel1;
        private DarkUI.Controls.DarkLabel summaryLabel;
        private DarkUI.Controls.DarkStatusStrip statusStrip1;
        private DarkUI.Controls.DarkButton identifyButton;
        private DarkUI.Controls.DarkButton cleanButton;
        private DarkUI.Controls.DarkButton graphsButton;
        private DarkUI.Controls.DarkButton transactionsButton;
        private DarkUI.Controls.DarkButton analysisButton;
        private DarkUI.Controls.DarkButton warningButton;
        private System.Windows.Forms.ToolStripStatusLabel statusText;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private DarkUI.Controls.DarkMenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadDirectoryToolStripMenuItem;
    }
}