namespace BudgetManager.Views
{
    partial class AnalysisForm
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
            this.analysisTextbox = new System.Windows.Forms.TextBox();
            this.vendorAnalysisButton = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // analysisTextbox
            // 
            this.analysisTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.analysisTextbox.Location = new System.Drawing.Point(12, 42);
            this.analysisTextbox.Multiline = true;
            this.analysisTextbox.Name = "analysisTextbox";
            this.analysisTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.analysisTextbox.Size = new System.Drawing.Size(776, 383);
            this.analysisTextbox.TabIndex = 3;
            // 
            // vendorAnalysisButton
            // 
            this.vendorAnalysisButton.Location = new System.Drawing.Point(12, 12);
            this.vendorAnalysisButton.Name = "vendorAnalysisButton";
            this.vendorAnalysisButton.Size = new System.Drawing.Size(75, 23);
            this.vendorAnalysisButton.TabIndex = 2;
            this.vendorAnalysisButton.Text = "Vendors";
            this.vendorAnalysisButton.UseVisualStyleBackColor = true;
            this.vendorAnalysisButton.Click += new System.EventHandler(this.vendorAnalysisButton_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusText});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusText
            // 
            this.statusText.Name = "statusText";
            this.statusText.Size = new System.Drawing.Size(12, 17);
            this.statusText.Text = "-";
            // 
            // AnalysisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.analysisTextbox);
            this.Controls.Add(this.vendorAnalysisButton);
            this.Name = "AnalysisForm";
            this.Text = "AnalysisForm";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox analysisTextbox;
        private System.Windows.Forms.Button vendorAnalysisButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusText;
    }
}