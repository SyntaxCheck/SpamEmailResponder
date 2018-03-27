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
using System.Windows.Forms.DataVisualization.Charting;
using static MailServerFunctions;

namespace MailServer
{
    public partial class Graphs : Form
    {
        private List<MailStorage> storage;
        private SerializeHelper serializeHelper;
        private MailServerFunctions mailServer;

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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Graphs_Load(object sender, EventArgs e)
        {

        }
        private void rbMessageType_CheckedChanged(object sender, EventArgs e)
        {
            LoadGraph();
        }

        private void rbThreadLength_CheckedChanged(object sender, EventArgs e)
        {
            LoadGraph();
        }

        //Helper functions
        private void LoadGraph()
        {
            if (rbMessageType.Checked)
            {
                List<MailStorageStats> stats = new List<MailStorageStats>();
                foreach (MailStorage ms in storage)
                {
                    if (ms.Ignored) //Dont add ignored messages to stats since it most likely is duplicates
                        continue;

                    bool found = false;
                    for (int i = 0; i < stats.Count(); i++)
                    {
                        if (stats[i].Type == (EmailType)ms.MessageType)
                        {
                            found = true;
                            stats[i].Count++;
                            break;
                        }
                    }
                    if (!found)
                    {
                        MailStorageStats mss = new MailStorageStats();
                        mss.Type = (EmailType)ms.MessageType;
                        mss.Count = 1;
                        stats.Add(mss);
                    }
                }

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
                List<MailStorageStats> stats = new List<MailStorageStats>();
                List<MailStorage> tempStorage = CopyList(storage);
                List<string> skipMsgIds = new List<string>();

                foreach (MailStorage ms in tempStorage)
                {
                    bool found = false;

                    if (ms.Ignored) //Dont add ignored messages to stats since it most likely is duplicates
                        continue;
                    if (skipMsgIds.Contains(ms.MsgId))
                        continue;

                    //Get Thread Length for current Message
                    List<MailStorage> thread = mailServer.GetPreviousMessagesInThread(tempStorage, ms);
                    int threadCount = thread.Count() + 1;

                    //Add all the MsgIds from the thread list to the skip IDs list
                    foreach (MailStorage tms in thread)
                    {
                        skipMsgIds.Add(tms.MsgId);
                    }

                    for (int i = 0; i < stats.Count(); i++)
                    {
                        if (stats[i].ThreadLength == threadCount)
                        {
                            found = true;
                            stats[i].Count++;
                            break;
                        }
                    }
                    if (!found)
                    {
                        MailStorageStats mss = new MailStorageStats();
                        mss.ThreadLength = threadCount;
                        mss.Count = 1;
                        stats.Add(mss);
                    }
                }

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
        private List<MailStorage> CopyList(List<MailStorage> list)
        {
            List<MailStorage> newList = new List<MailStorage>();

            for (int i = 0; i < list.Count(); i++)
            {
                newList.Add(list[i]);
            }

            return newList;
        }
    }
}
