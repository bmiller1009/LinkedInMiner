using System;
using Logging;

namespace LinkedInMiner.Tags
{
	public abstract class TagParser
	{	
		protected enum TagType : byte
		{
			Anon = 1,
			Semiknown = 2,
			Known = 3
		}
		
		private string _html = String.Empty;
		private EntryRecord _entryRecord;
		private Logger _logger;
		
		public TagParser(string html, Logger logger)
		{
			_html = html;
			_logger = logger;
			_entryRecord = new EntryRecord(_logger);
		}
		
		public Logger Logger
		{
			get{return _logger;}
		}
		
		public string HTML
		{
			get{return _html;}
		}
		
		public EntryRecord EntryRecord
		{
			get{return _entryRecord;}
		}
		
		public abstract EntryRecord ParseTag();
		
		public override string ToString ()
		{
			return string.Format ("[TagParser: EntryRecord={0}]", EntryRecord);
		}
	}
}