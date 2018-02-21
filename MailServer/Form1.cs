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
        private const int SEND_INTERVAL = 240000; //Gmail has a limit of 500 emails per 24 hour period. Set the max output well above that. At 3 minutes we would have a max of 480 per day.
        private const string STORAGE_OBJECT_FILENAME = "MailStorage.store";
        private string currentDirectory;
        private LoggerInfo loggerInfo;
        private List<MailStorage> storage;
        private SerializeHelper serializeHelper;
        private MailServerFunctions mailServer;
        private string workingOnMsg;
        private string skippedMessages;
        private int skippedCount;

        public Form1()
        {
            try
            {
                storage = new List<MailStorage>();
                mailServer = new MailServerFunctions();
                serializeHelper = new SerializeHelper();
                currentDirectory = Directory.GetCurrentDirectory();
                workingOnMsg = String.Empty;
                skippedMessages = String.Empty;

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

                string validate = mailServer.SettingsFileValidate();
                if (!String.IsNullOrEmpty(validate))
                {
                    MessageBox.Show("There was an error with your settings", validate);
                    this.Close();
                }

                //Load in the storage serialized class
                string fullPath = Path.Combine(currentDirectory, STORAGE_OBJECT_FILENAME);
                if (File.Exists(fullPath))
                {
                    storage = serializeHelper.ReadFromBinaryFile<List<MailStorage>>(fullPath);
                }

                skippedCount = 0;

                CheckForMessages();

                processTimer.Interval = SEND_INTERVAL;
                processTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Message: " + ex.Message, "Exception on launch");
            }
        }

        private void processTimer_Tick(object sender, EventArgs e)
        {
            StandardResponse response = new StandardResponse() { Code = 0, Message = String.Empty, Data = String.Empty };
            processTimer.Interval = SEND_INTERVAL;

            int preCount = storage.Count();
            response = mailServer.GetMessages(loggerInfo, ref storage);
            int postCount = storage.Count();

            //Write Storage object to disk
            if (storage.Count() > 0)
            {
                string fullPath = Path.Combine(currentDirectory, STORAGE_OBJECT_FILENAME);
                serializeHelper.WriteToBinaryFile(fullPath, storage, false);
            }

            if (cbxAutoSend.Checked)
            {
                if (response.Code < 0)
                {
                    tbxOutput.Text = response.AsString();
                    MessageBox.Show(response.AsString(), "Failed to get new mail");
                    processTimer.Stop();
                }
                else
                {
                    bool foundMessageToSend = false;
                    //Load the message onto the screen
                    int rtn = LoadMessage();

                    if (rtn > 0)
                    {
                        while (!foundMessageToSend)
                        {
                            if (String.IsNullOrEmpty(tbxDeterminedReply.Text.Trim()))
                            {
                                //If the reply is blank try to regen before we skip as more code is added to handle new types of emails
                                Regen();
                            }

                            if (String.IsNullOrEmpty(tbxDeterminedReply.Text.Trim()) || String.IsNullOrEmpty(tbxFromAddress.Text.Trim()))
                            {
                                skippedMessages += ";;;" + workingOnMsg + ";;;";
                                skippedCount++;

                                rtn = CheckForMessages();
                                if (rtn <= 0) //End the loop if there are no more messages or we encountered an error
                                    break;
                            }
                            else
                            {
                                foundMessageToSend = true;
                            }
                        }

                        if (foundMessageToSend)
                        {
                            rtn = SendEmail();
                            if (rtn < 0) //If the email fails to send add to the temp skip list for someone to deal with later
                            {
                                skippedMessages += ";;;" + workingOnMsg + ";;;";
                            }
                        }
                    }
                }
            }
            else
            {
                if (response.Code < 0)
                {
                    tbxOutput.Text = response.AsString();
                }
                else if (postCount > preCount)
                {
                    processTimer.Stop();
                    LoadMessage();
                }
            }
        }
        private void btnSendEmail_Click(object sender, EventArgs e)
        {
            SendEmail();
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            btnSendEmail.Enabled = false;
            btnNext.Enabled = false;
            btnIgnore.Enabled = false;
            btnRegenerate.Enabled = false;

            skippedMessages += ";;;" + workingOnMsg + ";;;";
            skippedCount++;
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
            Regen();
        }
        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            //Save data from screen to object
            SaveData();

            btnSendEmail.Enabled = true;
            btnNext.Enabled = true;
            btnIgnore.Enabled = true;
            btnRegenerate.Enabled = true;
        }
        private void dgvPastEmail_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string myValue = dgvPastEmail[e.ColumnIndex, e.RowIndex].Value.ToString();
        }
        private void cbxAutoSend_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxAutoSend.Checked)
            {
                processTimer.Start();
            }
            else
            {
                processTimer.Stop();
            }
        }

        //Private functions
        private int CheckForMessages()
        {
            StandardResponse response = new StandardResponse() { Code = 0, Message = String.Empty, Data = String.Empty };

            //Check for new messages
            if ((storage.Count(t => !t.Replied) - skippedCount) <= 0)
            {
                response = mailServer.GetMessages(loggerInfo, ref storage);
                if (response.Code < 0)
                {
                    tbxOutput.Text = response.AsString();
                }
            }
            else
            {
                response = new StandardResponse() { Code = 0 };
            }

            //Write Storage object to disk
            if (storage.Count() > 0)
            {
                if (response.Code >= 0)
                {
                    //Always write the storage object here. When the program is auto sending we want to write after the send even if we didnt retrieve new messages
                    string fullPath = Path.Combine(currentDirectory, STORAGE_OBJECT_FILENAME);
                    serializeHelper.WriteToBinaryFile(fullPath, storage, false);

                    int rtn = LoadMessage();
                    return rtn;
                }
            }

            return 0;
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
                        string fullPath = Path.Combine(currentDirectory, STORAGE_OBJECT_FILENAME);
                        serializeHelper.WriteToBinaryFile(fullPath, storage, false);
                    }
                    break;
                }
            }
        }
        private int LoadMessage()
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

            if (!found || cbxAutoSend.Checked)
            {
                processTimer.Start();
                processTimer.Interval = SEND_INTERVAL;
            }

            if (!found)
            {
                //MessageBox.Show("No new messages", "No messages found");
                tbxOutput.Text = "No new messages found.";

                return 0;
            }
            else
            {
                return 1;
            }
        }
        private int SendEmail()
        {
            StandardResponse response = new StandardResponse() { Code = 0, Message = String.Empty, Data = String.Empty };

            if (String.IsNullOrWhiteSpace(tbxFromAddress.Text))
            {
                MessageBox.Show("Must have to To Address to send email.", "Cannot send email");
                return -1;
            }

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
                    response = mailServer.SendSMTP(loggerInfo, storage[i].ToAddress, storage[i].SubjectLine, storage[i].DeterminedReply, storage[i].IncludeID);
                    if (response.Code >= 0)
                    {
                        storage[i].Replied = true;
                        workingOnMsg = String.Empty;
                        CheckForMessages();
                    }
                    else
                    {
                        tbxOutput.Text = response.AsString();

                        return response.Code;
                    }
                    break;
                }
            }

            return 0;
        }
        private void Regen()
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
                        string fullPath = Path.Combine(currentDirectory, STORAGE_OBJECT_FILENAME);
                        serializeHelper.WriteToBinaryFile(fullPath, storage, false);
                    }

                    break;
                }
            }
        }
    }
}
