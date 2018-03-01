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
using static MailServerFunctions;

namespace MailServer
{
    public partial class StorageViewer : Form
    {
        private List<MailStorage> storage;
        private SerializeHelper serializeHelper;
        private const string STORAGE_OBJECT_FILENAME = "MailStorage.store";

        public StorageViewer()
        {
            InitializeComponent();

            try
            {
                storage = new List<MailStorage>();
                serializeHelper = new SerializeHelper();
                string currentDirectory = Directory.GetCurrentDirectory();

                string fullPath = Path.Combine(currentDirectory, STORAGE_OBJECT_FILENAME);
                if (File.Exists(fullPath))
                {
                    storage = serializeHelper.ReadFromBinaryFile<List<MailStorage>>(fullPath);
                }

                dgvEmails.Rows.Clear();

                foreach (MailStorage ms in storage)
                {
                    if(!ms.Replied)
                        dgvEmails.Rows.Add(ms.ToAddress, ms.SubjectLine, ms.DateReceived.ToString("yyyy-MM-dd hh:mm"), ms.PersonName, ((EmailType)ms.MessageType).ToString(), ms.Replied.ToString(), ms.EmailBodyPlain, ms.DeterminedReply, ms.NumberOfAttachments.ToString(), ms.MsgId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + Environment.NewLine + "Stack: " + ex.StackTrace, "Failed");
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
                    tbxDeterminedName.Text = (string)dgvEmails[3, e.RowIndex].Value;
                    tbxDeterminedType.Text = (string)dgvEmails[4, e.RowIndex].Value;
                    tbxBodyPlainText.Text = (string)dgvEmails[6, e.RowIndex].Value;
                    tbxDeterminedReply.Text = (string)dgvEmails[7, e.RowIndex].Value;
                    tbxMessageId.Text = (string)dgvEmails[9, e.RowIndex].Value;

                    if (dgvEmails[8, e.RowIndex].Value.ToString().ToUpper() == "TRUE")
                    {
                        cbxHasAttachments.Checked = true;
                    }
                    else
                    {
                        cbxHasAttachments.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + Environment.NewLine + "Stack: " + ex.StackTrace, "Failed");
            }
}
    }
}
