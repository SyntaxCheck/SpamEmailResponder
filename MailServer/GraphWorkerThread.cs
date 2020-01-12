using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ResponseProcessing;

public class GraphWorkerThread
{
    public double Progress { get; set; }
    public bool Finished { get; set; }
    public List<MailStorageStats> ReturnStats;

    private MailServerFunctions mailServer;
    private List<MailStorage> storage;
    private GraphType type;

    public enum GraphType
    {
        MessageType = 1,
        ThreadLength = 2
    }

    public GraphWorkerThread(MailServerFunctions mailServerIn, List<MailStorage> storageIn, GraphType typeIn)
    {
        Finished = false;
        Progress = 0d;
        ReturnStats = new List<MailStorageStats>();
        mailServer = mailServerIn;
        storage = storageIn;
        type = typeIn;
    }

    public void DoWork()
    {
        List<MailStorage> copy = CopyList(storage);
        ReturnStats = new List<MailStorageStats>();
        int count = 0;

        switch (type)
        {
            case GraphType.MessageType:
                foreach (MailStorage ms in storage)
                {
                    if (ms.Ignored) //Dont add ignored messages to stats since it most likely is duplicates
                        continue;

                    bool found = false;
                    for (int i = 0; i < ReturnStats.Count(); i++)
                    {
                        if (ReturnStats[i].Type == (EmailType)ms.MessageType)
                        {
                            found = true;
                            ReturnStats[i].Count++;
                            break;
                        }
                    }
                    if (!found)
                    {
                        MailStorageStats mss = new MailStorageStats();
                        mss.Type = (EmailType)ms.MessageType;
                        mss.Count = 1;
                        ReturnStats.Add(mss);
                    }

                    count++;
                    Progress = (count / (double)storage.Count()) * 100;
                }

                Finished = true;
                Progress = 100d;

                break;
            case GraphType.ThreadLength:
                List<string> skipMsgIds = new List<string>();

                //foreach (MailStorage ms in storage)
                for(int i = storage.Count() - 1; i >= 0; i--)
                {
                    bool found = false;

                    if (storage[i].Ignored) //Dont add ignored messages to stats since it most likely is duplicates
                        continue;
                    if (skipMsgIds.Contains(storage[i].MsgId))
                        continue;

                    //Get Thread Length for current Message
                    List<MailStorage> thread = mailServer.GetPreviousMessagesInThread(copy, storage[i]);
                    int threadCount = thread.Count() + 1;

                    //Add all the MsgIds from the thread list to the skip IDs list
                    foreach (MailStorage tms in thread)
                    {
                        skipMsgIds.Add(tms.MsgId);
                        copy.Remove(tms);
                    }

                    for (int k = 0; k < ReturnStats.Count(); k++)
                    {
                        if (ReturnStats[k].ThreadLength == threadCount)
                        {
                            found = true;
                            ReturnStats[k].Count++;
                            break;
                        }
                    }
                    if (!found)
                    {
                        MailStorageStats mss = new MailStorageStats();
                        mss.ThreadLength = threadCount;
                        mss.Count = 1;
                        ReturnStats.Add(mss);
                    }

                    count++;
                    Progress = (count / (double)storage.Count()) * 100;
                }

                Finished = true;
                Progress = 100d;

                break;
            default:
                Finished = true;
                Progress = 100d;
                break;
        }
    }
    private List<MailStorage> CopyList(List<MailStorage> list)
    {
        List<MailStorage> newList = new List<MailStorage>();

        for (int i = 0; i < list.Count(); i++)
        {
            newList.Add(list[i].DeepCopy());
        }

        return newList;
    }
}