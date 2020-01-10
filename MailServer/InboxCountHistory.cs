using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class InboxCountHistory
{
    public DateTime HistoryTime { get; set; }
    public int InboxCount { get; set; }
    public double MessageSendRate { get; set; }
    public bool RateCalculated { get; set; }

    public InboxCountHistory()
    {
        RateCalculated = false;
        MessageSendRate = 0d;
    }
}