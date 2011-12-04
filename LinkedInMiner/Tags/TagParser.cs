using System;
using Logging;

namespace LinkedInMiner.Tags
{	
	/// <summary>
	/// Base parser class html retrieved
	/// </summary>
	internal abstract class TagParser
	{	
		#region Abstract Methods
		/// <summary>
		/// Parses the html in the tag
		/// </summary>
		/// <returns>
		/// Returns an entry record for the database to which the html has been mapped.
		/// </returns>
		protected abstract EntryRecord ParseTag();
		#endregion
		
		#region Enums
		protected enum TagType : byte
		{
			Anon = 1,
			Semiknown = 2,
			Known = 3
		}
		#endregion
		
		#region Member Variables
		private string _html = String.Empty;
		private EntryRecord _entryRecord;
		private Logger _logger;
		#endregion
		
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="LinkedInMiner.Tags.TagParser"/> class.
		/// </summary>
		/// <param name='html'>
		/// Matched html to be parsed.
		/// </param>
		/// <param name='logger'>
		/// Logger object
		/// </param>
		public TagParser(string html, Logger logger)
		{
			_html = html;
			_logger = logger;
			_entryRecord = new EntryRecord(_logger);
		}
		#endregion
		
		#region Properties
		public Logger Logger
		{
			get{return _logger;}
		}
		
		public string HTML
		{
			get{return _html;}
		}
		
		protected EntryRecord EntryRecord
		{
			get{return _entryRecord;}
		}
		#endregion
		
		#region Public Methods
		/// <summary>
		/// Persists the parsed html data to the database/
		/// </summary>
		public void SaveTagContents()
		{
			var _entryRecord = ParseTag();
			
			_entryRecord.Save();
		}
		#endregion
		
		#region Overrides
		public override string ToString ()
		{
			return string.Format ("[TagParser: EntryRecord={0}]", EntryRecord);
		}
		#endregion
	}
}