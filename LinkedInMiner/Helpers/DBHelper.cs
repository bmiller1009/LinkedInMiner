using System;

namespace LinkedInMiner.Helpers
{
	internal class DBHelper
	{
		private DBHelper ()
		{
		}

		public static string SQLFormatString(string s, bool includeComma)
		{
			if (String.IsNullOrEmpty(s))
				return " NULL ";
			else
			{	
				var comma = (includeComma) ? "," : String.Empty;
				return "'" + s.Replace("'", "''").TrimEnd() + "'" + comma;
			}
		}
	}
}