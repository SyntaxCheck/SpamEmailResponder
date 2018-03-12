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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.processTimer = new System.Windows.Forms.Timer(this.components);
            this.tbxOutput = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.trckBar = new System.Windows.Forms.TrackBar();
            this.lblNext = new System.Windows.Forms.Label();
            this.lblTimeTillNextSend = new System.Windows.Forms.Label();
            this.lblTrackBarValue = new System.Windows.Forms.Label();
            this.lblSendFreq = new System.Windows.Forms.Label();
            this.lblCountdown = new System.Windows.Forms.Label();
            this.cbxDebug = new System.Windows.Forms.CheckBox();
            this.lblMessageInfo = new System.Windows.Forms.Label();
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
            this.countdownTimer = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.responseConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.storageViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trckBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPastEmail)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // processTimer
            // 
            this.processTimer.Tick += new System.EventHandler(this.processTimer_Tick);
            // 
            // tbxOutput
            // 
            this.tbxOutput.Location = new System.Drawing.Point(13, 880);
            this.tbxOutput.Margin = new System.Windows.Forms.Padding(4);
            this.tbxOutput.Multiline = true;
            this.tbxOutput.Name = "tbxOutput";
            this.tbxOutput.Size = new System.Drawing.Size(1841, 74);
            this.tbxOutput.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.lblNext);
            this.groupBox1.Controls.Add(this.lblTimeTillNextSend);
            this.groupBox1.Controls.Add(this.lblTrackBarValue);
            this.groupBox1.Controls.Add(this.lblSendFreq);
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
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(16, 36);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(1838, 836);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Current Message";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.trckBar);
            this.panel1.Location = new System.Drawing.Point(978, 783);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(396, 48);
            this.panel1.TabIndex = 35;
            this.panel1.Visible = false;
            // 
            // trckBar
            // 
            this.trckBar.BackColor = System.Drawing.Color.White;
            this.trckBar.Location = new System.Drawing.Point(-1, 0);
            this.trckBar.Margin = new System.Windows.Forms.Padding(0);
            this.trckBar.Maximum = 500;
            this.trckBar.Minimum = 200;
            this.trckBar.Name = "trckBar";
            this.trckBar.Size = new System.Drawing.Size(394, 45);
            this.trckBar.TabIndex = 30;
            this.trckBar.TickFrequency = 10;
            this.trckBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trckBar.Value = 240;
            this.trckBar.Visible = false;
            this.trckBar.Scroll += new System.EventHandler(this.trckBar_Scroll);
            // 
            // lblNext
            // 
            this.lblNext.AutoSize = true;
            this.lblNext.Location = new System.Drawing.Point(705, 803);
            this.lblNext.Name = "lblNext";
            this.lblNext.Size = new System.Drawing.Size(34, 13);
            this.lblNext.TabIndex = 34;
            this.lblNext.Text = "Next:";
            this.lblNext.Visible = false;
            // 
            // lblTimeTillNextSend
            // 
            this.lblTimeTillNextSend.AutoSize = true;
            this.lblTimeTillNextSend.ForeColor = System.Drawing.Color.DarkRed;
            this.lblTimeTillNextSend.Location = new System.Drawing.Point(745, 803);
            this.lblTimeTillNextSend.Name = "lblTimeTillNextSend";
            this.lblTimeTillNextSend.Size = new System.Drawing.Size(13, 13);
            this.lblTimeTillNextSend.TabIndex = 33;
            this.lblTimeTillNextSend.Text = "0";
            this.lblTimeTillNextSend.Visible = false;
            // 
            // lblTrackBarValue
            // 
            this.lblTrackBarValue.AutoSize = true;
            this.lblTrackBarValue.Location = new System.Drawing.Point(897, 803);
            this.lblTrackBarValue.Name = "lblTrackBarValue";
            this.lblTrackBarValue.Size = new System.Drawing.Size(75, 13);
            this.lblTrackBarValue.TabIndex = 32;
            this.lblTrackBarValue.Text = "(240 seconds)";
            this.lblTrackBarValue.Visible = false;
            // 
            // lblSendFreq
            // 
            this.lblSendFreq.AutoSize = true;
            this.lblSendFreq.Location = new System.Drawing.Point(883, 783);
            this.lblSendFreq.Name = "lblSendFreq";
            this.lblSendFreq.Size = new System.Drawing.Size(89, 13);
            this.lblSendFreq.TabIndex = 31;
            this.lblSendFreq.Text = "Send Frequency:";
            this.lblSendFreq.Visible = false;
            // 
            // lblCountdown
            // 
            this.lblCountdown.AutoSize = true;
            this.lblCountdown.Location = new System.Drawing.Point(1455, 49);
            this.lblCountdown.Name = "lblCountdown";
            this.lblCountdown.Size = new System.Drawing.Size(23, 13);
            this.lblCountdown.TabIndex = 29;
            this.lblCountdown.Text = "....";
            // 
            // cbxDebug
            // 
            this.cbxDebug.AutoSize = true;
            this.cbxDebug.Location = new System.Drawing.Point(1737, 782);
            this.cbxDebug.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbxDebug.Name = "cbxDebug";
            this.cbxDebug.Size = new System.Drawing.Size(92, 17);
            this.cbxDebug.TabIndex = 28;
            this.cbxDebug.Text = "Enable Debug";
            this.cbxDebug.UseVisualStyleBackColor = true;
            this.cbxDebug.CheckedChanged += new System.EventHandler(this.cbxDebug_CheckedChanged);
            // 
            // lblMessageInfo
            // 
            this.lblMessageInfo.AutoSize = true;
            this.lblMessageInfo.Location = new System.Drawing.Point(1439, 21);
            this.lblMessageInfo.Name = "lblMessageInfo";
            this.lblMessageInfo.Size = new System.Drawing.Size(378, 13);
            this.lblMessageInfo.TabIndex = 27;
            this.lblMessageInfo.Text = "Sent: 0,000   Skipped: 0,000   Ignored: 0,000   Pending: 000   Unread: 0,000";
            this.lblMessageInfo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cbxAutoSend
            // 
            this.cbxAutoSend.AutoSize = true;
            this.cbxAutoSend.Location = new System.Drawing.Point(699, 782);
            this.cbxAutoSend.Margin = new System.Windows.Forms.Padding(4);
            this.cbxAutoSend.Name = "cbxAutoSend";
            this.cbxAutoSend.Size = new System.Drawing.Size(76, 17);
            this.cbxAutoSend.TabIndex = 26;
            this.cbxAutoSend.Text = "Auto Send";
            this.cbxAutoSend.UseVisualStyleBackColor = true;
            this.cbxAutoSend.CheckedChanged += new System.EventHandler(this.cbxAutoSend_CheckedChanged);
            // 
            // btnSaveChanges
            // 
            this.btnSaveChanges.Location = new System.Drawing.Point(572, 127);
            this.btnSaveChanges.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSaveChanges.Name = "btnSaveChanges";
            this.btnSaveChanges.Size = new System.Drawing.Size(102, 24);
            this.btnSaveChanges.TabIndex = 25;
            this.btnSaveChanges.Text = "Save Changes";
            this.btnSaveChanges.UseVisualStyleBackColor = true;
            this.btnSaveChanges.Click += new System.EventHandler(this.btnSaveChanges_Click);
            // 
            // tbxAttachmentNames
            // 
            this.tbxAttachmentNames.Location = new System.Drawing.Point(142, 102);
            this.tbxAttachmentNames.Margin = new System.Windows.Forms.Padding(4);
            this.tbxAttachmentNames.Name = "tbxAttachmentNames";
            this.tbxAttachmentNames.Size = new System.Drawing.Size(545, 20);
            this.tbxAttachmentNames.TabIndex = 24;
            // 
            // btnRegenerate
            // 
            this.btnRegenerate.BackColor = System.Drawing.Color.AliceBlue;
            this.btnRegenerate.Location = new System.Drawing.Point(12, 776);
            this.btnRegenerate.Margin = new System.Windows.Forms.Padding(4);
            this.btnRegenerate.Name = "btnRegenerate";
            this.btnRegenerate.Size = new System.Drawing.Size(82, 23);
            this.btnRegenerate.TabIndex = 23;
            this.btnRegenerate.Text = "ReGen";
            this.btnRegenerate.UseVisualStyleBackColor = false;
            this.btnRegenerate.Click += new System.EventHandler(this.btnRegenerate_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.AliceBlue;
            this.btnRefresh.Location = new System.Drawing.Point(1564, 778);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(82, 23);
            this.btnRefresh.TabIndex = 22;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnIgnore
            // 
            this.btnIgnore.BackColor = System.Drawing.Color.AliceBlue;
            this.btnIgnore.Enabled = false;
            this.btnIgnore.Location = new System.Drawing.Point(326, 776);
            this.btnIgnore.Margin = new System.Windows.Forms.Padding(4);
            this.btnIgnore.Name = "btnIgnore";
            this.btnIgnore.Size = new System.Drawing.Size(82, 23);
            this.btnIgnore.TabIndex = 21;
            this.btnIgnore.Text = "Ignore";
            this.btnIgnore.UseVisualStyleBackColor = false;
            this.btnIgnore.Click += new System.EventHandler(this.btnIgnore_Click);
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.Color.AliceBlue;
            this.btnNext.Enabled = false;
            this.btnNext.Location = new System.Drawing.Point(236, 776);
            this.btnNext.Margin = new System.Windows.Forms.Padding(4);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(82, 23);
            this.btnNext.TabIndex = 20;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnSendEmail
            // 
            this.btnSendEmail.BackColor = System.Drawing.Color.AliceBlue;
            this.btnSendEmail.Enabled = false;
            this.btnSendEmail.Location = new System.Drawing.Point(542, 776);
            this.btnSendEmail.Margin = new System.Windows.Forms.Padding(4);
            this.btnSendEmail.Name = "btnSendEmail";
            this.btnSendEmail.Size = new System.Drawing.Size(82, 23);
            this.btnSendEmail.TabIndex = 19;
            this.btnSendEmail.Text = "Send Email";
            this.btnSendEmail.UseVisualStyleBackColor = false;
            this.btnSendEmail.Click += new System.EventHandler(this.btnSendEmail_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(695, 53);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(110, 13);
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
            this.dgvPastEmail.Location = new System.Drawing.Point(695, 77);
            this.dgvPastEmail.Margin = new System.Windows.Forms.Padding(4);
            this.dgvPastEmail.Name = "dgvPastEmail";
            this.dgvPastEmail.ReadOnly = true;
            this.dgvPastEmail.Size = new System.Drawing.Size(1135, 693);
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
            this.tbxDeterminedReply.Location = new System.Drawing.Point(12, 488);
            this.tbxDeterminedReply.Margin = new System.Windows.Forms.Padding(4);
            this.tbxDeterminedReply.Multiline = true;
            this.tbxDeterminedReply.Name = "tbxDeterminedReply";
            this.tbxDeterminedReply.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxDeterminedReply.Size = new System.Drawing.Size(675, 280);
            this.tbxDeterminedReply.TabIndex = 16;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 471);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(116, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "Determined Response:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 168);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Body plain text:";
            // 
            // tbxBodyPlainText
            // 
            this.tbxBodyPlainText.Location = new System.Drawing.Point(8, 185);
            this.tbxBodyPlainText.Margin = new System.Windows.Forms.Padding(4);
            this.tbxBodyPlainText.Multiline = true;
            this.tbxBodyPlainText.Name = "tbxBodyPlainText";
            this.tbxBodyPlainText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxBodyPlainText.Size = new System.Drawing.Size(679, 280);
            this.tbxBodyPlainText.TabIndex = 2;
            // 
            // tbxDateReceived
            // 
            this.tbxDateReceived.Location = new System.Drawing.Point(114, 74);
            this.tbxDateReceived.Margin = new System.Windows.Forms.Padding(4);
            this.tbxDateReceived.Name = "tbxDateReceived";
            this.tbxDateReceived.Size = new System.Drawing.Size(116, 20);
            this.tbxDateReceived.TabIndex = 13;
            // 
            // tbxDeterminedName
            // 
            this.tbxDeterminedName.Location = new System.Drawing.Point(342, 130);
            this.tbxDeterminedName.Margin = new System.Windows.Forms.Padding(4);
            this.tbxDeterminedName.Name = "tbxDeterminedName";
            this.tbxDeterminedName.Size = new System.Drawing.Size(223, 20);
            this.tbxDeterminedName.TabIndex = 12;
            // 
            // tbxDeterminedType
            // 
            this.tbxDeterminedType.Enabled = false;
            this.tbxDeterminedType.Location = new System.Drawing.Point(113, 130);
            this.tbxDeterminedType.Margin = new System.Windows.Forms.Padding(4);
            this.tbxDeterminedType.Name = "tbxDeterminedType";
            this.tbxDeterminedType.Size = new System.Drawing.Size(117, 20);
            this.tbxDeterminedType.TabIndex = 11;
            // 
            // cbxHasAttachments
            // 
            this.cbxHasAttachments.AutoSize = true;
            this.cbxHasAttachments.Location = new System.Drawing.Point(119, 105);
            this.cbxHasAttachments.Margin = new System.Windows.Forms.Padding(4);
            this.cbxHasAttachments.Name = "cbxHasAttachments";
            this.cbxHasAttachments.Size = new System.Drawing.Size(15, 14);
            this.cbxHasAttachments.TabIndex = 10;
            this.cbxHasAttachments.UseVisualStyleBackColor = true;
            // 
            // tbxMessageId
            // 
            this.tbxMessageId.Enabled = false;
            this.tbxMessageId.Location = new System.Drawing.Point(314, 74);
            this.tbxMessageId.Margin = new System.Windows.Forms.Padding(4);
            this.tbxMessageId.Name = "tbxMessageId";
            this.tbxMessageId.Size = new System.Drawing.Size(373, 20);
            this.tbxMessageId.TabIndex = 9;
            // 
            // tbxFromAddress
            // 
            this.tbxFromAddress.Location = new System.Drawing.Point(114, 46);
            this.tbxFromAddress.Margin = new System.Windows.Forms.Padding(4);
            this.tbxFromAddress.Name = "tbxFromAddress";
            this.tbxFromAddress.Size = new System.Drawing.Size(573, 20);
            this.tbxFromAddress.TabIndex = 8;
            // 
            // tbxSubject
            // 
            this.tbxSubject.Location = new System.Drawing.Point(113, 18);
            this.tbxSubject.Margin = new System.Windows.Forms.Padding(4);
            this.tbxSubject.Name = "tbxSubject";
            this.tbxSubject.Size = new System.Drawing.Size(574, 20);
            this.tbxSubject.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 133);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(93, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Determined Type:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(238, 133);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Determined Name:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(239, 77);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Message ID:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 105);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Has Attachment:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 77);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Date Received:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 49);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "From Address:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Subject Line:";
            // 
            // countdownTimer
            // 
            this.countdownTimer.Interval = 1000;
            this.countdownTimer.Tick += new System.EventHandler(this.countdownTimer_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Turquoise;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1858, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.responseConfigToolStripMenuItem,
            this.storageViewerToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // responseConfigToolStripMenuItem
            // 
            this.responseConfigToolStripMenuItem.Name = "responseConfigToolStripMenuItem";
            this.responseConfigToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.responseConfigToolStripMenuItem.Text = "Response Config";
            // 
            // storageViewerToolStripMenuItem
            // 
            this.storageViewerToolStripMenuItem.Name = "storageViewerToolStripMenuItem";
            this.storageViewerToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.storageViewerToolStripMenuItem.Text = "Storage Viewer";
            this.storageViewerToolStripMenuItem.Click += new System.EventHandler(this.storageViewerToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(1858, 962);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tbxOutput);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.RightToLeftLayout = true;
            this.Text = "Mail Server";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trckBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPastEmail)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
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
        private System.Windows.Forms.Label lblSendFreq;
        private System.Windows.Forms.Label lblTrackBarValue;
        private System.Windows.Forms.Label lblTimeTillNextSend;
        private System.Windows.Forms.Timer countdownTimer;
        private System.Windows.Forms.Label lblNext;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem responseConfigToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem storageViewerToolStripMenuItem;
    }
}

