using System;
using Logging;

namespace LinkedInMiner
{
	public class EntryRecord
	{		
			public EntryRecord(Logger logger)
			{
				_logger = logger;
			}
		
			private int _mainEntryID = 0;
			private int _entryTypeID = 0;
			private string _entryName = String.Empty;
			private string _profileURL = String.Empty;
			private string _entryJobTitle = String.Empty;
			private string _entryLocation = String.Empty;
			private string _entryRegion = String.Empty;
			private DateTime _scrapeDate = DateTime.MinValue;
			private int? _semiKnownMainEntryID = null;
			private int? _sharesGroups = null;
			private int? _sharesConnections = null;
			
		    private Logger _logger = null;
		
			public int MainEntryID
			{
				get{return _mainEntryID;}
			}
			
			public int EntryTypeID
			{
				get{return _entryTypeID;}
				set{_entryTypeID = value;}
			}
			
			public string EntryName
			{
				get{return _entryName;}
				set{_entryName = value;}
			}
			
			public string ProfileURL
			{
				get{return _profileURL;}
				set{_profileURL = value;}
			}
			
			public string EntryJobTitle
			{
				get{return _entryJobTitle;}
				set{_entryJobTitle = value;}
			}
			
			public string EntryLocation
			{
				get{return _entryLocation;}
				set{_entryLocation = value;}
			}
			
			public string EntryRegion
			{
				get{return _entryRegion;}
				set{_entryRegion = value;}
			}
			
			private DateTime ScrapeDate
			{
				get{return _scrapeDate;}
			}
			
			public int? SemiKnownMainEntryID
			{
				get{return _semiKnownMainEntryID;}
				set{_semiKnownMainEntryID = value;}
			}
			
			public int? SharesGroups
			{
				get{return _sharesGroups;}
				set{_sharesGroups = value;}
			}
		
			public int? SharesConnections
			{
				get{return _sharesConnections;}
				set{_sharesConnections = value;}
			}
			
			public override string ToString ()
		{
			return string.Format ("[EntryRecord: MainEntryID={0}, EntryTypeID={1}, EntryName={2}, ProfileURL={3}, EntryJobTitle={4}, EntryLocation={5}, EntryRegion={6}, SemiKnownMainEntryID={7}, SharesGroups={8}, SharesConnections={9}]", MainEntryID, EntryTypeID, EntryName, ProfileURL, EntryJobTitle, EntryLocation, EntryRegion, SemiKnownMainEntryID, SharesGroups, SharesConnections);
		}
			
			public void Save()
			{   
				try
				{
					string sql = "INSERT INTO main_entry (entry_type_id, entry_name, profile_url, " +
								 "entry_job_title, entry_location, entry_region, semi_known_main_entry_id, " +
								 "shares_groups, shares_connections) " +
								 "VALUES " +
						         "(" +
								  _entryTypeID + "," +
								  "'" + _entryName.Replace("'", String.Empty) + "'" + "," +
								  "'" + _profileURL + "'" + "," +
								  "'" + _entryJobTitle.Replace("'", String.Empty) + "'" + "," +
								  "'" + _entryLocation.Replace("'", String.Empty) + "'" + "," +
								  "'" + _entryRegion.Replace("'", String.Empty) + "'" + "," +
								  "'" + _semiKnownMainEntryID + "'," +
								  "'" + _sharesGroups + "'," +
								  "'" + _sharesConnections + "'" +
								 ")";
				
					var data = new Data();
					_logger.AddLogMessage("Saving Entry Record.  SQL = '" + sql + '"');
					_mainEntryID = data.ExecuteScalar(sql);
					_logger.AddLogMessage("Saving Entry Record Complete.  Main Entry ID ='" + _mainEntryID +"'");
				}
				catch(Exception ex)
				{	
					_logger.AddLogMessage(ex);
				
					throw;
				}
			}
		}
}