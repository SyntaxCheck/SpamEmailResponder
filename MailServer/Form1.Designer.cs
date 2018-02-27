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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
            this.processTimer = new System.Windows.Forms.Timer(this.components);
            this.tbxOutput = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbxAutoSend = new System.Windows.Forms.CheckBox();
            this.btnSaveChanges = new System.Windows.Forms.Button();
            this.tbxAttachmentNames = new System.Windows.Forms.TextBox();
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
            this.lblMessageInfo = new System.Windows.Forms.Label();
            this.cbxDebug = new System.Windows.Forms.CheckBox();
            this.lblCountdown = new System.Windows.Forms.Label();
            this.trckBar = new System.Windows.Forms.TrackBar();
            this.label11 = new System.Windows.Forms.Label();
            this.lblTrackBarValue = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPastEmail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trckBar)).BeginInit();
            this.SuspendLayout();
            // 
            // processTimer
            // 
            this.processTimer.Tick += new System.EventHandler(this.processTimer_Tick);
            // 
            // tbxOutput
            // 
            this.tbxOutput.Location = new System.Drawing.Point(16, 999);
            this.tbxOutput.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbxOutput.Multiline = true;
            this.tbxOutput.Name = "tbxOutput";
            this.tbxOutput.Size = new System.Drawing.Size(2265, 94);
            this.tbxOutput.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblTrackBarValue);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.trckBar);
            this.groupBox1.Controls.Add(this.lblCountdown);
            this.groupBox1.Controls.Add(this.cbxDebug);
            this.groupBox1.Controls.Add(this.lblMessageInfo);
            this.groupBox1.Controls.Add(this.cbxAutoSend);
            this.groupBox1.Controls.Add(this.btnSaveChanges);
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
            this.groupBox1.Location = new System.Drawing.Point(16, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(2267, 954);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Current Message";
            // 
            // cbxAutoSend
            // 
            this.cbxAutoSend.AutoSize = true;
            this.cbxAutoSend.Location = new System.Drawing.Point(965, 903);
            this.cbxAutoSend.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbxAutoSend.Name = "cbxAutoSend";
            this.cbxAutoSend.Size = new System.Drawing.Size(96, 21);
            this.cbxAutoSend.TabIndex = 26;
            this.cbxAutoSend.Text = "Auto Send";
            this.cbxAutoSend.UseVisualStyleBackColor = true;
            this.cbxAutoSend.CheckedChanged += new System.EventHandler(this.cbxAutoSend_CheckedChanged);
            // 
            // btnSaveChanges
            // 
            this.btnSaveChanges.Location = new System.Drawing.Point(819, 225);
            this.btnSaveChanges.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSaveChanges.Name = "btnSaveChanges";
            this.btnSaveChanges.Size = new System.Drawing.Size(129, 28);
            this.btnSaveChanges.TabIndex = 25;
            this.btnSaveChanges.Text = "Save Changes";
            this.btnSaveChanges.UseVisualStyleBackColor = true;
            this.btnSaveChanges.Click += new System.EventHandler(this.btnSaveChanges_Click);
            // 
            // tbxAttachmentNames
            // 
            this.tbxAttachmentNames.Location = new System.Drawing.Point(172, 135);
            this.tbxAttachmentNames.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbxAttachmentNames.Name = "tbxAttachmentNames";
            this.tbxAttachmentNames.Size = new System.Drawing.Size(776, 22);
            this.tbxAttachmentNames.TabIndex = 24;
            // 
            // btnRegenerate
            // 
            this.btnRegenerate.Location = new System.Drawing.Point(120, 896);
            this.btnRegenerate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRegenerate.Name = "btnRegenerate";
            this.btnRegenerate.Size = new System.Drawing.Size(100, 28);
            this.btnRegenerate.TabIndex = 23;
            this.btnRegenerate.Text = "ReGen";
            this.btnRegenerate.UseVisualStyleBackColor = true;
            this.btnRegenerate.Click += new System.EventHandler(this.btnRegenerate_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(12, 896);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 28);
            this.btnRefresh.TabIndex = 22;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnIgnore
            // 
            this.btnIgnore.Enabled = false;
            this.btnIgnore.Location = new System.Drawing.Point(380, 896);
            this.btnIgnore.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnIgnore.Name = "btnIgnore";
            this.btnIgnore.Size = new System.Drawing.Size(100, 28);
            this.btnIgnore.TabIndex = 21;
            this.btnIgnore.Text = "Ignore";
            this.btnIgnore.UseVisualStyleBackColor = true;
            this.btnIgnore.Click += new System.EventHandler(this.btnIgnore_Click);
            // 
            // btnNext
            // 
            this.btnNext.Enabled = false;
            this.btnNext.Location = new System.Drawing.Point(272, 896);
            this.btnNext.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(100, 28);
            this.btnNext.TabIndex = 20;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnSendEmail
            // 
            this.btnSendEmail.Enabled = false;
            this.btnSendEmail.Location = new System.Drawing.Point(644, 898);
            this.btnSendEmail.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSendEmail.Name = "btnSendEmail";
            this.btnSendEmail.Size = new System.Drawing.Size(100, 28);
            this.btnSendEmail.TabIndex = 19;
            this.btnSendEmail.Text = "Send Email";
            this.btnSendEmail.UseVisualStyleBackColor = true;
            this.btnSendEmail.Click += new System.EventHandler(this.btnSendEmail_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(961, 47);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(144, 17);
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
            this.dgvPastEmail.Location = new System.Drawing.Point(965, 75);
            this.dgvPastEmail.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvPastEmail.Name = "dgvPastEmail";
            this.dgvPastEmail.ReadOnly = true;
            this.dgvPastEmail.Size = new System.Drawing.Size(1293, 807);
            this.dgvPastEmail.TabIndex = 17;
            this.dgvPastEmail.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPastEmail_CellContentDoubleClick);
            // 
            // Subject
            // 
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            this.Subject.DefaultCellStyle = dataGridViewCellStyle21;
            this.Subject.FillWeight = 125F;
            this.Subject.HeaderText = "Subject";
            this.Subject.Name = "Subject";
            this.Subject.ReadOnly = true;
            this.Subject.Width = 125;
            // 
            // FromAddress
            // 
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            this.FromAddress.DefaultCellStyle = dataGridViewCellStyle22;
            this.FromAddress.FillWeight = 150F;
            this.FromAddress.HeaderText = "From Address";
            this.FromAddress.Name = "FromAddress";
            this.FromAddress.ReadOnly = true;
            this.FromAddress.Width = 150;
            // 
            // Date
            // 
            dataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.Date.DefaultCellStyle = dataGridViewCellStyle23;
            this.Date.FillWeight = 125F;
            this.Date.HeaderText = "Date";
            this.Date.Name = "Date";
            this.Date.ReadOnly = true;
            this.Date.Width = 125;
            // 
            // Body
            // 
            dataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            this.Body.DefaultCellStyle = dataGridViewCellStyle24;
            this.Body.FillWeight = 310F;
            this.Body.HeaderText = "Body";
            this.Body.Name = "Body";
            this.Body.ReadOnly = true;
            this.Body.Width = 310;
            // 
            // Response
            // 
            dataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            this.Response.DefaultCellStyle = dataGridViewCellStyle25;
            this.Response.FillWeight = 350F;
            this.Response.HeaderText = "Response";
            this.Response.Name = "Response";
            this.Response.ReadOnly = true;
            this.Response.Width = 350;
            // 
            // tbxDeterminedReply
            // 
            this.tbxDeterminedReply.Location = new System.Drawing.Point(12, 594);
            this.tbxDeterminedReply.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbxDeterminedReply.Multiline = true;
            this.tbxDeterminedReply.Name = "tbxDeterminedReply";
            this.tbxDeterminedReply.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxDeterminedReply.Size = new System.Drawing.Size(936, 287);
            this.tbxDeterminedReply.TabIndex = 16;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 575);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(153, 17);
            this.label9.TabIndex = 15;
            this.label9.Text = "Determined Response:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 263);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(104, 17);
            this.label8.TabIndex = 14;
            this.label8.Text = "Body plain text:";
            // 
            // tbxBodyPlainText
            // 
            this.tbxBodyPlainText.Location = new System.Drawing.Point(13, 283);
            this.tbxBodyPlainText.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbxBodyPlainText.Multiline = true;
            this.tbxBodyPlainText.Name = "tbxBodyPlainText";
            this.tbxBodyPlainText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxBodyPlainText.Size = new System.Drawing.Size(936, 287);
            this.tbxBodyPlainText.TabIndex = 2;
            // 
            // tbxDateReceived
            // 
            this.tbxDateReceived.Location = new System.Drawing.Point(144, 107);
            this.tbxDateReceived.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbxDateReceived.Name = "tbxDateReceived";
            this.tbxDateReceived.Size = new System.Drawing.Size(253, 22);
            this.tbxDateReceived.TabIndex = 13;
            // 
            // tbxDeterminedName
            // 
            this.tbxDeterminedName.Location = new System.Drawing.Point(144, 228);
            this.tbxDeterminedName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbxDeterminedName.Name = "tbxDeterminedName";
            this.tbxDeterminedName.Size = new System.Drawing.Size(253, 22);
            this.tbxDeterminedName.TabIndex = 12;
            // 
            // tbxDeterminedType
            // 
            this.tbxDeterminedType.Enabled = false;
            this.tbxDeterminedType.Location = new System.Drawing.Point(144, 196);
            this.tbxDeterminedType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbxDeterminedType.Name = "tbxDeterminedType";
            this.tbxDeterminedType.Size = new System.Drawing.Size(253, 22);
            this.tbxDeterminedType.TabIndex = 11;
            // 
            // cbxHasAttachments
            // 
            this.cbxHasAttachments.AutoSize = true;
            this.cbxHasAttachments.Location = new System.Drawing.Point(144, 139);
            this.cbxHasAttachments.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbxHasAttachments.Name = "cbxHasAttachments";
            this.cbxHasAttachments.Size = new System.Drawing.Size(18, 17);
            this.cbxHasAttachments.TabIndex = 10;
            this.cbxHasAttachments.UseVisualStyleBackColor = true;
            // 
            // tbxMessageId
            // 
            this.tbxMessageId.Location = new System.Drawing.Point(144, 164);
            this.tbxMessageId.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbxMessageId.Name = "tbxMessageId";
            this.tbxMessageId.Size = new System.Drawing.Size(804, 22);
            this.tbxMessageId.TabIndex = 9;
            // 
            // tbxFromAddress
            // 
            this.tbxFromAddress.Location = new System.Drawing.Point(144, 75);
            this.tbxFromAddress.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbxFromAddress.Name = "tbxFromAddress";
            this.tbxFromAddress.Size = new System.Drawing.Size(804, 22);
            this.tbxFromAddress.TabIndex = 8;
            // 
            // tbxSubject
            // 
            this.tbxSubject.Location = new System.Drawing.Point(144, 43);
            this.tbxSubject.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbxSubject.Name = "tbxSubject";
            this.tbxSubject.Size = new System.Drawing.Size(804, 22);
            this.tbxSubject.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 199);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(121, 17);
            this.label7.TabIndex = 6;
            this.label7.Text = "Determined Type:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 231);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(126, 17);
            this.label6.TabIndex = 5;
            this.label6.Text = "Determined Name:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(47, 167);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 17);
            this.label5.TabIndex = 4;
            this.label5.Text = "Message ID:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 139);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "Has Attachment:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 111);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Date Received:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 79);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "From Address:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 47);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Subject Line:";
            // 
            // lblMessageInfo
            // 
            this.lblMessageInfo.AutoSize = true;
            this.lblMessageInfo.Location = new System.Drawing.Point(1574, 19);
            this.lblMessageInfo.Name = "lblMessageInfo";
            this.lblMessageInfo.Size = new System.Drawing.Size(684, 17);
            this.lblMessageInfo.TabIndex = 27;
            this.lblMessageInfo.Text = "Sent Messages: 0,000   Skipped Messages: 000   Pending Messages: 000   Unprocesse" +
    "d Messages: 0,000";
            this.lblMessageInfo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cbxDebug
            // 
            this.cbxDebug.AutoSize = true;
            this.cbxDebug.Location = new System.Drawing.Point(2132, 901);
            this.cbxDebug.Name = "cbxDebug";
            this.cbxDebug.Size = new System.Drawing.Size(120, 21);
            this.cbxDebug.TabIndex = 28;
            this.cbxDebug.Text = "Enable Debug";
            this.cbxDebug.UseVisualStyleBackColor = true;
            this.cbxDebug.CheckedChanged += new System.EventHandler(this.cbxDebug_CheckedChanged);
            // 
            // lblCountdown
            // 
            this.lblCountdown.AutoSize = true;
            this.lblCountdown.Location = new System.Drawing.Point(1755, 48);
            this.lblCountdown.Name = "lblCountdown";
            this.lblCountdown.Size = new System.Drawing.Size(486, 17);
            this.lblCountdown.TabIndex = 29;
            this.lblCountdown.Text = "Estimated Time till Mailbox Cleared: 15 days 4 hours 33 minutes 23 seconds";
            // 
            // trckBar
            // 
            this.trckBar.Location = new System.Drawing.Point(1252, 889);
            this.trckBar.Maximum = 500;
            this.trckBar.Minimum = 200;
            this.trckBar.Name = "trckBar";
            this.trckBar.Size = new System.Drawing.Size(645, 56);
            this.trckBar.SmallChange = 10;
            this.trckBar.TabIndex = 30;
            this.trckBar.TickFrequency = 10;
            this.trckBar.Value = 240;
            this.trckBar.Scroll += new System.EventHandler(this.trckBar_Scroll);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(1130, 902);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(116, 17);
            this.label11.TabIndex = 31;
            this.label11.Text = "Send Frequency:";
            // 
            // lblTrackBarValue
            // 
            this.lblTrackBarValue.AutoSize = true;
            this.lblTrackBarValue.Location = new System.Drawing.Point(1147, 928);
            this.lblTrackBarValue.Name = "lblTrackBarValue";
            this.lblTrackBarValue.Size = new System.Drawing.Size(99, 17);
            this.lblTrackBarValue.TabIndex = 32;
            this.lblTrackBarValue.Text = "(240 seconds)";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2299, 1106);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tbxOutput);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.RightToLeftLayout = true;
            this.Text = "Mail Server";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPastEmail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trckBar)).EndInit();
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
        private System.Windows.Forms.Button btnSaveChanges;
        private System.Windows.Forms.CheckBox cbxAutoSend;
        private System.Windows.Forms.Label lblMessageInfo;
        private System.Windows.Forms.CheckBox cbxDebug;
        private System.Windows.Forms.Label lblCountdown;
        private System.Windows.Forms.TrackBar trckBar;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblTrackBarValue;
    }
}

