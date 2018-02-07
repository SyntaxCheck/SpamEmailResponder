public class LoggerInfo
{
    /*******************************************************************************
        * This is a primitive Class that communicates thru its public property accessors
        * rather than thru the common StandardResponse object.
        * 
        * Classes that instantiate this object should:
        * (1) set the RootPath and FolderName properties (and optionally FileName), 
        * (2) call the .ValidatePath() method, 
        * (3) finally check the "LogError" property (boolean) to verify that the path 
        *     and folder provided were valid.  If ErrorFlag=true, the LogErrorMsg property (string) 
        *     will contain an error message.
        * 
        * Optional property "DebugActive" (boolean, default=false) may be set to control 
        * whether uses of method .writeDbg() will be honored or ignored.  Setting this
        * property eliminates the need to condition the use of the .WriteDbgMsgs() method
        * in all other classes that use Logger.
        * *****************************************************************************/
    private string rootPath;
    private string folderName;
    private string fileName;
    private bool debugActive;
    private bool errorFlag;
    private string errorMsg;
    private bool fullPathChecked;

    // CONSTRUCTOR
    public LoggerInfo()
    {
        rootPath = "";
        folderName = "";
        fileName = "log.txt";
        debugActive = false;
        errorFlag = true;          // set True until we checkpath or make first attempt to write to log
        errorMsg = "no errors";

        fullPathChecked = false;
    }

    // ACCESSORS
    public string RootPath
    {
        get { return rootPath; }
        set { rootPath = value; }
    }
    public string FolderName
    {
        get { return folderName; }
        set { folderName = value; }
    }
    public string FileName
    {
        get { return fileName; }
        set { fileName = value; }
    }
    public bool DebugActive // defaults False if not set
    {
        get { return debugActive; }
        set { debugActive = value; }
    }
    public bool ErrorFlag
    {
        get { return errorFlag; }
        set { errorFlag = value; }
    }
    public string ErrorMsg
    {
        get { return errorMsg; }
        set { errorMsg = value; }
    }
    public string FullPath // readOnly
    {
        get
        {
            var path = System.IO.Path.Combine(rootPath, folderName);
            return System.IO.Path.Combine(path, fileName);
        }
    }
    public bool FullPathChecked
    {
        get { return fullPathChecked; }
        set { fullPathChecked = value; }
    }
}