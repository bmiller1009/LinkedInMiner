using System;
using System.Text;
using System.Text.RegularExpressions;
using Logging;

namespace LinkedInMiner
{
	public class AnonTagParser : TagParser
	{	
		public AnonTagParser (string html, Logger logger) : base(html, logger)
		{
			
		}
		
		public override EntryRecord ParseTag()
		{	
			if(base.EntryRecord == null)
				throw new NullReferenceException("EntryRecord in base class is null");
			
			base.EntryRecord.EntryTypeID = 1;
			base.EntryRecord.EntryName = "Anonymous LinkedIn User";
			base.EntryRecord.ProfileURL = String.Empty;
			base.EntryRecord.EntryJobTitle = String.Empty;
			base.EntryRecord.EntryLocation = String.Empty;
			base.EntryRecord.EntryRegion = String.Empty;
			base.EntryRecord.SemiKnownMainEntryID = null;
			
			return base.EntryRecord;
		}
	}
	
	public class SemiKnownTagParser : TagParser
	{	
		public SemiKnownTagParser (string html, Logger logger) : base(html, logger)
		{
			
		}
		
		public override EntryRecord ParseTag()
		{	
			try
			{	
				if(base.EntryRecord == null)
					throw new NullReferenceException("EntryRecord in base class is null");
				
				base.EntryRecord.EntryTypeID = 2;
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
				string semiKnownPattern = "<a href='/wvmx/profile/redherring?[^>]*>.*</a>";
				
				string tag = base.RegexReturnFirstMatch(semiKnownPattern);
				
				return GetTextBetweenTags(base.CleanData(tag));
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
				string urlPrefix = Global.LinkedInURL;
				
				string semiKnownPattern = "<a href='/wvmx/profile/redherring?[^>]*>.*</a>";
				
				string tag = base.RegexReturnFirstMatch(semiKnownPattern);
				
				int startIndex = tag.IndexOf("href='/");
				int titleIndex = tag.IndexOf("trk=");
				
				return urlPrefix + base.CleanData(tag.Substring(startIndex + 7,titleIndex - startIndex - 7));
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
				base.EntryRecord.EntryTypeID = 3;
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
				string entryNamePattern = " title='View profile'>.*</a>";
				
				string tag = base.RegexReturnFirstMatch(entryNamePattern);
				
				return GetTextBetweenTags(base.CleanData(tag));
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
				string urlPrefix = Global.LinkedInURL + "profile/view?id=";
				string profileURLPattern = @"<a href='/profile[^>]*>";
				
				string tag = base.RegexReturnFirstMatch(profileURLPattern);
				
				string indexOfString = String.Empty;
				string titleString = String.Empty;
				
				int offSet = 0;
				
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
				
				offSet = indexOfString.Length;
				
				int startIndex = tag.IndexOf(indexOfString);
				int titleIndex = tag.IndexOf(titleString);
				
				return urlPrefix + base.CleanData(tag.Substring(startIndex + offSet, titleIndex - startIndex - offSet));
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
				string jobTitlePattern = "<dd class='title'>.*</dd>";
				
				string tag = base.RegexReturnFirstMatch(jobTitlePattern);
				
				return base.GetTextBetweenTags(tag);
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
				string jobTitlePattern = "<span class='location'>.*</span>";
				
				string tag = base.RegexReturnFirstMatch(jobTitlePattern);
				
				return base.GetTextBetweenTags(tag);
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
				string jobTitlePattern = "<span class='industry'>.*</span>";
				
				string tag = base.RegexReturnFirstMatch(jobTitlePattern);
				
				return base.GetTextBetweenTags(tag);
			}
			catch(Exception ex)
			{	
				base.Logger.AddLogMessage(ex);
				
				throw;
			}
		}
		
		private int GetEntrySharesGroups()
		{
			return Convert.ToInt32(base.HTML.Contains("<div class='shared-groups'>"));
		}
		
		private int GetEntrySharesConnections()
		{   
			return Convert.ToInt32(base.HTML.Contains("<div class='shared-connections'"));
		}
	}
}

