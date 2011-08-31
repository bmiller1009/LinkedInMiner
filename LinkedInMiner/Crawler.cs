using System;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Logging;

namespace LinkedInMiner
{
	class Crawler
	{	
		private string _url = String.Empty;
		private string _cookie = String.Empty;
		private Logger _logger;
		
		public Crawler(string url, Logger logger)
		{
			_url = url;
			_cookie = File.ReadAllText(Global.CookiePath);
			_logger = logger;
		}
		
		public string URL
		{
			get{return _url;}
			set{_url = value;}
		}

		public void Post(int? semiKnownID) 
		{
       	    try
			{	
				_logger.AddLogMessage("Beginning HTTP Post.  URL='"+ _url + "'");
				
				//grab cookie authentication token from Firebug/Fiddler and add in here
				string pagedata = String.Empty;
				
				var request = HttpWebRequest.Create(_url) as HttpWebRequest;
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
				
				string exp = @"<li id='vcard-recently-[0-9]' class='vcard[^>]*>\s*(.*\n)*?\s*</li>";
	
				var r = new Regex(exp);
				
				foreach(Match m in r.Matches(html))	
					DetermineParse(m.Value.Trim(), semiKnownID);
				
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
				TagParser tp;
				
				if(htmlMatch.Contains("Anonymous LinkedIn User"))
				{
					tp = new AnonTagParser(htmlMatch, _logger);
					tp.ParseTag();
					tp.EntryRecord.Save();
				}
				else if(htmlMatch.Contains("redherring"))
				{
					tp = new SemiKnownTagParser(htmlMatch, _logger);
					tp.ParseTag();
					tp.EntryRecord.Save();
					
					_url = ((SemiKnownTagParser)tp).SemiKnownURL;
					
					Post(tp.EntryRecord.MainEntryID);
				}
				else
				{
					tp = new IdentTagParser(htmlMatch, _logger, semiKnownID);
					tp.ParseTag();
					tp.EntryRecord.Save();
				}
				
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

