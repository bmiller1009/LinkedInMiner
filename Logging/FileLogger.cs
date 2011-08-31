using System;
using System.IO;
using System.Text;

public class FileLogger: ILogger
{
    private string _folderPath = String.Empty;
	private string _mFileName = "Log.txt";
	
    private StreamWriter _mLogFile;
	
    public string FileName
    {
        get 
        { 
            return _mFileName; 
        }
    }
    
	public FileLogger(string folderPath)
    {
        _folderPath = folderPath;
		
		if(!Directory.Exists(_folderPath))
			Directory.CreateDirectory(_folderPath);
    }
	
    public void Init()
    {
		string directoryName = @_folderPath + "/" + "DailyScrape_" + DateTime.Now.ToString().Replace('/', '.').Replace(':', '.');
		Directory.CreateDirectory(directoryName);
		string fileName = @directoryName + "/" + _mFileName;
		
		//File.Create(fileName);
		using(var fs = File.Create(fileName))
		{
			fs.Close();
		}
		
        _mLogFile = new StreamWriter(fileName, true);
    }
    public void Terminate()
    {	
        _mLogFile.Close();
		_mLogFile.Dispose();
    }
    public void ProcessLogMessage(string logMessage)
    {
    	// FileLogger implements the ProcessLogMessage method by
    	// writing the incoming message to a file.
        _mLogFile.WriteLine(logMessage);
    }
}