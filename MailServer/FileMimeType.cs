using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.IO;

public class FileMimeType
{
    private readonly LoggerInfo loggerInfo;
    private List<KnownFileTypes> acceptedMimeTypes, specialExceptionMimeTypes;

    [DllImport("urlmon.dll", CharSet = CharSet.Auto)]
    private static extern UInt32 FindMimeFromData(
        UInt32 pBC, [MarshalAs(UnmanagedType.LPStr)]
        string pwzUrl, [MarshalAs(UnmanagedType.LPArray)]
        byte[] pBuffer, UInt32 cbSize, [MarshalAs(UnmanagedType.LPStr)]
        string pwzMimeProposed, UInt32 dwMimeFlags, ref UInt32 ppwzMimeOut, UInt32 dwReserverd
    );

    public FileMimeType(LoggerInfo loggerInfo)
    {
        this.loggerInfo = loggerInfo;
        if (acceptedMimeTypes == null || specialExceptionMimeTypes == null)
            InitializeDictionary();
    }

    /// <summary>
    /// Get the content type passing in path to file, it will open the file and call the overloaded method to check for the content type
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public string GetContentType(string filename)
    {
        try
        {
            FileStream fs = new FileStream(filename, FileMode.Open);
            string rtn = GetContentType(fs, filename);
            fs.Close();
            return rtn;
        }
        catch (Exception ex)
        {
            Logger.WriteDbg(loggerInfo, "Failed to get content type for file: " + filename + ". Message: " + ex.Message + ". Stack Trace: " + ex.StackTrace);
            return "";
        }
    }
    /// <summary>
    /// Function to return the content type, pass it a stream (could be filestream or memorystream). ScanFileForMimeType checks the beginning bytes of the file
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="filename"></param>
    /// <returns></returns>
    public string GetContentType(Stream stream, string filename)
    {
        string headerType = String.Empty;

        if (acceptedMimeTypes == null || specialExceptionMimeTypes == null)
            InitializeDictionary();

        try
        {
            headerType = ScanFileForMimeType(stream);
        }
        catch (Exception ex)
        {
            headerType = "";
            Logger.WriteDbg(loggerInfo, "Failed to scan file for mime type: " + filename + ". Message: " + ex.Message + ". Stack Trace: " + ex.StackTrace);
        }

        return headerType;
    }
    /// <summary>
    /// Function has logic to determine which files to accept and which files to reject, it will also return the reason why the file was accepted/rejected
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="contentType"></param>
    /// <param name="containsBinary"></param>
    /// <param name="reason"></param>
    /// <param name="fileIsException"></param>
    /// <returns></returns>
    public bool AcceptFile(string filename, string contentType, bool containsBinary, ref string reason, ref bool fileIsException)
    {
        string extension = String.Empty;
        int acceptedCount = 0, exceptionCount = 0;
        bool acceptFile = false;
        bool exception = false;

        if (acceptedMimeTypes == null || specialExceptionMimeTypes == null)
            InitializeDictionary();

        if (filename.Contains('.'))
            extension = filename.Substring(filename.LastIndexOf('.') + 1);
        else
            extension = "";

        acceptedCount =
          (from n in acceptedMimeTypes
          where n.Extension.ToUpper().Equals(extension.ToUpper())
          select acceptedMimeTypes).Count();

        exceptionCount =
            (from n in specialExceptionMimeTypes
             where n.Extension.ToUpper().Equals(extension.ToUpper())
             select specialExceptionMimeTypes).Count();

        if (!String.IsNullOrEmpty(extension))
        {
            if (acceptedCount > 0 || exceptionCount > 0)
            {
                //Extension Passed, Now check that the extension matches the documented Content Type
                acceptedCount = 0;
                exceptionCount = 0;

                acceptedCount =
                    (from n in acceptedMimeTypes
                     where (n.Extension.ToUpper().Equals(extension.ToUpper()) && n.ContentType.ToUpper().Equals(contentType.ToUpper()))
                     select acceptedMimeTypes).Count();

                exceptionCount =
                    (from n in specialExceptionMimeTypes
                     where (n.Extension.ToUpper().Equals(extension.ToUpper()) && n.ContentType.ToUpper().Equals(contentType.ToUpper()))
                     select specialExceptionMimeTypes).Count();

                if (acceptedCount > 0 || exceptionCount > 0)
                {
                    //File matches one of the known types list
                    KnownFileTypes type;
                    type = acceptedMimeTypes.FirstOrDefault(s => s.Extension.ToUpper().Equals(extension.ToUpper()));
                    if (type == null)
                    {
                        type = specialExceptionMimeTypes.FirstOrDefault(s => s.Extension.ToUpper().Equals(extension.ToUpper()));
                        exception = true;
                    }

                    if (type != null)
                    {
                        if (!containsBinary)
                        {
                            reason = "File extension matches known types and file does not contain binary data";
                            acceptFile = true;
                        }
                        else if (type.CouldContainBinary == containsBinary)
                        {
                            reason = "File extension matches know types and file contains binary as expected";
                            acceptFile = true;
                        }
                        else
                        {
                            reason = "File extension matches know types and file contains binary but binary is not expected";
                            acceptFile = false;
                        }
                    }
                    else
                    {
                        reason = "File matched known types but file extension is not accepted"; //Should never hit this else, changed the text a little just so we could differentiate
                        acceptFile = false;
                        exception = false;
                    }
                }
                else
                {
                    KnownFileTypes type;
                    type = acceptedMimeTypes.FirstOrDefault(s=>s.Extension.ToUpper().Equals(extension.ToUpper()));
                    if (type == null)
                    {
                        type = specialExceptionMimeTypes.FirstOrDefault(s => s.Extension.ToUpper().Equals(extension.ToUpper()));
                        exception = true;
                    }
                    else
                    {
                        exception = false;
                    }

                    if (type != null)
                    {
                        if (exception)
                        {
                            reason = "File extension does not match content type";
                            acceptFile = true;
                        }
                        else
                        {
                            if (!containsBinary)
                            {
                                reason = "File extension does not match content type but file does not contain binary data";
                                acceptFile = true;
                            }
                            else
                            {
                                reason = "File extension does not match content type and file contains binary data";
                                acceptFile = false;
                            }
                        }
                    }
                    else
                    {
                        reason = "File extension is not accepted"; //Should never hit this else, changed the text a little just so we could differentiate
                        acceptFile = false;
                        exception = false;
                    }
                }
            }
            else
            {
                if (!containsBinary)
                {
                    reason = "Unknown File extension but file does not contain binary data";
                    acceptFile = true;
                }
                else
                {
                    reason = "Unknown File extension and file contains binary data";
                    acceptFile = false;
                }
            }
        }
        else
        {
            reason = "File must have an extension";
            acceptFile = false;
        }

        fileIsException = exception;

        return acceptFile;
    }
    /// <summary>
    /// Typical ASCII text documents would contain no bytes = 0. Exe/image/video/dll etc typically contain a byte 0. Some plain text documents in other encodings other than ASCII
    /// Could contain every other byte = 0, so to accomodate that we search for double 0 and this does yield some pdf files as NON-binary, but so far I have yet to find a pds
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public bool ContainsBinary(byte[] file)
    {
        bool containsZeroByte = false;

        //Skip last character as some documents are allowed to end with a NUL in ASCII which is a Zero byte
        for(int i=0;i<(file.Length - 1);i++)
        {
            //Not 100% but a large number of binary files contain consecutive 0 bytes. PDF files known this will not detect PDF 100%
            if (file[i] == 0 && file[i + 1] == 0)
            {
                containsZeroByte = true;
                break;
            }
        }

        return containsZeroByte;
    }
    /// <summary>
    /// Pass the path to the file and the function will open the file saving the bytes to a byte array for the overloaded function to scan
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public bool ContainsBinary(string filename)
    {        
        byte[] bytes;
        int count, sum = 0, len;

        FileStream fs = new FileStream(filename, FileMode.Open);
        len = (int)fs.Length;
        bytes = new byte[len];

        while ((count = fs.Read(bytes, sum, len - sum)) > 0)
            sum += count;

        fs.Close();
        return ContainsBinary(bytes.ToArray());
    }
    /// <summary>
    /// Check the first 256 bytes of file for the content type using urlmon
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    private string ScanFileForMimeType(Stream stream)
    {
        byte[] buffer = new byte[256];
        stream.Position = 0;
        int readLength = Convert.ToInt32(Math.Min(256, stream.Length));
        stream.Read(buffer, 0, readLength);
        stream.Position = 0;

        UInt32 mimeType = default(UInt32);
        FindMimeFromData(0, null, buffer, 256, null, 0, ref mimeType, 0);
        IntPtr mimeTypePtr = new IntPtr(mimeType);
        string mime = Marshal.PtrToStringUni(mimeTypePtr);
        Marshal.FreeCoTaskMem(mimeTypePtr);
        return mime;
    }
    /// <summary>
    /// Function to init the list of accepted extension/content types and the list of extensions/content types we accept no matter what
    /// </summary>
    private void InitializeDictionary()
    {
        //http://www.freeformatter.com/mime-types-list.html
        //http://filext.com/
        //Other references from google to build the list
        acceptedMimeTypes = new List<KnownFileTypes>();
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "7z", ContentType = "application/x-7z-compressed", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "bmp", ContentType = "image/bmp", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "bmp", ContentType = "image/x-bmp", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "bmp", ContentType = "image/x-bitmap", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "bmp", ContentType = "image/x-xbitmap", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "bmp", ContentType = "image/x-win-bitmap", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "bmp", ContentType = "image/x-windows-bmp", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "bmp", ContentType = "image/ms-bmp", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "bmp", ContentType = "image/x-ms-bmp", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "bmp", ContentType = "application/bmp", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "bmp", ContentType = "application/x-bmp", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "bmp", ContentType = "application/x-win-bitmap ", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "csv", ContentType = "text/csv", CouldContainBinary = false });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "csv", ContentType = "text/comma-separated-values", CouldContainBinary = false });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "csv", ContentType = "application/csv", CouldContainBinary = false });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "csv", ContentType = "application/excel", CouldContainBinary = false });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "csv", ContentType = "text/anytext", CouldContainBinary = false });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "csv", ContentType = "application/vnd.msexcel", CouldContainBinary = false });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "csv", ContentType = "application/vnd.ms-excel", CouldContainBinary = false });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "gif", ContentType = "image/gif", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "gif", ContentType = "image/x-xbitmap", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "ico", ContentType = "image/ico", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "ico", ContentType = "image/x-icon", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "ico", ContentType = "application/ico", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "ico", ContentType = "application/x-ico", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "ico", ContentType = "application/x-win-bitmap", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "ico", ContentType = "image/x-win-bitmap", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "jpe", ContentType = "image/jpeg", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "jpeg", ContentType = "image/jpeg", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "jpeg", ContentType = "image/jpg", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "jpeg", ContentType = "image/pjpeg", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "jpg", ContentType = "image/jpeg", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "jpg", ContentType = "image/jpg", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "jpg", ContentType = "image/jp_", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "jpg", ContentType = "application/jpg", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "jpg", ContentType = "application/x-jpg", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "jpg", ContentType = "image/pjpeg", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "jpg", ContentType = "image/pipeg", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "jpg", ContentType = "image/vnd.swiftview-jpeg", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "jpg", ContentType = "image/x-xbitmap", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "jfif", ContentType = "image/pipeg", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "mer", ContentType = "application/octet-stream", CouldContainBinary = false });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "pjpeg", ContentType = "image/pjpeg", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "png", ContentType = "image/png", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "png", ContentType = "application/png", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "png", ContentType = "application/x-png", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "rtf", ContentType = "text/richtext", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "rtf", ContentType = "application/rtf", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "rtf", ContentType = "application/x-rtf", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "rtf", ContentType = "text/rtf", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "tif", ContentType = "image/tif", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "tif", ContentType = "image/x-tif", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "tif", ContentType = "image/tiff", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "tif", ContentType = "image/x-tiff", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "tif", ContentType = "application/tif", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "tif", ContentType = "application/x-tif", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "tif", ContentType = "application/tiff", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "tif", ContentType = "application/x-tiff", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "tiff", ContentType = "image/tiff", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "txt", ContentType = "text/plain", CouldContainBinary = false });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "txt", ContentType = "application/txt", CouldContainBinary = false });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "txt", ContentType = "text/anytext", CouldContainBinary = false });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "txt", ContentType = "widetext/plain", CouldContainBinary = false });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "txt", ContentType = "widetext/paragraph", CouldContainBinary = false });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "xml", ContentType = "application/xml", CouldContainBinary = false });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "xml", ContentType = "application/x-xml", CouldContainBinary = false });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "xml", ContentType = "text/xml", CouldContainBinary = false });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "zip", ContentType = "application/zip", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "zip", ContentType = "application/x-zip", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "zip", ContentType = "application/x-zip-compressed", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "zip", ContentType = "application/x-compress", CouldContainBinary = true });
        acceptedMimeTypes.Add(new KnownFileTypes { Extension = "zip", ContentType = "application/x-compressed", CouldContainBinary = true });

        //List of accepted application types. These should yeild a warning to the user as it is difficult to detect hidden binary
        specialExceptionMimeTypes = new List<KnownFileTypes>();
        specialExceptionMimeTypes.Add(new KnownFileTypes { Extension = "doc", ContentType = "application/msword", CouldContainBinary = true });
        specialExceptionMimeTypes.Add(new KnownFileTypes { Extension = "doc", ContentType = "application/doc", CouldContainBinary = true });
        specialExceptionMimeTypes.Add(new KnownFileTypes { Extension = "doc", ContentType = "appl/text", CouldContainBinary = true });
        specialExceptionMimeTypes.Add(new KnownFileTypes { Extension = "doc", ContentType = "application/vnd.msword", CouldContainBinary = true });
        specialExceptionMimeTypes.Add(new KnownFileTypes { Extension = "doc", ContentType = "application/vnd.ms-word", CouldContainBinary = true });
        specialExceptionMimeTypes.Add(new KnownFileTypes { Extension = "doc", ContentType = "application/winword", CouldContainBinary = true });
        specialExceptionMimeTypes.Add(new KnownFileTypes { Extension = "doc", ContentType = "application/word", CouldContainBinary = true });
        specialExceptionMimeTypes.Add(new KnownFileTypes { Extension = "doc", ContentType = "application/x-msw6", CouldContainBinary = true });
        specialExceptionMimeTypes.Add(new KnownFileTypes { Extension = "doc", ContentType = "application/x-msword", CouldContainBinary = true });
        specialExceptionMimeTypes.Add(new KnownFileTypes { Extension = "docx", ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document", CouldContainBinary = true });
        specialExceptionMimeTypes.Add(new KnownFileTypes { Extension = "docx", ContentType = "application/x-zip-compressed", CouldContainBinary = true });
        specialExceptionMimeTypes.Add(new KnownFileTypes { Extension = "pdf", ContentType = "application/pdf", CouldContainBinary = true });
        specialExceptionMimeTypes.Add(new KnownFileTypes { Extension = "pdf", ContentType = "application/x-pdf", CouldContainBinary = true });
        specialExceptionMimeTypes.Add(new KnownFileTypes { Extension = "pdf", ContentType = "application/acrobat", CouldContainBinary = true });
        specialExceptionMimeTypes.Add(new KnownFileTypes { Extension = "pdf", ContentType = "applications/vnd.pdf", CouldContainBinary = true });
        specialExceptionMimeTypes.Add(new KnownFileTypes { Extension = "pdf", ContentType = "text/pdf", CouldContainBinary = true });
        specialExceptionMimeTypes.Add(new KnownFileTypes { Extension = "pdf", ContentType = "text/x-pdf", CouldContainBinary = true });
        specialExceptionMimeTypes.Add(new KnownFileTypes { Extension = "rtf", ContentType = "application/msword", CouldContainBinary = true });
        specialExceptionMimeTypes.Add(new KnownFileTypes { Extension = "rtf", ContentType = "application/doc", CouldContainBinary = true });
        specialExceptionMimeTypes.Add(new KnownFileTypes { Extension = "xls", ContentType = "application/vnd.ms-excel", CouldContainBinary = true });
        specialExceptionMimeTypes.Add(new KnownFileTypes { Extension = "xls", ContentType = "application/msexcel", CouldContainBinary = true });
        specialExceptionMimeTypes.Add(new KnownFileTypes { Extension = "xls", ContentType = "application/x-msexcel", CouldContainBinary = true });
        specialExceptionMimeTypes.Add(new KnownFileTypes { Extension = "xls", ContentType = "application/x-ms-excel", CouldContainBinary = true });
        specialExceptionMimeTypes.Add(new KnownFileTypes { Extension = "xls", ContentType = "application/x-excel", CouldContainBinary = true });
        specialExceptionMimeTypes.Add(new KnownFileTypes { Extension = "xls", ContentType = "application/x-dos_ms_excel", CouldContainBinary = true });
        specialExceptionMimeTypes.Add(new KnownFileTypes { Extension = "xls", ContentType = "application/application/xls", CouldContainBinary = true });
        specialExceptionMimeTypes.Add(new KnownFileTypes { Extension = "xlsx", ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", CouldContainBinary = true });
        specialExceptionMimeTypes.Add(new KnownFileTypes { Extension = "xlsx", ContentType = "application/x-zip-compressed", CouldContainBinary = true });
    }
}