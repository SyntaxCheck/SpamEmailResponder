namespace MailServer
{
    partial class StorageViewer
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvEmails = new System.Windows.Forms.DataGridView();
            this.tbxDateReceived = new System.Windows.Forms.TextBox();
            this.tbxDeterminedName = new System.Windows.Forms.TextBox();
            this.tbxDeterminedType = new System.Windows.Forms.TextBox();
            this.cbxHasAttachments = new System.Windows.Forms.CheckBox();
            this.tbxMessageId = new System.Windows.Forms.TextBox();
            this.tbxFromAddress = new System.Windows.Forms.TextBox();
            this.tbxSubject = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tbxBodyPlainText = new System.Windows.Forms.TextBox();
            this.tbxDeterminedReply = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cbxShowAll = new System.Windows.Forms.CheckBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.FromAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Subject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateReceived = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateProcessed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PersonName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MessageType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RepliedBool = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ignored = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EmailBody = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeterminedReply = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NumberOfAttachments = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MsgID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEmails)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Orange;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(992, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(108, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvEmails);
            this.groupBox1.Location = new System.Drawing.Point(9, 33);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(971, 291);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Emails";
            // 
            // dgvEmails
            // 
            this.dgvEmails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEmails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FromAddress,
            this.Subject,
            this.DateReceived,
            this.DateProcessed,
            this.PersonName,
            this.MessageType,
            this.RepliedBool,
            this.Ignored,
            this.EmailBody,
            this.DeterminedReply,
            this.NumberOfAttachments,
            this.MsgID});
            this.dgvEmails.Location = new System.Drawing.Point(4, 17);
            this.dgvEmails.Margin = new System.Windows.Forms.Padding(2);
            this.dgvEmails.Name = "dgvEmails";
            this.dgvEmails.ReadOnly = true;
            this.dgvEmails.RowTemplate.Height = 24;
            this.dgvEmails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEmails.Size = new System.Drawing.Size(963, 270);
            this.dgvEmails.TabIndex = 0;
            this.dgvEmails.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEmails_RowEnter);
            // 
            // tbxDateReceived
            // 
            this.tbxDateReceived.Location = new System.Drawing.Point(137, 389);
            this.tbxDateReceived.Name = "tbxDateReceived";
            this.tbxDateReceived.Size = new System.Drawing.Size(191, 24);
            this.tbxDateReceived.TabIndex = 38;
            // 
            // tbxDeterminedName
            // 
            this.tbxDeterminedName.Location = new System.Drawing.Point(460, 419);
            this.tbxDeterminedName.Name = "tbxDeterminedName";
            this.tbxDeterminedName.Size = new System.Drawing.Size(191, 24);
            this.tbxDeterminedName.TabIndex = 37;
            // 
            // tbxDeterminedType
            // 
            this.tbxDeterminedType.Enabled = false;
            this.tbxDeterminedType.Location = new System.Drawing.Point(137, 419);
            this.tbxDeterminedType.Name = "tbxDeterminedType";
            this.tbxDeterminedType.Size = new System.Drawing.Size(191, 24);
            this.tbxDeterminedType.TabIndex = 36;
            // 
            // cbxHasAttachments
            // 
            this.cbxHasAttachments.AutoSize = true;
            this.cbxHasAttachments.Location = new System.Drawing.Point(771, 422);
            this.cbxHasAttachments.Name = "cbxHasAttachments";
            this.cbxHasAttachments.Size = new System.Drawing.Size(18, 17);
            this.cbxHasAttachments.TabIndex = 35;
            this.cbxHasAttachments.UseVisualStyleBackColor = true;
            // 
            // tbxMessageId
            // 
            this.tbxMessageId.Enabled = false;
            this.tbxMessageId.Location = new System.Drawing.Point(420, 389);
            this.tbxMessageId.Name = "tbxMessageId";
            this.tbxMessageId.Size = new System.Drawing.Size(560, 24);
            this.tbxMessageId.TabIndex = 34;
            // 
            // tbxFromAddress
            // 
            this.tbxFromAddress.Location = new System.Drawing.Point(137, 359);
            this.tbxFromAddress.Name = "tbxFromAddress";
            this.tbxFromAddress.Size = new System.Drawing.Size(843, 24);
            this.tbxFromAddress.TabIndex = 33;
            // 
            // tbxSubject
            // 
            this.tbxSubject.Location = new System.Drawing.Point(137, 329);
            this.tbxSubject.Name = "tbxSubject";
            this.tbxSubject.Size = new System.Drawing.Size(843, 24);
            this.tbxSubject.TabIndex = 32;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 419);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(119, 17);
            this.label7.TabIndex = 31;
            this.label7.Text = "Determined Type:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(331, 419);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(123, 17);
            this.label6.TabIndex = 30;
            this.label6.Text = "Determined Name:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(332, 389);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 17);
            this.label5.TabIndex = 29;
            this.label5.Text = "Message ID:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(654, 422);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(111, 17);
            this.label4.TabIndex = 28;
            this.label4.Text = "Has Attachment:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 389);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 17);
            this.label3.TabIndex = 27;
            this.label3.Text = "Date Received:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 359);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 17);
            this.label2.TabIndex = 26;
            this.label2.Text = "From Address:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 329);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 17);
            this.label1.TabIndex = 25;
            this.label1.Text = "Subject Line:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(86, 449);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 17);
            this.label8.TabIndex = 40;
            this.label8.Text = "Body:";
            // 
            // tbxBodyPlainText
            // 
            this.tbxBodyPlainText.Location = new System.Drawing.Point(137, 449);
            this.tbxBodyPlainText.Multiline = true;
            this.tbxBodyPlainText.Name = "tbxBodyPlainText";
            this.tbxBodyPlainText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxBodyPlainText.Size = new System.Drawing.Size(398, 183);
            this.tbxBodyPlainText.TabIndex = 41;
            // 
            // tbxDeterminedReply
            // 
            this.tbxDeterminedReply.Location = new System.Drawing.Point(594, 449);
            this.tbxDeterminedReply.Multiline = true;
            this.tbxDeterminedReply.Name = "tbxDeterminedReply";
            this.tbxDeterminedReply.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxDeterminedReply.Size = new System.Drawing.Size(386, 183);
            this.tbxDeterminedReply.TabIndex = 43;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(541, 446);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 17);
            this.label9.TabIndex = 42;
            this.label9.Text = "Reply:";
            // 
            // cbxShowAll
            // 
            this.cbxShowAll.AutoSize = true;
            this.cbxShowAll.BackColor = System.Drawing.Color.Orange;
            this.cbxShowAll.Location = new System.Drawing.Point(907, 4);
            this.cbxShowAll.Name = "cbxShowAll";
            this.cbxShowAll.Size = new System.Drawing.Size(80, 21);
            this.cbxShowAll.TabIndex = 1;
            this.cbxShowAll.Text = "Show All";
            this.cbxShowAll.UseVisualStyleBackColor = false;
            this.cbxShowAll.CheckedChanged += new System.EventHandler(this.cbxShowAll_CheckedChanged);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = global::MailServer.Properties.Resources.if_arrow_refresh_35687;
            this.btnRefresh.Location = new System.Drawing.Point(9, 593);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(47, 39);
            this.btnRefresh.TabIndex = 44;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // FromAddress
            // 
            this.FromAddress.FillWeight = 150F;
            this.FromAddress.HeaderText = "From";
            this.FromAddress.Name = "FromAddress";
            this.FromAddress.ReadOnly = true;
            this.FromAddress.Width = 150;
            // 
            // Subject
            // 
            this.Subject.FillWeight = 170F;
            this.Subject.HeaderText = "Subject";
            this.Subject.Name = "Subject";
            this.Subject.ReadOnly = true;
            this.Subject.Width = 170;
            // 
            // DateReceived
            // 
            this.DateReceived.HeaderText = "Date Received";
            this.DateReceived.Name = "DateReceived";
            this.DateReceived.ReadOnly = true;
            // 
            // DateProcessed
            // 
            this.DateProcessed.HeaderText = "Date Processed";
            this.DateProcessed.Name = "DateProcessed";
            this.DateProcessed.ReadOnly = true;
            // 
            // PersonName
            // 
            this.PersonName.HeaderText = "Person Name";
            this.PersonName.Name = "PersonName";
            this.PersonName.ReadOnly = true;
            // 
            // MessageType
            // 
            this.MessageType.HeaderText = "Type";
            this.MessageType.Name = "MessageType";
            this.MessageType.ReadOnly = true;
            // 
            // RepliedBool
            // 
            this.RepliedBool.FillWeight = 70F;
            this.RepliedBool.HeaderText = "Have Replied";
            this.RepliedBool.Name = "RepliedBool";
            this.RepliedBool.ReadOnly = true;
            this.RepliedBool.Width = 70;
            // 
            // Ignored
            // 
            this.Ignored.HeaderText = "Ignored";
            this.Ignored.Name = "Ignored";
            this.Ignored.ReadOnly = true;
            // 
            // EmailBody
            // 
            this.EmailBody.FillWeight = 300F;
            this.EmailBody.HeaderText = "Body";
            this.EmailBody.Name = "EmailBody";
            this.EmailBody.ReadOnly = true;
            this.EmailBody.Width = 300;
            // 
            // DeterminedReply
            // 
            this.DeterminedReply.FillWeight = 300F;
            this.DeterminedReply.HeaderText = "Determined Reply";
            this.DeterminedReply.Name = "DeterminedReply";
            this.DeterminedReply.ReadOnly = true;
            this.DeterminedReply.Width = 300;
            // 
            // NumberOfAttachments
            // 
            this.NumberOfAttachments.FillWeight = 90F;
            this.NumberOfAttachments.HeaderText = "Attachments";
            this.NumberOfAttachments.Name = "NumberOfAttachments";
            this.NumberOfAttachments.ReadOnly = true;
            this.NumberOfAttachments.Width = 90;
            // 
            // MsgID
            // 
            this.MsgID.HeaderText = "MSG ID";
            this.MsgID.Name = "MsgID";
            this.MsgID.ReadOnly = true;
            // 
            // StorageViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(992, 644);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.cbxShowAll);
            this.Controls.Add(this.tbxDeterminedReply);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tbxBodyPlainText);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tbxDateReceived);
            this.Controls.Add(this.tbxDeterminedName);
            this.Controls.Add(this.tbxDeterminedType);
            this.Controls.Add(this.cbxHasAttachments);
            this.Controls.Add(this.tbxMessageId);
            this.Controls.Add(this.tbxFromAddress);
            this.Controls.Add(this.tbxSubject);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "StorageViewer";
            this.Text = "Storage Viewer";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEmails)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvEmails;
        private System.Windows.Forms.TextBox tbxDateReceived;
        private System.Windows.Forms.TextBox tbxDeterminedName;
        private System.Windows.Forms.TextBox tbxDeterminedType;
        private System.Windows.Forms.CheckBox cbxHasAttachments;
        private System.Windows.Forms.TextBox tbxMessageId;
        private System.Windows.Forms.TextBox tbxFromAddress;
        private System.Windows.Forms.TextBox tbxSubject;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbxBodyPlainText;
        private System.Windows.Forms.TextBox tbxDeterminedReply;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox cbxShowAll;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridViewTextBoxColumn FromAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn Subject;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateReceived;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateProcessed;
        private System.Windows.Forms.DataGridViewTextBoxColumn PersonName;
        private System.Windows.Forms.DataGridViewTextBoxColumn MessageType;
        private System.Windows.Forms.DataGridViewTextBoxColumn RepliedBool;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ignored;
        private System.Windows.Forms.DataGridViewTextBoxColumn EmailBody;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeterminedReply;
        private System.Windows.Forms.DataGridViewTextBoxColumn NumberOfAttachments;
        private System.Windows.Forms.DataGridViewTextBoxColumn MsgID;
    }
}