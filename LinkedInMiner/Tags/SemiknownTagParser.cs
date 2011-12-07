using System;
using Logging;
using LinkedInMiner.Helpers;
using LinkedInMiner.Data;
using Globals;

namespace LinkedInMiner.Tags
{
	/// <summary>
	/// This class handles parsing logic for semi-known contacts
	/// </summary>
	/// <exception cref='NullReferenceException'>
	/// Is thrown when there is an attempt to dereference a null object reference.
	/// </exception>
	internal class SemiKnownTagParser : TagParser
	{	
		private const string SEMI_REGEX_PATTERN = "<a href='/wvmx/profile/redherring?[^>]*>.*</a>";
		
		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="LinkedInMiner.Tags.SemiKnownTagParser"/> class.
		/// </summary>
		/// <param name='html'>
		/// Matched html
		/// </param>
		/// <param name='logger'>
		/// Logger.
		/// </param>
		public SemiKnownTagParser (string html, Logger logger) : base(html, logger)
		{
			
		}
		#endregion
		
		#region Properties
		public int MainEntryID
		{
			get{return base.EntryRecord.MainEntryID;}
		}
		
		public string SemiKnownURL
		{
			get{return base.EntryRecord.ProfileURL;}
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
		#endregion
		
		#region Private Methods
		/// <summary>
		/// Parses the semi-known description from a contact in the html list
		/// </summary>
		/// <returns>
		/// The semi-known description.
		/// </returns>
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
		
		/// <summary>
		/// Gets the semi known URL.
		/// </summary>
		/// <returns>
		/// The semi known URL.
		/// </returns>
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
		#endregion
    }
}