using System;
using Logging;
using LinkedInMiner.Helpers;

namespace LinkedInMiner
{	
	/// <summary>
	/// Maps to a single row of data in the database.
	/// </summary>
	internal class EntryRecord
	{	
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="LinkedInMiner.EntryRecord"/> class.
		/// </summary>
		/// <param name='logger'>
		/// Logger.
		/// </param>
		public EntryRecord(Logger logger)
		{
			_logger = logger;
		}
		#endregion
		
		#region Member Variables
		private string _entryName = String.Empty;
		private string _entryJobTitle = String.Empty;
		private string _entryLocation = String.Empty;
		private string _entryRegion = String.Empty;
		
	    private Logger _logger = null;
		#endregion
		
		#region Properties
		public int MainEntryID
		{
			get; private set;
		}
		
		public byte EntryTypeID
		{
			get; set;
		}
		
		public string EntryName
		{
			get{return _entryName.Replace("'", String.Empty);}
			set{_entryName = value;}
		}
		
		public string ProfileURL
		{
			get; set;
		}
		
		public string EntryJobTitle
		{
			get{return _entryJobTitle.Replace("'", String.Empty);}
			set{_entryJobTitle = value;}
		}
		
		public string EntryLocation
		{
			get{return _entryLocation.Replace("'", String.Empty);}
			set{_entryLocation = value;}
		}
		
		public string EntryRegion
		{
			get{return _entryRegion.Replace("'", String.Empty);}
			set{_entryRegion = value;}
		}
		
		public DateTime ScrapeDate
		{
			get; private set;
		}
		
		public int? SemiKnownMainEntryID
		{
			get; set;
		}
		
		public int? SharesGroups
		{
			get; set;
		}
	
		public int? SharesConnections
		{
			get; set;
		}
		#endregion
		
		#region Overrides
		public override string ToString ()
		{
			return string.Format ("[EntryRecord: MainEntryID={0}, EntryTypeID={1}, EntryName={2}, ProfileURL={3}, EntryJobTitle={4}, EntryLocation={5}, EntryRegion={6}, SemiKnownMainEntryID={7}, SharesGroups={8}, SharesConnections={9}]", MainEntryID, EntryTypeID, EntryName, ProfileURL, EntryJobTitle, EntryLocation, EntryRegion, SemiKnownMainEntryID, SharesGroups, SharesConnections);
		}
		#endregion
		
		#region Public Methods
		/// <summary>
		/// Save this instance.
		/// </summary>
		public void Save()
		{   
			try
			{
				string sql = "INSERT INTO main_entry (entry_type_id, entry_name, profile_url, " +
							 "entry_job_title, entry_location, entry_region, semi_known_main_entry_id, " +
							 "shares_groups, shares_connections) " +
							 "VALUES " +
					         "(" +
								  EntryTypeID + "," +
								  DBHelper.SQLFormatString(EntryName, true) +
								  DBHelper.SQLFormatString(ProfileURL, true) +
								  DBHelper.SQLFormatString(EntryJobTitle, true) + 
								  DBHelper.SQLFormatString(EntryLocation, true) + 
								  DBHelper.SQLFormatString(EntryRegion, true) +
								  SemiKnownMainEntryID + "," +
								  SharesGroups + "," +
								  SharesConnections +
							 ")";
			
				_logger.AddLogMessage("Saving Entry Record.  SQL = '" + sql + '"');
				MainEntryID = Data.ExecuteScalar(sql);
				_logger.AddLogMessage("Saving Entry Record Complete.  Main Entry ID ='" + MainEntryID + "'");
			}
			catch(Exception ex)
			{	
				_logger.AddLogMessage(ex);
			
				throw;
			}
		}
		#endregion
	}
}