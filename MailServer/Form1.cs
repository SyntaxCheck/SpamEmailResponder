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
        private int countdownRemaining;

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
                else
                {
                    this.Text = "Mail Server - " + mailServer.MyName;
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
                //processTimer.Start();
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
            if ((storage.Count(t => !t.Replied) - skippedCount) <= 10)
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
                Logger.Write(loggerInfo, "Skipping the check for messages. Unreplied count: " + storage.Count(t => !t.Replied).ToString() + ", Skip count: " + skippedCount.ToString());
            }
            //response = mailServer.GetMessages(loggerInfo, ref storage);
            //if (response.Code < 0)
            //{
            //    tbxOutput.Text = response.AsString();
            //    Logger.Write(loggerInfo, "Process Timer/MailServer.GetMessage()", response);
            //}

            int postCount = storage.Count();
            if (preCount == postCount)
            {
                Logger.Write(loggerInfo, "Pre and post counts match. Pre: " + preCount.ToString() + ", Post: " + postCount.ToString());
            }

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
                    processTimer.Stop();
                    MessageBox.Show(response.AsString(), "Failed to get new mail");
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

            //Start the visual countdown on screen
            countdownRemaining = trckBar.Value;
            if (!countdownTimer.Enabled)
            {
                countdownTimer.Start();
            }
        }
        private void countdownTimer_Tick(object sender, EventArgs e)
        {
            countdownRemaining = countdownRemaining - 1;
            lblTimeTillNextSend.Text = countdownRemaining.ToString();

            if (countdownRemaining <= 0)
            {
                countdownTimer.Stop();
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
                    storage[i].Ignored = true;
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
                lblCountdown.Visible = true;
                panel1.Visible = true;
                lblNext.Visible = true;
                lblTimeTillNextSend.Visible = true;
                lblSendFreq.Visible = true;
                lblTrackBarValue.Visible = true;
                trckBar.Visible = true;

                countdownRemaining = trckBar.Value;
                if (!countdownTimer.Enabled)
                {
                    countdownTimer.Start();
                }
            }
            else
            {
                processTimer.Stop();
                lblCountdown.Visible = false;
                panel1.Visible = false;
                lblNext.Visible = false;
                lblTimeTillNextSend.Visible = false;
                lblSendFreq.Visible = false;
                lblTrackBarValue.Visible = false;
                trckBar.Visible = false;
            }
        }
        private void cbxDebug_CheckedChanged(object sender, EventArgs e)
        {
            loggerInfo.DebugActive = cbxDebug.Checked;
        }
        private void trckBar_Scroll(object sender, EventArgs e)
        {
            processTimer.Interval = trckBar.Value * 1000;
            lblTrackBarValue.Text = "(" + trckBar.Value.ToString() + ") seconds";
            mailServer.InboxCountHistory = new List<int>(); //If they adjust the send interval we need to clear the inbox count since we do not track the amount of time between each InboxCount
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void storageViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StorageViewer sv = new StorageViewer();
            sv.Show();
        }

        //Private functions
        private int CheckForMessages()
        {
            StandardResponse response = new StandardResponse() { Code = 0, Message = String.Empty, Data = String.Empty };
            int rtn = 0;

            //response = mailServer.GetMessages(loggerInfo, ref storage);
            //if (response.Code < 0)
            //{
            //    tbxOutput.Text = response.AsString();
            //}

            //Check for new messages
            if ((storage.Count(t => !t.Replied) - skippedCount) <= 10)
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

                    rtn = LoadMessage();
                }
            }

            SetMessageCountLabel();

            return rtn;
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
            if (((EmailType)ms.MessageType) == EmailType.Unknown)
            {
                tbxOutput.Text = "Unknown email type. Please add code to handle this type of email or ignore it.";
            }
            if (((EmailType)ms.MessageType) != EmailType.Unknown || !String.IsNullOrEmpty(tbxDeterminedReply.Text))
            {
                btnSendEmail.Enabled = true;
            }

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
            double hoursBetweenReceivingAndSending = mailServer.GetHoursBetweenSending();

            ClearScreen();

            for (int i = 0; i < storage.Count(); i++)
            {
                if (!storage[i].Replied && !storage[i].Ignored && !skippedMessages.Contains(";;;" + storage[i].MsgId + ";;;"))
                {
                    //Make sure the age of the message has surpassed the setting
                    double hours = (DateTime.Now - storage[i].DateReceived).TotalHours;
                    if (hours > hoursBetweenReceivingAndSending)
                    {
                        List<MailStorage> previousMessagesInThread = mailServer.GetPreviousMessagesInThread(storage, storage[i]);

                        found = true;
                        LoadScreen(storage[i], previousMessagesInThread);
                        if (String.IsNullOrEmpty(tbxDeterminedReply.Text))
                        {
                            Regen();
                        }
                        break;
                    }
                    else
                    {
                        Logger.WriteDbg(loggerInfo, "Skipping new message. Received: " + storage[i].DateReceived.ToString() + ". Message is only " + hours.ToString() + " old.");
                    }
                }
            }

            if (!found)
            {
                tbxOutput.Text = "No new messages found.";
                Logger.Write(loggerInfo, "No new messages found");

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
                    if (!storage[i].DeterminedReply.Contains("|Get") || !storage[i].DeterminedReply.Contains("System.Collections") || !storage[i].DeterminedReply.Contains("[System.String]"))
                    {
                        string newMsgId = String.Empty;
                        response = mailServer.SendSMTP(loggerInfo, storage[i].ToAddress, storage[i].SubjectLine, storage[i].DeterminedReply, storage[i].IncludeID, storage[i].MsgId, ref newMsgId);
                        if (response.Code >= 0)
                        {
                            storage[i].MyReplyMsgId = newMsgId;
                            storage[i].Replied = true;
                            workingOnMsg = String.Empty;
                            CheckForMessages();
                        }
                        else
                        {
                            tbxOutput.Text = response.AsString();

                            return response.Code;
                        }
                    }
                    else
                    {
                        Logger.Write(loggerInfo, "Bad reply replace: " + storage[i].DeterminedReply);
                        MessageBox.Show("Bad reply replace: " + storage[i].DeterminedReply);
                        processTimer.Stop();
                        cbxAutoSend.Checked = false;
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
                    List<MailStorage> previousMessagesInThread = mailServer.GetPreviousMessagesInThread(storage, storage[i]);
                    MailStorage temp = storage[i];
                    string newReply = mailServer.GetResponseForType(loggerInfo, ref temp, previousMessagesInThread);

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
        private void SetMessageCountLabel()
        {
            if (mailServer.InboxCountHistory.Count() > 0)
            {
                
                int avg = 0;
                int sum = 0;
                int diffSum = 0;
                double diffAvg = 0;

                if (mailServer.InboxCountHistory.Count() > 1)
                {
                    for (int i = 0; i < mailServer.InboxCountHistory.Count(); i++)
                    {
                        sum += mailServer.InboxCountHistory[i];
                    }

                    avg = sum / mailServer.InboxCountHistory.Count();

                    for (int i = mailServer.InboxCountHistory.Count() - 1; i >= 0; i--)
                    {
                        //Remove any 0 counts reported when we have a large average size, these were probably the result of server errors incorrectly reporting the count
                        //Also cleanup any counts that are old so that Old data does not keep affecting current rates
                        if (mailServer.InboxCountHistory[i] == 0 && avg > 50 || Math.Abs(mailServer.InboxCountHistory[i] - (avg * 0.5)) > (avg * 0.5))
                        {
                            mailServer.InboxCountHistory.RemoveAt(i);
                        }
                    }

                    for (int i = 0; i < mailServer.InboxCountHistory.Count() - 1; i++)
                    {
                        diffSum += Math.Abs(mailServer.InboxCountHistory[i] - mailServer.InboxCountHistory[i + 1]);
                    }

                    diffAvg = (double)diffSum / mailServer.InboxCountHistory.Count() - 1;
                }

                TimeSpan ts = new TimeSpan();
                int totalTimeNeeded = 0;
                int secondsBetweenSends = trckBar.Value;

                if (mailServer.InboxCountHistory.Count() > 1)
                {
                    totalTimeNeeded = (int)Math.Round((mailServer.LastInboxCount * secondsBetweenSends) / diffAvg, 0);
                    ts = TimeSpan.FromSeconds(totalTimeNeeded);
                }

                //When the calculated time is negative or we do not have enough data yet
                if (ts == null || ts == TimeSpan.Zero || totalTimeNeeded == 0)
                {
                    totalTimeNeeded = mailServer.LastInboxCount * secondsBetweenSends;
                    ts = TimeSpan.FromSeconds(totalTimeNeeded);
                }

                lblCountdown.Text = "Estimated Time till Mailbox Cleared: ";
                if (ts.Days > 0)
                {
                    if(ts.Days > 1)
                        lblCountdown.Text += ts.Days.ToString() + " days ";
                    else
                        lblCountdown.Text += ts.Days.ToString() + " day ";
                }
                if (ts.Days > 0 || ts.Hours > 0)
                {
                    if (ts.Hours > 1)
                        lblCountdown.Text += ts.Hours.ToString() + " hours ";
                    else
                        lblCountdown.Text += ts.Hours.ToString() + " hour ";
                }
                if (ts.Days > 0 || ts.Hours > 0 || ts.Minutes > 0)
                {
                    if (ts.Minutes > 1)
                        lblCountdown.Text += ts.Minutes.ToString() + " minutes ";
                    else
                        lblCountdown.Text += ts.Minutes.ToString() + " minute ";
                }
            }

            lblMessageInfo.Text = "Sent: " + storage.Count(t => t.Replied).ToString("#,##0") + "   Skipped: " + skippedCount.ToString("#,##0") + "   Ignored: " + storage.Count(t => t.Ignored).ToString("#,##0") + "   Pending: " + storage.Count(t => !t.Replied).ToString("#,##0") + "   Unread: " + mailServer.LastInboxCount.ToString("#,##0");
        }
    }
}
