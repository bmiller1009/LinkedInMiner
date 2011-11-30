using System;
using System.IO;

public class FileLogger: ILogger
{
    private string _folderPath = String.Empty;
	private string _fileName = "Log.txt";
    private StreamWriter _logFile;
	
	public FileLogger(string folderPath)
    {
        _folderPath = folderPath;
		
		if(!Directory.Exists(_folderPath))
			Directory.CreateDirectory(_folderPath);
    }
	
    public void Init()
    {
		var directoryName = @_folderPath + "/" + "DailyScrape_" + DateTime.Now.ToString().Replace('/', '.').Replace(':', '.');
		
		Directory.CreateDirectory(directoryName);
		
		var fileName = @directoryName + "/" + _fileName;
		
		using(var fs = File.Create(fileName))
		{
			fs.Close();
		}
		
        _logFile = new StreamWriter(fileName, true);
    }
	
    public void Terminate()
    {	
        _logFile.Close();
		_logFile.Dispose();
    }
	
    public void ProcessLogMessage(string logMessage)
    {
    	// FileLogger implements the ProcessLogMessage method by
    	// writing the incoming message to a file.
        _logFile.WriteLine(logMessage);
    }
}