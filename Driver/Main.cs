using System;
using Logging;
using LinkedInMiner;
using Globals;

namespace Driver
{	
	/// <summary>
	/// Sample Driver to class to run the crawler.
	/// </summary>
	class MainClass
	{
		#region Member Variables
		private static FileLogger _fileLogger;
		private static Logger _logger;
		#endregion
		
		#region Main
		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name='args'>
		/// The command-line arguments.
		/// </param>
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
		#endregion
		
		#region Private Methods
		/// <summary>
		/// Initializes the log.
		/// </summary>
		private static void InitializeLog()
		{	
		      	_logger = Logger.Instance;
		     
		     		_fileLogger = new FileLogger(Global.LogFilePath);
		      	_fileLogger.Init();
		      	
		      	_logger.RegisterObserver(_fileLogger);
			
			_logger.AddLogMessage("Begin Scraping...");
		}
		#endregion
	}
}