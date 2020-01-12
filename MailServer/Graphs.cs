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
    public partial class Graphs : Form
    {
        private List<MailStorage> storage;
        private SerializeHelper serializeHelper;
        private MailServerFunctions mailServer;
        private Thread calculateThread;
        private GraphWorkerThread worker;

        public Graphs()
        {
            InitializeComponent();

            try
            {
                storage = new List<MailStorage>();
                serializeHelper = new SerializeHelper();
                mailServer = new MailServerFunctions();

                LoadStorageObject();
                LoadGraph();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + Environment.NewLine + "Stack: " + ex.StackTrace, "Failed to init screen");
            }
        }

        private void Graphs_Load(object sender, EventArgs e)
        {

        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void rbMessageType_CheckedChanged(object sender, EventArgs e)
        {
            LoadGraph();
        }
        private void rbThreadLength_CheckedChanged(object sender, EventArgs e)
        {
            LoadGraph();
        }
        private void calcTimer_Tick(object sender, EventArgs e)
        {
            CheckThread();
        }
        private void txtThreadLength_TextChanged(object sender, EventArgs e)
        {
            int intOut = 0;
            if (!int.TryParse(txtThreadLength.Text, out intOut))
            {
                txtThreadLength.Text = "50";
            }
        }
        private void cbxPopupMsgIds_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxPopupMsgIds.Checked)
            {
                lblSaveMsgId.Visible = true;
                txtThreadLength.Visible = true;
            }
            else
            {
                lblSaveMsgId.Visible = false;
                txtThreadLength.Visible = false;
            }
        }

        //Helper functions
        private void LoadGraph()
        {
            int intOut = 0;
            if (!int.TryParse(txtThreadLength.Text, out intOut))
            {
                txtThreadLength.Text = "50";
                intOut = 50;
            }

            if (rbMessageType.Checked)
            {
                if (calculateThread != null && calculateThread.ThreadState == ThreadState.Running)
                    calculateThread.Abort();

                worker = new GraphWorkerThread(mailServer, storage, GraphWorkerThread.GraphType.MessageType, intOut);
                calculateThread = new Thread(new ThreadStart(worker.DoWork));
                calculateThread.Start();

                rbMessageType.Enabled = false;
                rbThreadLength.Enabled = false;

                calcTimer.Start();
                CheckThread();
            }
            else if (rbThreadLength.Checked)
            {
                if (calculateThread != null && calculateThread.ThreadState == ThreadState.Running)
                    calculateThread.Abort();

                worker = new GraphWorkerThread(mailServer, storage, GraphWorkerThread.GraphType.ThreadLength, intOut);
                calculateThread = new Thread(new ThreadStart(worker.DoWork));
                calculateThread.Start();

                rbMessageType.Enabled = false;
                rbThreadLength.Enabled = false;

                calcTimer.Start();
                CheckThread();
            }
        }
        private void LoadStorageObject()
        {
            string currentDirectory = Directory.GetCurrentDirectory();

            string fullPath = Path.Combine(currentDirectory, StaticVariables.STORAGE_OBJECT_FILENAME);
            if (File.Exists(fullPath))
            {
                storage = serializeHelper.ReadFromBinaryFile<List<MailStorage>>(fullPath);
            }
        }
        private void CheckThread()
        {
            if (worker.Finished)
            {
                lblCalculating.Visible = false;
                List<MailStorageStats> stats = worker.ReturnStats;

                if (rbMessageType.Checked)
                {
                    //Set graph now that we have the stats compiled
                    chartMain.Series.Clear();
                    chartMain.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                    chartMain.ChartAreas[0].AxisY.MinorGrid.Enabled = false;
                    chartMain.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                    chartMain.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
                    chartMain.ChartAreas[0].RecalculateAxesScale();
                    foreach (MailStorageStats mss in stats)
                    {
                        Series tmpSeries = chartMain.Series.Add(mss.Type.ToString() + " (" + mss.Count.ToString() + ")");
                        tmpSeries.ChartType = SeriesChartType.Column;
                        tmpSeries.MarkerBorderWidth = 100;
                        tmpSeries.Points.AddXY("Message Type", mss.Count);
                        tmpSeries.BorderWidth = 100;
                        tmpSeries["PixelPointWidth"] = "700";
                    }
                }
                else if (rbThreadLength.Checked)
                {
                    //Set graph now that we have the stats compiled
                    chartMain.Series.Clear();
                    chartMain.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                    chartMain.ChartAreas[0].AxisY.MinorGrid.Enabled = false;
                    chartMain.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                    chartMain.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
                    chartMain.ChartAreas[0].RecalculateAxesScale();
                    stats = stats.OrderBy(t => t.ThreadLength).ToList();
                    foreach (MailStorageStats mss in stats)
                    {
                        Series tmpSeries = chartMain.Series.Add(mss.ThreadLength.ToString() + " msgs (" + mss.Count.ToString() + ")");
                        tmpSeries.ChartType = SeriesChartType.Column;
                        tmpSeries.MarkerBorderWidth = 100;
                        tmpSeries.Points.AddXY("Amount at thread length", mss.Count);
                        tmpSeries.BorderWidth = 100;
                        tmpSeries["PixelPointWidth"] = "600";
                    }

                    if (cbxPopupMsgIds.Checked)
                    {
                        PopupText popTxt = new PopupText();
                        popTxt.Show();
                        popTxt.GroupBoxText = "Msg IDs";
                        popTxt.TextToDisplay = worker.MsgIdsForLongest;
                        popTxt.SetValues();
                    }
                }

                calcTimer.Stop();
                rbMessageType.Enabled = true;
                rbThreadLength.Enabled = true;
            }
            else
            {
                lblCalculating.Visible = true;
                lblCalculating.Text = "Please wait, Calculating stats. Progress: " + Math.Round(worker.Progress, 0).ToString() + "%";
            }
        }
    }
}
