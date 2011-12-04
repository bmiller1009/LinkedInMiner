using System;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Logging;
using LinkedInMiner.Tags;
using Globals;

namespace LinkedInMiner
{	
	/// <summary>
	/// Core engine of the application.  Takes in url, makes the request, parses and persists the html in the response.
	/// </summary>
	/// <exception cref='ArgumentNullException'>
	/// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
	/// </exception>
	/// <exception cref='Exception'>
	/// Represents errors that occur during application execution.
	/// </exception>
	public class Crawler
	{	
		private string _cookie = String.Empty;
		private Logger _logger;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="LinkedInMiner.Crawler"/> class.
		/// </summary>
		/// <param name='logger'>
		/// Global Logger class for recording all actions within a HTML crawl.
		/// </param>
		/// <exception cref='ArgumentNullException'>
		/// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
		/// </exception>
		public Crawler(Logger logger)
		{	
			if(logger == null)
				throw new ArgumentNullException("logger", "Logger object cannot be null");
			
			_cookie = File.ReadAllText(Global.CookiePath);
			_logger = logger;
		}
		
		/// <summary>
		/// HTTP request/response which pulls the html for a specific user list on the "Who's Viewed Your Profile" section of the LinkedIn homepage.  
		/// </summary>
		/// <param name='semiKnownID'>
		/// This represents the primary key of a semi-known viewer of your profile.  It will only be populated if the viewer is semi-known, otherwise
		/// it will be null
		/// </param>
		/// <param name='url'>
		/// The URL which is being requested
		/// </param>
		/// <exception cref='Exception'>
		/// Represents errors that occur during application execution.
		/// </exception>
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
		
		/// <summary>
		/// Loops through all of the contacts found in the requested html text
		/// </summary>
		/// <param name='html'>
		/// Requested html
		/// </param>
		/// <param name='semiKnownID'>
		/// This represents the primary key of a semi-known viewer of your profile.  It will only be populated if the viewer is semi-known, otherwise
		/// it will be null
		/// </param>
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
		
		/// <summary>
		/// Determines the contact type (known, semi-known or anonymous) then parses and persists the captured data
		/// </summary>
		/// <param name='htmlMatch'>
		/// Requested html
		/// </param>
		/// <param name='semiKnownID'>
		/// This represents the primary key of a semi-known viewer of your profile.  It will only be populated if the viewer is semi-known, otherwise
		/// it will be null
		/// </param>
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