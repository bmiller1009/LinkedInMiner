using System;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Logging;
using LinkedInMiner.Tags;
using Globals;

namespace LinkedInMiner
{
	public class Crawler
	{	
		private string _cookie = String.Empty;
		private Logger _logger;
		
		public Crawler(Logger logger)
		{
			_cookie = File.ReadAllText(Global.CookiePath);
			_logger = logger;
		}
		
		public void Post(int? semiKnownID, string url) 
		{
       	    try
			{	
				_logger.AddLogMessage("Beginning HTTP Post.  URL='"+ url + "'");
				
				var pagedata = String.Empty;
				var request = HttpWebRequest.Create(url) as HttpWebRequest;
				
				if (request == null)
					throw new Exception("Invalid Http Request.");
				
				//set the user agent so it looks like IE to not raise suspicion 
				request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)";
				request.Method = "GET";
				//set the cookie in the request header
				request.Headers.Add("Cookie", _cookie);
	
				//get the response from the server
				using(var response = (HttpWebResponse)request.GetResponse())
				{
					using (var stream = response.GetResponseStream())
					{
				    	using (var reader = new StreamReader(stream))
				    	{
				        	pagedata = reader.ReadToEnd();
							reader.Close();
				    	}
					
						stream.Close();
					}
				
					response.Close();
				}
				
				_logger.AddLogMessage("HTTP Post complete.");
				
				GetContacts(pagedata.Replace("\"", "'"), semiKnownID);
			}
			catch(Exception ex)
			{	
				_logger.AddLogMessage(ex);
				
				throw;
			}
        }
		
		private void GetContacts(string html, int? semiKnownID)
		{	
			try
			{	
				_logger.AddLogMessage("Extracting contacts from captured html...");
	
				var r = new Regex(@"<li id='vcard-recently-[0-9]' class='vcard[^>]*>\s*(.*\n)*?\s*</li>");
				
				foreach(Match m in r.Matches(html))	
					DetermineParse(m.Value, semiKnownID);
				
				_logger.AddLogMessage("Extraction of contacts from html is complete.");
			}
			catch(Exception ex)
			{	
				_logger.AddLogMessage(ex);
				
				throw;
			}
		}
		
		private void DetermineParse(string htmlMatch, int? semiKnownID)
		{	
			_logger.AddLogMessage("Parsing contact...");
			
			try
			{
				var tp = TagFactory.GetTagParser(htmlMatch, _logger, semiKnownID);
				tp.ParseTag();
				tp.EntryRecord.Save();
				
				var semiKnownTagParser = tp as SemiKnownTagParser;
				
				if(semiKnownTagParser != null)
					Post(tp.EntryRecord.MainEntryID, semiKnownTagParser.SemiKnownURL);
				
				Console.WriteLine(tp);
			}
			catch(Exception ex)
			{	
				_logger.AddLogMessage(ex);
				
				throw;
			}
			
			_logger.AddLogMessage("Parsing contact complete...");
		}
	}
}