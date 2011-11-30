using System;
using Logging;
using LinkedInMiner.Helpers;

namespace LinkedInMiner.Tags
{
	public class SemiKnownTagParser : TagParser
	{	
		private const string SEMI_REGEX_PATTERN = "<a href='/wvmx/profile/redherring?[^>]*>.*</a>";
		
		public SemiKnownTagParser (string html, Logger logger) : base(html, logger)
		{
			
		}
		
		public override EntryRecord ParseTag()
		{	
			try
			{	
				if(base.EntryRecord == null)
					throw new NullReferenceException("EntryRecord in base class is null");
				
				base.EntryRecord.EntryTypeID = (byte)TagType.Semiknown;
				base.EntryRecord.EntryName = GetSemiKnownDescription();
				base.EntryRecord.ProfileURL = GetSemiKnownURL();
				base.EntryRecord.EntryJobTitle = String.Empty;
				base.EntryRecord.EntryLocation = String.Empty;
				base.EntryRecord.EntryRegion = String.Empty;
				base.EntryRecord.SemiKnownMainEntryID = null;
				
				return base.EntryRecord;
			}
			catch(Exception ex)
			{	
				base.Logger.AddLogMessage(ex);
				
				throw;
			}
		}
		
		private string GetSemiKnownDescription()
		{	
			try
			{	
				var tag = ParserHelper.RegexReturnFirstMatch(this.HTML, SEMI_REGEX_PATTERN);
				
				return ParserHelper.GetTextBetweenTags(tag);
			}
			catch(Exception ex)
			{	
				base.Logger.AddLogMessage(ex);
				
				throw;
			}
		}
		
		private string GetSemiKnownURL()
		{	
			try
			{	
				var tag = ParserHelper.RegexReturnFirstMatch(this.HTML, SEMI_REGEX_PATTERN);
				
				var startIndex = tag.IndexOf("href='/");
				var titleIndex = tag.IndexOf("trk=");
				
				return Global.LinkedInURL + ParserHelper.CleanData(tag.Substring(startIndex + 7,titleIndex - startIndex - 7));
			}
			catch(Exception ex)
			{	
				base.Logger.AddLogMessage(ex);
				
				throw;
			}
		}
		
		public string SemiKnownURL
		{
			get{return base.EntryRecord.ProfileURL;}
		}
    }
}