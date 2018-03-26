using static MailServerFunctions;

public class MailStorageStats
{
    private EmailType type;
    private int count;

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
    public EmailType Type
    {
        get { return type; }
        set { type = value; }
    }
}