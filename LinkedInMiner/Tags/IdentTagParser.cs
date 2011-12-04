using System;
using Logging;
using LinkedInMiner.Helpers;
using Globals;

namespace LinkedInMiner.Tags
{	
	/// <summary>
	/// This class handles parsing logic for fully identified contacts
	/// </summary>
	/// <exception cref='NullReferenceException'>
	/// Is thrown when there is an attempt to dereference a null object reference.
	/// </exception>
	internal class IdentTagParser : TagParser
	{	
		#region Member Variables
		private int? _semiKnownID;
		#endregion
		
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="LinkedInMiner.Tags.IdentTagParser"/> class.
		/// </summary>
		/// <param name='html'>
		/// Matched Html.
		/// </param>
		/// <param name='logger'>
		/// Logger.
		/// </param>
		/// <param name='semiKnownID'>
		/// Semi-known ID
		/// </param>
		public IdentTagParser (string html, Logger logger, int? semiKnownID) : base(html, logger)
		{
			_semiKnownID = semiKnownID;
		}
		#endregion
		
		#region Overrides
		/// <summary>
		/// Parses the tag.
		/// </summary>
		/// <returns>
		/// Returns an entry record for the database to which the html has been mapped.
		/// </returns>
		/// <exception cref='NullReferenceException'>
		/// Is thrown when there is an attempt to dereference a null object reference.
		/// </exception>
		protected override EntryRecord ParseTag()
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
		#endregion
		
		#region Private Methods
		/// <summary>
		/// Gets the name of the entry.
		/// </summary>
		/// <returns>
		/// The entry name.
		/// </returns>
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
		
		/// <summary>
		/// Gets the profile URL.
		/// </summary>
		/// <returns>
		/// The profile URL.
		/// </returns>
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
		
		/// <summary>
		/// Gets the entry job title.
		/// </summary>
		/// <returns>
		/// The entry job title.
		/// </returns>
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
		
		/// <summary>
		/// Gets the entry location.
		/// </summary>
		/// <returns>
		/// The entry location.
		/// </returns>
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
		
		/// <summary>
		/// Gets the entry industry.
		/// </summary>
		/// <returns>
		/// The entry industry.
		/// </returns>
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
		
		/// <summary>
		/// Gets the entry shares groups.
		/// </summary>
		/// <returns>
		/// The entry shares groups.
		/// </returns>
		private int GetEntrySharesGroups()
		{
			return Convert.ToInt32(this.HTML.Contains("<div class='shared-groups'>"));
		}
		
		/// <summary>
		/// Gets the entry shares connections.
		/// </summary>
		/// <returns>
		/// The entry shares connections.
		/// </returns>
		private int GetEntrySharesConnections()
		{   
			return Convert.ToInt32(this.HTML.Contains("<div class='shared-connections'"));
		}
		#endregion
	}
}