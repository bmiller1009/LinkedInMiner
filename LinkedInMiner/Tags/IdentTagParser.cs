using System;
using Logging;
using LinkedInMiner.Helpers;

namespace LinkedInMiner.Tags
{
	public class IdentTagParser : TagParser
	{	
		private int? _semiKnownID;
		
		public IdentTagParser (string html, Logger logger, int? semiKnownID) : base(html, logger)
		{
			_semiKnownID = semiKnownID;
		}
		
		public override EntryRecord ParseTag()
		{	
			try
			{	
				if(base.EntryRecord == null)
					throw new NullReferenceException("EntryRecord in base class is null");
				
				base.EntryRecord.EntryTypeID = (byte)TagType.Known;
				base.EntryRecord.EntryName = GetEntryName();
				base.EntryRecord.ProfileURL = GetProfileURL();
				base.EntryRecord.EntryJobTitle = GetEntryJobTitle();
				base.EntryRecord.EntryLocation = GetEntryLocation();
				base.EntryRecord.EntryRegion = GetEntryIndustry();
				base.EntryRecord.SemiKnownMainEntryID = _semiKnownID;
				base.EntryRecord.SharesGroups = GetEntrySharesGroups();
				base.EntryRecord.SharesConnections = GetEntrySharesConnections();
				
				return base.EntryRecord;
			}
			catch(Exception ex)
			{	
				base.Logger.AddLogMessage(ex);
				
				throw;
			}
		}
		
		private string GetEntryName()
		{	
			try
			{	
				var tag = ParserHelper.RegexReturnFirstMatch(this.HTML, " title='View profile'>.*</a>");
				
				return ParserHelper.GetTextBetweenTags(tag);
			}
			catch(Exception ex)
			{	
				base.Logger.AddLogMessage(ex);
				
				throw;
			}
		}
		
		private string GetProfileURL()
		{	
			try
			{
				var urlPrefix = Global.LinkedInURL + "profile/view?id=";
				
				var tag = ParserHelper.RegexReturnFirstMatch(this.HTML, @"<a href='/profile[^>]*>");
				
				var indexOfString = String.Empty;
				var titleString = String.Empty;

				if(tag.Contains("view?id"))
				{
					indexOfString = "id=";
					titleString = "&amp;authType";
				}
				else if(tag.Contains("viewProfile=&amp"))
				{
					indexOfString = "key=";
					titleString = "&amp;authToken";
				}
				
				var offSet = indexOfString.Length;
				
				var startIndex = tag.IndexOf(indexOfString);
				var titleIndex = tag.IndexOf(titleString);
				
				return urlPrefix + ParserHelper.CleanData(tag.Substring(startIndex + offSet, titleIndex - startIndex - offSet));
			}
			catch(Exception ex)
			{	
				base.Logger.AddLogMessage(ex);
				
				throw;
			}
		}
		
		private string GetEntryJobTitle()
		{	
			try
			{	
				var tag = ParserHelper.RegexReturnFirstMatch(this.HTML, "<dd class='title'>.*</dd>");
				
				return ParserHelper.GetTextBetweenTags(tag);
			}
			catch(Exception ex)
			{	
				base.Logger.AddLogMessage(ex);
				
				throw;
			}
		}
		
		private string GetEntryLocation()
		{	
			try
			{
				var tag = ParserHelper.RegexReturnFirstMatch(this.HTML, "<span class='location'>.*</span>");
				
				return ParserHelper.GetTextBetweenTags(tag);
			}
			catch(Exception ex)
			{	
				base.Logger.AddLogMessage(ex);
				
				throw;
			}
		}
		
		private string GetEntryIndustry()
		{	
			try
			{
				var tag = ParserHelper.RegexReturnFirstMatch(this.HTML, "<span class='industry'>.*</span>");
				
				return ParserHelper.GetTextBetweenTags(tag);
			}
			catch(Exception ex)
			{	
				base.Logger.AddLogMessage(ex);
				
				throw;
			}
		}
		
		private int GetEntrySharesGroups()
		{
			return Convert.ToInt32(this.HTML.Contains("<div class='shared-groups'>"));
		}
		
		private int GetEntrySharesConnections()
		{   
			return Convert.ToInt32(this.HTML.Contains("<div class='shared-connections'"));
		}
	}
}