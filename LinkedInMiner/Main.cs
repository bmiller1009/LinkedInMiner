using System;
using Logging;

namespace LinkedInMiner
{	
	class MainClass
	{	
		private static FileLogger _fileLogger;
		private static Logger _logger;
		
		public static void Main (string[] args)
		{	
			try
			{	
				var mainURL = Global.LinkedInURL + "wvmx/profile";
				
				InitializeLog();
				
				var crawler = new Crawler(_logger);
				crawler.Post(null, mainURL);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex);
				_logger.AddLogMessage(ex);
			}
			finally
			{	
				_logger.AddLogMessage("Scraping Complete.");
				_fileLogger.Terminate();
			}
		}
		
		private static void InitializeLog()
		{	
        	_logger = Logger.Instance;
       
       		_fileLogger = new FileLogger(Global.LogFilePath);
        	_fileLogger.Init();
        	
        	_logger.RegisterObserver(_fileLogger);
			
			_logger.AddLogMessage("Begin Scraping...");
		}
	}
}