namespace MailServer
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.processTimer = new System.Windows.Forms.Timer(this.components);
            this.tbxOutput = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnRegenerate = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnIgnore = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnSendEmail = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.dgvPastEmail = new System.Windows.Forms.DataGridView();
            this.Subject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FromAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Body = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Response = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tbxDeterminedReply = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tbxBodyPlainText = new System.Windows.Forms.TextBox();
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
            this.tbxAttachmentNames = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPastEmail)).BeginInit();
            this.SuspendLayout();
            // 
            // processTimer
            // 
            this.processTimer.Tick += new System.EventHandler(this.processTimer_Tick);
            // 
            // tbxOutput
            // 
            this.tbxOutput.Location = new System.Drawing.Point(12, 812);
            this.tbxOutput.Multiline = true;
            this.tbxOutput.Name = "tbxOutput";
            this.tbxOutput.Size = new System.Drawing.Size(1700, 130);
            this.tbxOutput.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbxAttachmentNames);
            this.groupBox1.Controls.Add(this.btnRegenerate);
            this.groupBox1.Controls.Add(this.btnRefresh);
            this.groupBox1.Controls.Add(this.btnIgnore);
            this.groupBox1.Controls.Add(this.btnNext);
            this.groupBox1.Controls.Add(this.btnSendEmail);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.dgvPastEmail);
            this.groupBox1.Controls.Add(this.tbxDeterminedReply);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.tbxBodyPlainText);
            this.groupBox1.Controls.Add(this.tbxDateReceived);
            this.groupBox1.Controls.Add(this.tbxDeterminedName);
            this.groupBox1.Controls.Add(this.tbxDeterminedType);
            this.groupBox1.Controls.Add(this.cbxHasAttachments);
            this.groupBox1.Controls.Add(this.tbxMessageId);
            this.groupBox1.Controls.Add(this.tbxFromAddress);
            this.groupBox1.Controls.Add(this.tbxSubject);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1700, 766);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Current Message";
            // 
            // btnRegenerate
            // 
            this.btnRegenerate.Location = new System.Drawing.Point(258, 728);
            this.btnRegenerate.Name = "btnRegenerate";
            this.btnRegenerate.Size = new System.Drawing.Size(75, 23);
            this.btnRegenerate.TabIndex = 23;
            this.btnRegenerate.Text = "ReGen";
            this.btnRegenerate.UseVisualStyleBackColor = true;
            this.btnRegenerate.Click += new System.EventHandler(this.btnRegenerate_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(9, 728);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 22;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnIgnore
            // 
            this.btnIgnore.Enabled = false;
            this.btnIgnore.Location = new System.Drawing.Point(639, 728);
            this.btnIgnore.Name = "btnIgnore";
            this.btnIgnore.Size = new System.Drawing.Size(75, 23);
            this.btnIgnore.TabIndex = 21;
            this.btnIgnore.Text = "Ignore";
            this.btnIgnore.UseVisualStyleBackColor = true;
            this.btnIgnore.Click += new System.EventHandler(this.btnIgnore_Click);
            // 
            // btnNext
            // 
            this.btnNext.Enabled = false;
            this.btnNext.Location = new System.Drawing.Point(558, 728);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 20;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnSendEmail
            // 
            this.btnSendEmail.Enabled = false;
            this.btnSendEmail.Location = new System.Drawing.Point(177, 728);
            this.btnSendEmail.Name = "btnSendEmail";
            this.btnSendEmail.Size = new System.Drawing.Size(75, 23);
            this.btnSendEmail.TabIndex = 19;
            this.btnSendEmail.Text = "Send Email";
            this.btnSendEmail.UseVisualStyleBackColor = true;
            this.btnSendEmail.Click += new System.EventHandler(this.btnSendEmail_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(721, 38);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(107, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "Past emails in thread:";
            // 
            // dgvPastEmail
            // 
            this.dgvPastEmail.AllowUserToAddRows = false;
            this.dgvPastEmail.AllowUserToDeleteRows = false;
            this.dgvPastEmail.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.dgvPastEmail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPastEmail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Subject,
            this.FromAddress,
            this.Date,
            this.Body,
            this.Response});
            this.dgvPastEmail.Location = new System.Drawing.Point(724, 61);
            this.dgvPastEmail.Name = "dgvPastEmail";
            this.dgvPastEmail.ReadOnly = true;
            this.dgvPastEmail.Size = new System.Drawing.Size(970, 656);
            this.dgvPastEmail.TabIndex = 17;
            this.dgvPastEmail.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPastEmail_CellContentDoubleClick);
            // 
            // Subject
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            this.Subject.DefaultCellStyle = dataGridViewCellStyle1;
            this.Subject.FillWeight = 125F;
            this.Subject.HeaderText = "Subject";
            this.Subject.Name = "Subject";
            this.Subject.ReadOnly = true;
            this.Subject.Width = 125;
            // 
            // FromAddress
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            this.FromAddress.DefaultCellStyle = dataGridViewCellStyle2;
            this.FromAddress.FillWeight = 150F;
            this.FromAddress.HeaderText = "From Address";
            this.FromAddress.Name = "FromAddress";
            this.FromAddress.ReadOnly = true;
            this.FromAddress.Width = 150;
            // 
            // Date
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.Date.DefaultCellStyle = dataGridViewCellStyle3;
            this.Date.FillWeight = 125F;
            this.Date.HeaderText = "Date";
            this.Date.Name = "Date";
            this.Date.ReadOnly = true;
            this.Date.Width = 125;
            // 
            // Body
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            this.Body.DefaultCellStyle = dataGridViewCellStyle4;
            this.Body.FillWeight = 310F;
            this.Body.HeaderText = "Body";
            this.Body.Name = "Body";
            this.Body.ReadOnly = true;
            this.Body.Width = 310;
            // 
            // Response
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            this.Response.DefaultCellStyle = dataGridViewCellStyle5;
            this.Response.FillWeight = 350F;
            this.Response.HeaderText = "Response";
            this.Response.Name = "Response";
            this.Response.ReadOnly = true;
            this.Response.Width = 350;
            // 
            // tbxDeterminedReply
            // 
            this.tbxDeterminedReply.Location = new System.Drawing.Point(9, 483);
            this.tbxDeterminedReply.Multiline = true;
            this.tbxDeterminedReply.Name = "tbxDeterminedReply";
            this.tbxDeterminedReply.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxDeterminedReply.Size = new System.Drawing.Size(703, 234);
            this.tbxDeterminedReply.TabIndex = 16;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 467);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(115, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "Determined Response:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 214);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(79, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Body plain text:";
            // 
            // tbxBodyPlainText
            // 
            this.tbxBodyPlainText.Location = new System.Drawing.Point(10, 230);
            this.tbxBodyPlainText.Multiline = true;
            this.tbxBodyPlainText.Name = "tbxBodyPlainText";
            this.tbxBodyPlainText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxBodyPlainText.Size = new System.Drawing.Size(703, 234);
            this.tbxBodyPlainText.TabIndex = 2;
            // 
            // tbxDateReceived
            // 
            this.tbxDateReceived.Location = new System.Drawing.Point(108, 87);
            this.tbxDateReceived.Name = "tbxDateReceived";
            this.tbxDateReceived.Size = new System.Drawing.Size(191, 20);
            this.tbxDateReceived.TabIndex = 13;
            // 
            // tbxDeterminedName
            // 
            this.tbxDeterminedName.Location = new System.Drawing.Point(108, 185);
            this.tbxDeterminedName.Name = "tbxDeterminedName";
            this.tbxDeterminedName.Size = new System.Drawing.Size(191, 20);
            this.tbxDeterminedName.TabIndex = 12;
            // 
            // tbxDeterminedType
            // 
            this.tbxDeterminedType.Enabled = false;
            this.tbxDeterminedType.Location = new System.Drawing.Point(108, 159);
            this.tbxDeterminedType.Name = "tbxDeterminedType";
            this.tbxDeterminedType.Size = new System.Drawing.Size(191, 20);
            this.tbxDeterminedType.TabIndex = 11;
            // 
            // cbxHasAttachments
            // 
            this.cbxHasAttachments.AutoSize = true;
            this.cbxHasAttachments.Location = new System.Drawing.Point(108, 113);
            this.cbxHasAttachments.Name = "cbxHasAttachments";
            this.cbxHasAttachments.Size = new System.Drawing.Size(15, 14);
            this.cbxHasAttachments.TabIndex = 10;
            this.cbxHasAttachments.UseVisualStyleBackColor = true;
            // 
            // tbxMessageId
            // 
            this.tbxMessageId.Location = new System.Drawing.Point(108, 133);
            this.tbxMessageId.Name = "tbxMessageId";
            this.tbxMessageId.Size = new System.Drawing.Size(604, 20);
            this.tbxMessageId.TabIndex = 9;
            // 
            // tbxFromAddress
            // 
            this.tbxFromAddress.Location = new System.Drawing.Point(108, 61);
            this.tbxFromAddress.Name = "tbxFromAddress";
            this.tbxFromAddress.Size = new System.Drawing.Size(604, 20);
            this.tbxFromAddress.TabIndex = 8;
            // 
            // tbxSubject
            // 
            this.tbxSubject.Location = new System.Drawing.Point(108, 35);
            this.tbxSubject.Name = "tbxSubject";
            this.tbxSubject.Size = new System.Drawing.Size(604, 20);
            this.tbxSubject.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 162);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(91, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Determined Type:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 188);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Determined Name:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(35, 136);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Message ID:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Has Attachment:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Date Received:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "From Address:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Subject Line:";
            // 
            // tbxAttachmentNames
            // 
            this.tbxAttachmentNames.Location = new System.Drawing.Point(129, 110);
            this.tbxAttachmentNames.Name = "tbxAttachmentNames";
            this.tbxAttachmentNames.Size = new System.Drawing.Size(583, 20);
            this.tbxAttachmentNames.TabIndex = 24;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1724, 863);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tbxOutput);
            this.Name = "Form1";
            this.RightToLeftLayout = true;
            this.Text = "Mail Server";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPastEmail)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer processTimer;
        private System.Windows.Forms.TextBox tbxOutput;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbxFromAddress;
        private System.Windows.Forms.TextBox tbxSubject;
        private System.Windows.Forms.TextBox tbxDateReceived;
        private System.Windows.Forms.TextBox tbxDeterminedName;
        private System.Windows.Forms.TextBox tbxDeterminedType;
        private System.Windows.Forms.CheckBox cbxHasAttachments;
        private System.Windows.Forms.TextBox tbxMessageId;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbxBodyPlainText;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbxDeterminedReply;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DataGridView dgvPastEmail;
        private System.Windows.Forms.Button btnSendEmail;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnIgnore;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnRegenerate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Subject;
        private System.Windows.Forms.DataGridViewTextBoxColumn FromAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn Body;
        private System.Windows.Forms.DataGridViewTextBoxColumn Response;
        private System.Windows.Forms.TextBox tbxAttachmentNames;
    }
}

