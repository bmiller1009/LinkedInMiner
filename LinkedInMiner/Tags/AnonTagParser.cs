using System;
using Logging;

namespace LinkedInMiner.Tags
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
			
			base.EntryRecord.EntryTypeID = (byte)TagType.Anon;
			base.EntryRecord.EntryName = "Anonymous LinkedIn User";
			base.EntryRecord.ProfileURL = String.Empty;
			base.EntryRecord.EntryJobTitle = String.Empty;
			base.EntryRecord.EntryLocation = String.Empty;
			base.EntryRecord.EntryRegion = String.Empty;
			base.EntryRecord.SemiKnownMainEntryID = null;
			
			return base.EntryRecord;
		}
	}
}