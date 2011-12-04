using System;
using System.IO;

/// <summary>
/// File logger.
/// </summary>
public class FileLogger: ILogger
{	
	#region Member Variables
    private string _folderPath = String.Empty;
	private string _fileName = "Log.txt";
    private StreamWriter _logFile;
	#endregion
	
	#region Constructors
	/// <summary>
	/// Initializes a new instance of the <see cref="FileLogger"/> class.
	/// </summary>
	/// <param name='folderPath'>
	/// Folder path.
	/// </param>
	public FileLogger(string folderPath)
	{
		_folderPath = folderPath;
	
		if(!Directory.Exists(_folderPath))
			Directory.CreateDirectory(_folderPath);
	}
	#endregion
	
	#region Public Methods
	/// <summary>
	/// Initialize this instance.
	/// </summary>
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
	
	/// <summary>
	/// Terminate this instance.
	/// </summary>
	public void Terminate()
	{	
		_logFile.Close();
		_logFile.Dispose();
	}
	
	/// <summary>
	/// Processes the log message.
	/// </summary>
	/// <param name='logMessage'>
	/// Log message.
	/// </param>
	public void ProcessLogMessage(string logMessage)
	{
		// FileLogger implements the ProcessLogMessage method by
		// writing the incoming message to a file.
		_logFile.WriteLine(logMessage);
	}
	#endregion
}