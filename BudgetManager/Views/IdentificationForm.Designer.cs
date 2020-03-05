namespace BudgetManager.Views
{
    partial class IdentificationForm
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
            this.transactionTypeList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.unidentifiedTransactionsList = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.vendorText = new System.Windows.Forms.TextBox();
            this.categoryText = new System.Windows.Forms.TextBox();
            this.identifierText = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.matchesText = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.caseSensitiveCheckbox = new System.Windows.Forms.CheckBox();
            this.startsWithCheckbox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // transactionTypeList
            // 
            this.transactionTypeList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.transactionTypeList.FormattingEnabled = true;
            this.transactionTypeList.Location = new System.Drawing.Point(3, 18);
            this.transactionTypeList.Name = "transactionTypeList";
            this.transactionTypeList.Size = new System.Drawing.Size(245, 173);
            this.transactionTypeList.TabIndex = 0;
            this.transactionTypeList.SelectedIndexChanged += new System.EventHandler(this.transactionTypeList_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Transaction Types";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 213);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Unidentified Transactions";
            // 
            // unidentifiedTransactionsList
            // 
            this.unidentifiedTransactionsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.unidentifiedTransactionsList.FormattingEnabled = true;
            this.unidentifiedTransactionsList.Location = new System.Drawing.Point(3, 230);
            this.unidentifiedTransactionsList.Name = "unidentifiedTransactionsList";
            this.unidentifiedTransactionsList.Size = new System.Drawing.Size(245, 472);
            this.unidentifiedTransactionsList.TabIndex = 3;
            this.unidentifiedTransactionsList.SelectedIndexChanged += new System.EventHandler(this.unidentifiedTransactionsList_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Vendor";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(0, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Category";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(0, 154);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Identifier";
            // 
            // vendorText
            // 
            this.vendorText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vendorText.Location = new System.Drawing.Point(3, 57);
            this.vendorText.Name = "vendorText";
            this.vendorText.Size = new System.Drawing.Size(720, 20);
            this.vendorText.TabIndex = 7;
            this.vendorText.TextChanged += new System.EventHandler(this.vendorText_TextChanged);
            // 
            // categoryText
            // 
            this.categoryText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.categoryText.Location = new System.Drawing.Point(3, 115);
            this.categoryText.Name = "categoryText";
            this.categoryText.Size = new System.Drawing.Size(720, 20);
            this.categoryText.TabIndex = 8;
            this.categoryText.TextChanged += new System.EventHandler(this.categoryText_TextChanged);
            // 
            // identifierText
            // 
            this.identifierText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.identifierText.Location = new System.Drawing.Point(3, 171);
            this.identifierText.Name = "identifierText";
            this.identifierText.Size = new System.Drawing.Size(720, 20);
            this.identifierText.TabIndex = 9;
            this.identifierText.TextChanged += new System.EventHandler(this.identifierText_TextChanged);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(3, 3);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 10;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(84, 3);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(75, 23);
            this.deleteButton.TabIndex = 11;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // matchesText
            // 
            this.matchesText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.matchesText.Location = new System.Drawing.Point(3, 230);
            this.matchesText.Multiline = true;
            this.matchesText.Name = "matchesText";
            this.matchesText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.matchesText.Size = new System.Drawing.Size(720, 472);
            this.matchesText.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(0, 213);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(115, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Matching Transactions";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BackColor = System.Drawing.SystemColors.InfoText;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.transactionTypeList);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.unidentifiedTransactionsList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel2.Controls.Add(this.startsWithCheckbox);
            this.splitContainer1.Panel2.Controls.Add(this.caseSensitiveCheckbox);
            this.splitContainer1.Panel2.Controls.Add(this.saveButton);
            this.splitContainer1.Panel2.Controls.Add(this.label6);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.matchesText);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this.deleteButton);
            this.splitContainer1.Panel2.Controls.Add(this.label5);
            this.splitContainer1.Panel2.Controls.Add(this.vendorText);
            this.splitContainer1.Panel2.Controls.Add(this.identifierText);
            this.splitContainer1.Panel2.Controls.Add(this.categoryText);
            this.splitContainer1.Size = new System.Drawing.Size(995, 709);
            this.splitContainer1.SplitterDistance = 257;
            this.splitContainer1.TabIndex = 14;
            // 
            // caseSensitiveCheckbox
            // 
            this.caseSensitiveCheckbox.AutoSize = true;
            this.caseSensitiveCheckbox.Location = new System.Drawing.Point(102, 148);
            this.caseSensitiveCheckbox.Name = "caseSensitiveCheckbox";
            this.caseSensitiveCheckbox.Size = new System.Drawing.Size(96, 17);
            this.caseSensitiveCheckbox.TabIndex = 14;
            this.caseSensitiveCheckbox.Text = "Case Sensitive";
            this.caseSensitiveCheckbox.UseVisualStyleBackColor = true;
            this.caseSensitiveCheckbox.CheckedChanged += new System.EventHandler(this.caseSensitiveCheckbox_CheckedChanged);
            // 
            // startsWithCheckbox
            // 
            this.startsWithCheckbox.AutoSize = true;
            this.startsWithCheckbox.Location = new System.Drawing.Point(229, 148);
            this.startsWithCheckbox.Name = "startsWithCheckbox";
            this.startsWithCheckbox.Size = new System.Drawing.Size(78, 17);
            this.startsWithCheckbox.TabIndex = 15;
            this.startsWithCheckbox.Text = "Starts With";
            this.startsWithCheckbox.UseVisualStyleBackColor = true;
            this.startsWithCheckbox.CheckedChanged += new System.EventHandler(this.startsWithCheckbox_CheckedChanged);
            // 
            // IdentificationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1019, 733);
            this.Controls.Add(this.splitContainer1);
            this.Name = "IdentificationForm";
            this.Text = "IdentificationForm";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox transactionTypeList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox unidentifiedTransactionsList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox vendorText;
        private System.Windows.Forms.TextBox categoryText;
        private System.Windows.Forms.TextBox identifierText;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.TextBox matchesText;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckBox caseSensitiveCheckbox;
        private System.Windows.Forms.CheckBox startsWithCheckbox;
    }
}