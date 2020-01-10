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

    public InboxCountHistory()
    {
        MessageSendRate = 0d;
    }
}