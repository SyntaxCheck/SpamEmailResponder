using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static ResponseProcessing;

namespace MailServer
{
    public partial class PopupText : Form
    {
        public string GroupBoxText { get; set; }
        public string TextToDisplay { get; set; }

        public PopupText()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void SetValues()
        {
            gbxMain.Text = GroupBoxText;
            txtMainText.Text = TextToDisplay;
        }
    }
}
