using System;
using Logging;

namespace LinkedInMiner
{	
	class MainClass
	{	
		private static FileLogger mFileLogger;
		private static Logger mLogger;
		
		public static void Main (string[] args)
		{	
			try
			{	
				InitializeLog();
				
				var crawler = new Crawler(Global.LinkedInURL + "wvmx/profile", mLogger);
				crawler.Post(null);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex);
				mLogger.AddLogMessage(ex);
			}
			finally
			{	
				mLogger.AddLogMessage("Scraping Complete.");
				mFileLogger.Terminate();
			}
		}
		
		private static void InitializeLog()
		{	
			// instantiate the logger
        	mLogger = Logger.Instance;
        
			// instantiate the log observer that will write to disk
       		mFileLogger = new FileLogger(Global.LogFilePath);
        	mFileLogger.Init();
        	
			// Register mFileLogger as a Logger observer.
        	mLogger.RegisterObserver(mFileLogger);
			
			mLogger.AddLogMessage("Begin Scraping...");
		}
	}
}
