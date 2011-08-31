using System;
using System.Text;
using System.Text.RegularExpressions;
using Logging;

namespace LinkedInMiner
{
	public abstract class TagParser
	{	
		private string _html = String.Empty;
		private EntryRecord _entryRecord;
		private Logger _logger;
		
		private TagParser(){}
		
		public TagParser(string html, Logger logger) : this()
		{
			_html = html;
			_logger = logger;
			_entryRecord = new EntryRecord(_logger);
		}
		
		public Logger Logger
		{
			get{return _logger;}
		}
		
		public string HTML
		{
			get{return _html;}
		}
		
		public EntryRecord EntryRecord
		{
			get{return _entryRecord;}
		}
		
		public string GetTextBetweenTags(string tag)
		{
			var outputName = new StringBuilder();
			
			int posStartTag = tag.IndexOf('>');
			
			foreach(char c in tag.Substring(posStartTag + 1))
			{
				if(c == '<')
					break;
				
				outputName.Append(c);
			}
			
			return CleanData(outputName.ToString());
		}
		
		public string CleanData(string data)
		{
			data = data.Replace("amp;", String.Empty);
			
			return data;
		}
		
		public string RegexReturnFirstMatch(string pattern)
		{	
			try
			{
				var r = new Regex(pattern);
				
				if(r.Matches(this.HTML).Count == 0)
					return String.Empty;
				
		    	return r.Matches(this.HTML)[0].Value.Trim();
			}
			catch(IndexOutOfRangeException ex)
			{
				throw new IndexOutOfRangeException("No regex match found at index 0", ex);
			}
		}
		
		public abstract EntryRecord ParseTag();
		
		public override string ToString ()
		{
			return string.Format ("[TagParser: EntryRecord={0}]", EntryRecord);
		}
	}
}

