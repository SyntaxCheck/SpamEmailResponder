using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static MailServerFunctions;

namespace MailServer
{
    public partial class Form1 : Form
    {
        string storageObjectFile = "MailStorage.store";
        string currentDirectory;
        LoggerInfo loggerInfo;
        List<MailStorage> storage = new List<MailStorage>();
        HelperFunctions helperFunctions;
        MailServerFunctions mailServer = new MailServerFunctions();
        string workingOnMsg = String.Empty;
        string skippedMessages = String.Empty;

        public Form1()
        {
            helperFunctions = new HelperFunctions();
            currentDirectory = Directory.GetCurrentDirectory();

            InitializeComponent();

            string path = System.Reflection.Assembly.GetEntryAssembly().Location.ToString();

            loggerInfo = new LoggerInfo();
            loggerInfo.RootPath = path.Substring(0, path.LastIndexOf('\\'));
            loggerInfo.FolderName = "LogsFolder";
            loggerInfo.FileName = "log_" + DateTimeConversion.DateTimeToStringTimestamp(DateTime.Now.Date).Replace('-', ' ').Replace(':', ' ').Replace('.', ' ') + ".txt";
            Logger.ValidatePath(ref loggerInfo);
            if (loggerInfo.ErrorFlag)
            {
                MessageBox.Show(loggerInfo.ErrorMsg, "Logger Error");
            }

            //Load in the storage serialized class
            string fullPath = Path.Combine(currentDirectory, storageObjectFile);
            if (File.Exists(fullPath))
            {
                storage = helperFunctions.ReadFromBinaryFile<List<MailStorage>>(fullPath);
            }

            CheckForMessages();

            //processTimer.Interval = 1; //Thirty seconds
            ////processTimer.Interval = 300000; //Five minutes
            //processTimer.Start();
        }

        private void processTimer_Tick(object sender, EventArgs e)
        {
            StandardResponse response = new StandardResponse() { Code = 0, Message = String.Empty, Data = String.Empty };
            processTimer.Interval = 600000; //Every 10 minutes check for new mail

            int preCount = storage.Count();
            response = mailServer.GetMessages(loggerInfo, ref storage);
            int postCount = storage.Count();
            if (response.Code < 0)
            {
                tbxOutput.Text = response.AsString();
            }
            else if(postCount > preCount)
            {
                processTimer.Stop();
                LoadMessage();
            }

            //Write Storage object to disk
            if (storage.Count() > 0)
            {
                string fullPath = Path.Combine(currentDirectory, storageObjectFile);
                helperFunctions.WriteToBinaryFile(fullPath, storage, false);
            }
        }
        private void btnSendEmail_Click(object sender, EventArgs e)
        {
            StandardResponse response = new StandardResponse() { Code = 0, Message = String.Empty, Data = String.Empty };

            btnSendEmail.Enabled = false;
            btnNext.Enabled = false;
            btnIgnore.Enabled = false;
            btnRegenerate.Enabled = false;

            //Save data from screen to object
            SaveData();

            for (int i = 0; i < storage.Count(); i++)
            {
                if (storage[i].MsgId == workingOnMsg)
                {
                    response = mailServer.SendSMTP(loggerInfo, storage[i].ToAddress, storage[i].SubjectLine, storage[i].DeterminedReply);
                    if (response.Code >= 0)
                    {
                        storage[i].Replied = true;
                        workingOnMsg = String.Empty;
                        CheckForMessages();
                    }
                    else
                    {
                        tbxOutput.Text = response.AsString();
                    }
                    break;
                }
            }
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            btnSendEmail.Enabled = false;
            btnNext.Enabled = false;
            btnIgnore.Enabled = false;
            btnRegenerate.Enabled = false;

            skippedMessages += ";;;" + workingOnMsg + ";;;";
            CheckForMessages();
        }
        private void btnIgnore_Click(object sender, EventArgs e)
        {
            btnSendEmail.Enabled = false;
            btnNext.Enabled = false;
            btnIgnore.Enabled = false;
            btnRegenerate.Enabled = false;

            for (int i = 0; i < storage.Count(); i++)
            {
                if (storage[i].MsgId == workingOnMsg)
                {
                    storage[i].Replied = true;
                    workingOnMsg = String.Empty;
                    CheckForMessages();

                    break;
                }
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            btnSendEmail.Enabled = false;
            btnNext.Enabled = false;
            btnIgnore.Enabled = false;
            btnRegenerate.Enabled = false;

            CheckForMessages();
        }
        private void btnRegenerate_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < storage.Count(); i++)
            {
                if (storage[i].MsgId == workingOnMsg)
                {
                    List<MailStorage> previousMessagesInThread = new List<MailStorage>();
                    foreach (MailStorage ms in storage)
                    {
                        if (ms.SubjectLine == storage[i].SubjectLine && ms.MsgId != storage[i].MsgId)
                        {
                            int foundCount = 0;
                            foreach (var v in storage[i].ToAddressList)
                            {
                                if (ms.ToAddress.Contains(v.ToString()))
                                {
                                    foundCount++;
                                    break;
                                }
                            }

                            if (foundCount > 0)
                            {
                                previousMessagesInThread.Add(ms);
                                if (ms.MessageType != (int)EmailType.Unknown)
                                {
                                    storage[i].MessageType = ms.MessageType;
                                }
                            }
                        }
                    }
                    MailStorage temp = storage[i];
                    string newReply = mailServer.GetResponseForType(ref temp, previousMessagesInThread);

                    tbxDeterminedReply.Text = newReply;
                    temp.DeterminedReply = newReply;

                    storage[i] = temp;

                    LoadScreen(storage[i], previousMessagesInThread);

                    //Write Storage object to disk
                    if (storage.Count() > 0)
                    {
                        string fullPath = Path.Combine(currentDirectory, storageObjectFile);
                        helperFunctions.WriteToBinaryFile(fullPath, storage, false);
                    }

                    break;
                }
            }
        }

        //Private functions
        private void CheckForMessages()
        {
            StandardResponse response = new StandardResponse() { Code = 0, Message = String.Empty, Data = String.Empty };

            //Check for new messages
            response = mailServer.GetMessages(loggerInfo, ref storage);
            if (response.Code < 0)
            {
                tbxOutput.Text = response.AsString();
            }

            //Write Storage object to disk
            if (storage.Count() > 0)
            {
                string fullPath = Path.Combine(currentDirectory, storageObjectFile);
                helperFunctions.WriteToBinaryFile(fullPath, storage, false);

                if (response.Code >= 0)
                    LoadMessage();
            }
        }
        private void ClearScreen()
        {
            tbxBodyPlainText.Text = String.Empty;
            tbxDateReceived.Text = String.Empty;
            tbxDeterminedName.Text = String.Empty;
            tbxDeterminedType.Text = String.Empty;
            tbxDeterminedReply.Text = String.Empty;
            tbxFromAddress.Text = String.Empty;
            tbxMessageId.Text = String.Empty;
            tbxOutput.Text = String.Empty;
            tbxSubject.Text = String.Empty;
            cbxHasAttachments.Checked = false;
            dgvPastEmail.Rows.Clear();
            tbxOutput.Text = String.Empty;
            tbxAttachmentNames.Text = String.Empty;
        }
        private void LoadScreen(MailStorage ms, List<MailStorage> previousMessagesInThread)
        {
            tbxBodyPlainText.Text = mailServer.MakeEmailEasierToRead(ms.EmailBodyPlain);
            tbxDateReceived.Text = ms.DateReceived.ToString("yyyy-MM-dd hh:mm");
            tbxDeterminedName.Text = ms.PersonName;
            tbxDeterminedType.Text = ((EmailType)ms.MessageType).ToString();
            tbxDeterminedReply.Text = ms.DeterminedReply;
            tbxFromAddress.Text = ms.ToAddress;
            tbxMessageId.Text = ms.MsgId;
            tbxSubject.Text = ms.SubjectLine;
            tbxOutput.Text = String.Empty;

            if (ms.NumberOfAttachments > 0)
            {
                cbxHasAttachments.Checked = true;
                tbxAttachmentNames.Text = ms.AttachmentNames;
            }

            dgvPastEmail.Rows.Clear();
            dgvPastEmail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvPastEmail.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            foreach (MailStorage ms2 in previousMessagesInThread)
            {
                dgvPastEmail.Rows.Add(ms2.SubjectLine, ms2.ToAddress, ms2.DateReceived, mailServer.MakeEmailEasierToRead(ms2.EmailBodyPlain), ms2.DeterminedReply);
            }

            workingOnMsg = ms.MsgId;
            if (((EmailType)ms.MessageType) != EmailType.Unknown)
                btnSendEmail.Enabled = true;
            else
                tbxOutput.Text = "Unknown email type. Please add code to handle this type of email or ignore it.";
            btnNext.Enabled = true;
            btnIgnore.Enabled = true;
            btnRegenerate.Enabled = true;
        }
        private void SaveData()
        {
            for (int i = 0; i < storage.Count(); i++)
            {
                if (storage[i].MsgId == workingOnMsg)
                {
                    storage[i].DeterminedReply = tbxDeterminedReply.Text;
                    storage[i].PersonName = tbxDeterminedName.Text;
                    storage[i].ToAddress = tbxFromAddress.Text;
                    storage[i].SubjectLine = tbxSubject.Text;

                    //Write Storage object to disk
                    if (storage.Count() > 0)
                    {
                        string fullPath = Path.Combine(currentDirectory, storageObjectFile);
                        helperFunctions.WriteToBinaryFile(fullPath, storage, false);
                    }
                    break;
                }
            }
        }
        private void LoadMessage()
        {
            StandardResponse response = new StandardResponse() { Code = 0, Message = String.Empty, Data = String.Empty };
            bool found = false;

            ClearScreen();

            for (int i = 0; i < storage.Count(); i++)
            {
                if (!storage[i].Replied && !skippedMessages.Contains(";;;" + storage[i].MsgId + ";;;"))
                {
                    //Make sure it has been atleast 5 hours
                    double hours = (DateTime.Now - storage[i].DateReceived).TotalHours;
                    if (hours > 5)
                    {
                        List<MailStorage> previousMessagesInThread = new List<MailStorage>();
                        foreach (MailStorage ms in storage)
                        {
                            if (ms.SubjectLine == storage[i].SubjectLine && ms.MsgId != storage[i].MsgId)
                            {
                                int foundCount = 0;
                                foreach (var v in storage[i].ToAddressList)
                                {
                                    if (ms.ToAddress.Contains(v.ToString()))
                                    {
                                        foundCount++;
                                        break;
                                    }
                                }

                                if (foundCount > 0)
                                {
                                    previousMessagesInThread.Add(ms);
                                }
                            }
                        }

                        found = true;
                        LoadScreen(storage[i], previousMessagesInThread);
                        break;
                    }
                }
            }

            if (!found)
            {
                //MessageBox.Show("No new messages", "No messages found");
                tbxOutput.Text = "No new messages found.";
                processTimer.Start();
                processTimer.Interval = 300000;
            }
        }

        private void dgvPastEmail_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string myValue = dgvPastEmail[e.ColumnIndex, e.RowIndex].Value.ToString();
        }
    }
}
