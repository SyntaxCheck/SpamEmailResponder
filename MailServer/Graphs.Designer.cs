namespace MailServer
{
    partial class Graphs
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chartMain = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbThreadLength = new System.Windows.Forms.RadioButton();
            this.rbMessageType = new System.Windows.Forms.RadioButton();
            this.calcTimer = new System.Windows.Forms.Timer(this.components);
            this.lblCalculating = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartMain)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Turquoise;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1051, 24);
            this.menuStrip1.TabIndex = 0;
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
            // chartMain
            // 
            chartArea3.Name = "ChartArea1";
            this.chartMain.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.chartMain.Legends.Add(legend3);
            this.chartMain.Location = new System.Drawing.Point(12, 82);
            this.chartMain.Name = "chartMain";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chartMain.Series.Add(series3);
            this.chartMain.Size = new System.Drawing.Size(1027, 455);
            this.chartMain.TabIndex = 1;
            this.chartMain.Text = "Main Chart";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbThreadLength);
            this.groupBox1.Controls.Add(this.rbMessageType);
            this.groupBox1.Location = new System.Drawing.Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1027, 49);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Graph Type";
            // 
            // rbThreadLength
            // 
            this.rbThreadLength.AutoSize = true;
            this.rbThreadLength.Location = new System.Drawing.Point(201, 20);
            this.rbThreadLength.Name = "rbThreadLength";
            this.rbThreadLength.Size = new System.Drawing.Size(95, 17);
            this.rbThreadLength.TabIndex = 1;
            this.rbThreadLength.TabStop = true;
            this.rbThreadLength.Text = "Thread Length";
            this.rbThreadLength.UseVisualStyleBackColor = true;
            this.rbThreadLength.CheckedChanged += new System.EventHandler(this.rbThreadLength_CheckedChanged);
            // 
            // rbMessageType
            // 
            this.rbMessageType.AutoSize = true;
            this.rbMessageType.Checked = true;
            this.rbMessageType.Location = new System.Drawing.Point(15, 20);
            this.rbMessageType.Name = "rbMessageType";
            this.rbMessageType.Size = new System.Drawing.Size(145, 17);
            this.rbMessageType.TabIndex = 0;
            this.rbMessageType.TabStop = true;
            this.rbMessageType.Text = "Message Type Bar Graph";
            this.rbMessageType.UseVisualStyleBackColor = true;
            this.rbMessageType.CheckedChanged += new System.EventHandler(this.rbMessageType_CheckedChanged);
            // 
            // calcTimer
            // 
            this.calcTimer.Interval = 500;
            this.calcTimer.Tick += new System.EventHandler(this.calcTimer_Tick);
            // 
            // lblCalculating
            // 
            this.lblCalculating.AutoSize = true;
            this.lblCalculating.BackColor = System.Drawing.Color.White;
            this.lblCalculating.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCalculating.Font = new System.Drawing.Font("Tahoma", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCalculating.ForeColor = System.Drawing.Color.Red;
            this.lblCalculating.Location = new System.Drawing.Point(50, 241);
            this.lblCalculating.Name = "lblCalculating";
            this.lblCalculating.Size = new System.Drawing.Size(366, 35);
            this.lblCalculating.TabIndex = 37;
            this.lblCalculating.Text = "Stats are being calculated";
            this.lblCalculating.Visible = false;
            // 
            // Graphs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(1051, 549);
            this.Controls.Add(this.lblCalculating);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chartMain);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Graphs";
            this.Text = "Graphs";
            this.Load += new System.EventHandler(this.Graphs_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartMain)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartMain;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbMessageType;
        private System.Windows.Forms.RadioButton rbThreadLength;
        private System.Windows.Forms.Timer calcTimer;
        private System.Windows.Forms.Label lblCalculating;
    }
}