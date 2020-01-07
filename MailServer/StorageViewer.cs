using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ResponseProcessing;

namespace MailServer
{
    public partial class StorageViewer : Form
    {
        private List<MailStorage> storage;
        private SerializeHelper serializeHelper;
        private MailServerFunctions mailServer;

        public StorageViewer()
        {
            InitializeComponent();

            try
            {
                storage = new List<MailStorage>();
                serializeHelper = new SerializeHelper();
                mailServer = new MailServerFunctions();


                LoadScreen();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + Environment.NewLine + "Stack: " + ex.StackTrace, "Failed to init screen");
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void dgvEmails_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    tbxFromAddress.Text = (string)dgvEmails[0, e.RowIndex].Value;
                    tbxSubject.Text = (string)dgvEmails[1, e.RowIndex].Value;
                    tbxDateReceived.Text = (string)dgvEmails[2, e.RowIndex].Value;
                    tbxDeterminedName.Text = (string)dgvEmails[4, e.RowIndex].Value;
                    tbxDeterminedType.Text = (string)dgvEmails[5, e.RowIndex].Value;
                    tbxBodyPlainText.Text = (string)dgvEmails[8, e.RowIndex].Value;
                    tbxDeterminedReply.Text = (string)dgvEmails[9, e.RowIndex].Value;
                    tbxMessageId.Text = (string)dgvEmails[11, e.RowIndex].Value;

                    int attachCount = 0;
                    if (int.TryParse(dgvEmails[10, e.RowIndex].Value.ToString(), out attachCount))
                    {
                        if (attachCount > 0)
                            cbxHasAttachments.Checked = true;
                        else
                            cbxHasAttachments.Checked = false;
                    }
                    else
                    {
                        cbxHasAttachments.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + Environment.NewLine + "Stack: " + ex.StackTrace, "Failed to load Grid");
            }
}
        private void cbxShowAll_CheckedChanged(object sender, EventArgs e)
        {
            LoadScreen();
        }
        private void cbxHideWithResponse_CheckedChanged(object sender, EventArgs e)
        {
            LoadScreen();
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadScreen();
        }
        private void dgvEmails_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }
        private void MakeColumnsSortable_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //Add this as an event on DataBindingComplete
            DataGridView dataGridView = sender as DataGridView;
            if (dataGridView == null)
            {
                var ex = new InvalidOperationException("This event is for a DataGridView type senders only.");
                ex.Data.Add("Sender type", sender.GetType().Name);
                throw ex;
            }

            foreach (DataGridViewColumn column in dataGridView.Columns)
                column.SortMode = DataGridViewColumnSortMode.Automatic;
        }
        private void graphsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Graphs graph = new Graphs();
            graph.Show();
        }

        //Private functions
        private void LoadScreen()
        {
            try
            {
                string currentDirectory = Directory.GetCurrentDirectory();

                string fullPath = Path.Combine(currentDirectory, StaticVariables.STORAGE_OBJECT_FILENAME);
                if (File.Exists(fullPath))
                {
                    storage = serializeHelper.ReadFromBinaryFile<List<MailStorage>>(fullPath);
                }

                dgvEmails.Rows.Clear();

                int count = 0;
                foreach (MailStorage ms in storage)
                {
                    if (cbxShowAll.Checked || (!cbxHideWithResponse.Checked && !ms.Replied) || (cbxHideWithResponse.Checked && !ms.Replied && String.IsNullOrEmpty(ms.DeterminedReply.Trim())))
                    {
                        dgvEmails.Rows.Add(ms.ToAddress, ms.SubjectLine, ms.DateReceived.ToString("yyyy-MM-dd hh:mm"), ms.DateProcessed.ToString("yyyy-MM-dd hh:mm"), ms.PersonName, ((EmailType)ms.MessageType).ToString(), ms.Replied.ToString(), ms.Ignored.ToString(), TextProcessing.MakeEmailEasierToRead(ms.EmailBodyPlain), TextProcessing.MakeEmailEasierToRead(ms.DeterminedReply), ms.NumberOfAttachments.ToString(), ms.MsgId, ms.InReplyToMsgId, ms.MyReplyMsgId);
                        count++;
                    }
                }
                dgvEmails.DataBindingComplete += MakeColumnsSortable_DataBindingComplete;
                gbxEmails.Text = "Emails, Count: " + count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + Environment.NewLine + "Stack: " + ex.StackTrace, "Failed to LoadScreen()");
            }
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }
}
