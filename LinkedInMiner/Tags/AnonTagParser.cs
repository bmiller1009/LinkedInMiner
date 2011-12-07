using System;
using Logging;
using LinkedInMiner.Data;

namespace LinkedInMiner.Tags
{	
	/// <summary>
	/// This class handles parsing logic for anonymous contacts
	/// </summary>
	/// <exception cref='NullReferenceException'>
	/// Is thrown when there is an attempt to dereference a null object reference.
	/// </exception>
	internal class AnonTagParser : TagParser
	{	
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="LinkedInMiner.Tags.AnonTagParser"/> class.
		/// </summary>
		/// <param name='html'>
		/// Match html.
		/// </param>
		/// <param name='logger'>
		/// Logger.
		/// </param>
		public AnonTagParser (string html, Logger logger) : base(html, logger)
		{
			
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
				
				base.EntryRecord.EntryTypeID = (byte)TagType.Anon;
				base.EntryRecord.EntryName = "Anonymous LinkedIn User";
				base.EntryRecord.ProfileURL = String.Empty;
				base.EntryRecord.EntryJobTitle = String.Empty;
				base.EntryRecord.EntryLocation = String.Empty;
				base.EntryRecord.EntryRegion = String.Empty;
				base.EntryRecord.SemiKnownMainEntryID = null;
				
				return base.EntryRecord;
			}
			catch(Exception ex)
			{
				Logger.AddLogMessage(ex);
				
				throw;
			}
		}
		#endregion
	}
}