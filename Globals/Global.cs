using System;
using System.Configuration;

namespace Globals
{	
	/// <summary>
	/// Global class to abstract app.config content
	/// </summary>
	public class Global
	{	
		public static string ConnectionString
		{	
			get{return ConfigurationManager.AppSettings["connectionString"];}
		}
		
		public static string CookiePath
		{
			get{return ConfigurationManager.AppSettings["cookiePath"];}
		}
		
		public static string LinkedInURL
		{
			get{return ConfigurationManager.AppSettings["linkedInURL"];}
		}
		
		public static string LogFilePath
		{
			get{return ConfigurationManager.AppSettings["logFilePath"];}
		}
			
		private Global ()
		{
		}
	}
}