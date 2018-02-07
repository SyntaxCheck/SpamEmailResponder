using System;
using System.Linq;
using System.Text;
using System.IO;

public class FileAttachment
{
    private string fileName, parentZipName, contentType, processingMessage;
    private bool containsBinary, fileException, validXML;
    private byte[] fileBytes;

    public byte[] FileBytes
    {
        get { return fileBytes; }
        set { fileBytes = value; }
    }
    public string FileName
    {
        get { return fileName; }
        set { fileName = CleanFileName2(value); }
    }
    public string FileExtension
    {
        get { return fileName.Substring(fileName.LastIndexOf('.')); }
    }
    public string ParentZipName
    {
        get { return parentZipName; }
        set { parentZipName = value; }
    }
    public string ContentType
    {
        get { return contentType; }
        set { contentType = value; }
    }
    public bool FileException
    {
        get { return fileException; }
        set { fileException = value; }
    }
    public string ProcessingMessage
    {
        get { return processingMessage; }
        set { processingMessage = value; }
    }
    public bool ContainsBinary
    {
        get { return containsBinary; }
        set { containsBinary = value; }
    }
    public bool ValidXML
    {
        get { return validXML; }
        set { validXML = value; }
    }

    public FileAttachment()
    {
        fileName = String.Empty;
        parentZipName = String.Empty;
        contentType = String.Empty;
        processingMessage = String.Empty;
        containsBinary = true;
        fileException = false;
        validXML = false;
    }

    /// <summary>
    /// Function to simplify appending to the processing Notes
    /// </summary>
    /// <param name="text"></param>
    /// <param name="newLineOnNew"></param>
    public void AppendProcessingNotes(string text, bool newLineOnNew)
    {
        if (processingMessage == null || processingMessage == "")
        {
            processingMessage = text;
        }
        else
        {
            if (newLineOnNew)
                processingMessage += Environment.NewLine;
            else
                processingMessage += ", ";

            processingMessage += text;
        }
    }

    //Private functions
    public string CleanFileName(string filename)
    {
        StringBuilder builder = new StringBuilder();
        char[] invalid = Path.GetInvalidFileNameChars();
        foreach (var cur in filename)
        {
            if (!invalid.Contains(cur))
            {
                builder.Append(cur);
            }
        }
        return builder.ToString();
    }
    public string CleanFileName2(string filename)
    {
        StringBuilder builder = new StringBuilder();
        char[] valid = " abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-_().".ToCharArray();
        foreach (var cur in filename)
        {
            if (valid.Contains(cur))
            {
                builder.Append(cur);
            }
        }
        return builder.ToString();
    }
}