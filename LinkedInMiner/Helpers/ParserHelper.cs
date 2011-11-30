using System;
using System.Text;
using System.Text.RegularExpressions;

namespace LinkedInMiner.Helpers
{
	public class ParserHelper
	{
		private ParserHelper ()
		{
		}
		
		public static string RegexReturnFirstMatch(string html, string pattern)
		{	
			try
			{
				var r = new Regex(pattern);
				
				if(r.Matches(html).Count == 0)
					return String.Empty;
				
		    	return r.Matches(html)[0].Value.Trim();
			}
			catch(IndexOutOfRangeException ex)
			{
				throw new IndexOutOfRangeException("No regex match found at index 0", ex);
			}
		}
		
		public static string GetTextBetweenTags(string tag)
		{	
			CleanData(tag);
			
			var outputName = new StringBuilder();
			
			var posStartTag = tag.IndexOf('>');
			
			foreach(var c in tag.Substring(posStartTag + 1))
			{
				if(c == '<')
					break;
				
				outputName.Append(c);
			}
			
			return CleanData(outputName.ToString());
		}
		
		public static string CleanData(string data)
		{	
			data = data.Trim();
			data = data.Replace("amp;", String.Empty);
			
			return data;
		}
	}
}