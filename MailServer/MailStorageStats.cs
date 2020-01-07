using static ResponseProcessing;

public class MailStorageStats
{
    private EmailType type;
    private int count, threadLength;

    public MailStorageStats()
    {
        type = EmailType.Unknown;
        count = 0;
    }

    public int Count
    {
        get { return count; }
        set { count = value; }
    }
    public int ThreadLength
    {
        get { return threadLength; }
        set { threadLength = value; }
    }
    public EmailType Type
    {
        get { return type; }
        set { type = value; }
    }
}