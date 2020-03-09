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
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.vendorText = new System.Windows.Forms.TextBox();
            this.categoryText = new System.Windows.Forms.TextBox();
            this.identifierText = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.startsWithCheckbox = new System.Windows.Forms.CheckBox();
            this.caseSensitiveCheckbox = new System.Windows.Forms.CheckBox();
            this.transactionsGrid = new System.Windows.Forms.DataGridView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.existingGrid = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.unidentifiedGrid = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.transactionsGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.existingGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unidentifiedGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Vendor";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Category";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(0, 162);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Identifier";
            // 
            // vendorText
            // 
            this.vendorText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vendorText.Location = new System.Drawing.Point(3, 66);
            this.vendorText.Name = "vendorText";
            this.vendorText.Size = new System.Drawing.Size(900, 20);
            this.vendorText.TabIndex = 7;
            this.vendorText.TextChanged += new System.EventHandler(this.vendorText_TextChanged);
            // 
            // categoryText
            // 
            this.categoryText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.categoryText.Location = new System.Drawing.Point(3, 120);
            this.categoryText.Name = "categoryText";
            this.categoryText.Size = new System.Drawing.Size(900, 20);
            this.categoryText.TabIndex = 8;
            this.categoryText.TextChanged += new System.EventHandler(this.categoryText_TextChanged);
            // 
            // identifierText
            // 
            this.identifierText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.identifierText.Location = new System.Drawing.Point(3, 178);
            this.identifierText.Name = "identifierText";
            this.identifierText.Size = new System.Drawing.Size(900, 20);
            this.identifierText.TabIndex = 9;
            this.identifierText.TextChanged += new System.EventHandler(this.identifierText_TextChanged);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(3, 12);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 10;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(84, 12);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(75, 23);
            this.deleteButton.TabIndex = 11;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 221);
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
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel2.Controls.Add(this.transactionsGrid);
            this.splitContainer1.Panel2.Controls.Add(this.startsWithCheckbox);
            this.splitContainer1.Panel2.Controls.Add(this.caseSensitiveCheckbox);
            this.splitContainer1.Panel2.Controls.Add(this.saveButton);
            this.splitContainer1.Panel2.Controls.Add(this.label6);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this.deleteButton);
            this.splitContainer1.Panel2.Controls.Add(this.label5);
            this.splitContainer1.Panel2.Controls.Add(this.vendorText);
            this.splitContainer1.Panel2.Controls.Add(this.identifierText);
            this.splitContainer1.Panel2.Controls.Add(this.categoryText);
            this.splitContainer1.Size = new System.Drawing.Size(1467, 730);
            this.splitContainer1.SplitterDistance = 549;
            this.splitContainer1.TabIndex = 14;
            // 
            // startsWithCheckbox
            // 
            this.startsWithCheckbox.AutoSize = true;
            this.startsWithCheckbox.Location = new System.Drawing.Point(229, 155);
            this.startsWithCheckbox.Name = "startsWithCheckbox";
            this.startsWithCheckbox.Size = new System.Drawing.Size(78, 17);
            this.startsWithCheckbox.TabIndex = 15;
            this.startsWithCheckbox.Text = "Starts With";
            this.startsWithCheckbox.UseVisualStyleBackColor = true;
            this.startsWithCheckbox.CheckedChanged += new System.EventHandler(this.startsWithCheckbox_CheckedChanged);
            // 
            // caseSensitiveCheckbox
            // 
            this.caseSensitiveCheckbox.AutoSize = true;
            this.caseSensitiveCheckbox.Location = new System.Drawing.Point(102, 155);
            this.caseSensitiveCheckbox.Name = "caseSensitiveCheckbox";
            this.caseSensitiveCheckbox.Size = new System.Drawing.Size(96, 17);
            this.caseSensitiveCheckbox.TabIndex = 14;
            this.caseSensitiveCheckbox.Text = "Case Sensitive";
            this.caseSensitiveCheckbox.UseVisualStyleBackColor = true;
            this.caseSensitiveCheckbox.CheckedChanged += new System.EventHandler(this.caseSensitiveCheckbox_CheckedChanged);
            // 
            // transactionsGrid
            // 
            this.transactionsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.transactionsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.transactionsGrid.Location = new System.Drawing.Point(3, 237);
            this.transactionsGrid.Name = "transactionsGrid";
            this.transactionsGrid.RowHeadersVisible = false;
            this.transactionsGrid.Size = new System.Drawing.Size(900, 486);
            this.transactionsGrid.TabIndex = 16;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.label1);
            this.splitContainer2.Panel1.Controls.Add(this.existingGrid);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.unidentifiedGrid);
            this.splitContainer2.Panel2.Controls.Add(this.label2);
            this.splitContainer2.Size = new System.Drawing.Size(549, 730);
            this.splitContainer2.SplitterDistance = 325;
            this.splitContainer2.TabIndex = 0;
            // 
            // existingGrid
            // 
            this.existingGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.existingGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.existingGrid.Location = new System.Drawing.Point(12, 28);
            this.existingGrid.MultiSelect = false;
            this.existingGrid.Name = "existingGrid";
            this.existingGrid.RowHeadersVisible = false;
            this.existingGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.existingGrid.Size = new System.Drawing.Size(521, 286);
            this.existingGrid.TabIndex = 0;
            this.existingGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.existingGrid_CellContentClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Transaction Types";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Unidentified Descriptions";
            // 
            // unidentifiedGrid
            // 
            this.unidentifiedGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.unidentifiedGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.unidentifiedGrid.Location = new System.Drawing.Point(13, 21);
            this.unidentifiedGrid.MultiSelect = false;
            this.unidentifiedGrid.Name = "unidentifiedGrid";
            this.unidentifiedGrid.RowHeadersVisible = false;
            this.unidentifiedGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.unidentifiedGrid.Size = new System.Drawing.Size(520, 371);
            this.unidentifiedGrid.TabIndex = 1;
            this.unidentifiedGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.unidentifiedGrid_CellContentClick);
            // 
            // IdentificationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1464, 733);
            this.Controls.Add(this.splitContainer1);
            this.Name = "IdentificationForm";
            this.Text = "IdentificationForm";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.transactionsGrid)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.existingGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unidentifiedGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox vendorText;
        private System.Windows.Forms.TextBox categoryText;
        private System.Windows.Forms.TextBox identifierText;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckBox caseSensitiveCheckbox;
        private System.Windows.Forms.CheckBox startsWithCheckbox;
        private System.Windows.Forms.DataGridView transactionsGrid;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView existingGrid;
        private System.Windows.Forms.DataGridView unidentifiedGrid;
        private System.Windows.Forms.Label label2;
    }
}