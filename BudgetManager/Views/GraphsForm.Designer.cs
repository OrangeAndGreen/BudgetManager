namespace BudgetManager.Views
{
    partial class GraphsForm
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
            this.graphDrawButton = new System.Windows.Forms.Button();
            this.mainPlot = new LiveCharts.WinForms.CartesianChart();
            this.graphSelector = new System.Windows.Forms.ComboBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // graphDrawButton
            // 
            this.graphDrawButton.Location = new System.Drawing.Point(223, 12);
            this.graphDrawButton.Name = "graphDrawButton";
            this.graphDrawButton.Size = new System.Drawing.Size(75, 23);
            this.graphDrawButton.TabIndex = 9;
            this.graphDrawButton.Text = "Draw";
            this.graphDrawButton.UseVisualStyleBackColor = true;
            this.graphDrawButton.Click += new System.EventHandler(this.graphDrawButton_Click);
            // 
            // mainPlot
            // 
            this.mainPlot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainPlot.Location = new System.Drawing.Point(12, 39);
            this.mainPlot.Name = "mainPlot";
            this.mainPlot.Size = new System.Drawing.Size(785, 567);
            this.mainPlot.TabIndex = 7;
            this.mainPlot.Text = "cartesianChart1";
            // 
            // graphSelector
            // 
            this.graphSelector.FormattingEnabled = true;
            this.graphSelector.Location = new System.Drawing.Point(12, 12);
            this.graphSelector.Name = "graphSelector";
            this.graphSelector.Size = new System.Drawing.Size(204, 21);
            this.graphSelector.TabIndex = 8;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusText});
            this.statusStrip1.Location = new System.Drawing.Point(0, 609);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(809, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusText
            // 
            this.statusText.Name = "statusText";
            this.statusText.Size = new System.Drawing.Size(12, 17);
            this.statusText.Text = "-";
            // 
            // GraphsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 631);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.graphDrawButton);
            this.Controls.Add(this.mainPlot);
            this.Controls.Add(this.graphSelector);
            this.Name = "GraphsForm";
            this.Text = "GraphsForm";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button graphDrawButton;
        private LiveCharts.WinForms.CartesianChart mainPlot;
        private System.Windows.Forms.ComboBox graphSelector;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusText;
    }
}