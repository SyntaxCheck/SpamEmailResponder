public class KnownFileTypes
{
    private string extension, contentType;
    private bool couldContainBinary;

    public KnownFileTypes()
    {
    }

    public string ContentType
    {
        get { return contentType; }
        set { contentType = value; }
    }
    public bool CouldContainBinary
    {
        get { return couldContainBinary; }
        set { couldContainBinary = value; }
    }
    public string Extension
    {
        get { return extension; }
        set { extension = value; }
    }
}