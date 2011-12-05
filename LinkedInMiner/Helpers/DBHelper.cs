using System;

namespace LinkedInMiner.Helpers
{	
	/// <summary>
	/// Utility class for common database functionality
	/// </summary>
	internal class DBHelper
	{
		private DBHelper ()
		{
		}
		
		#region Public Methods
		/// <summary>
		/// Formats a string so it can be used in a dynamic sql statement.
		/// This function will return NULL if the string is null or an empty length string.
		/// If it is not null or empty then it will replace single quote with two single quotes and encase the string in single quotes.
		/// </summary>
		/// <returns>
		/// The formatted string.
		/// </returns>
		/// <param name='s'>
		/// string parameter
		/// </param>
		/// <param name='includeComma'>
		/// Include comma.
		/// </param>
		public static string SQLFormatString(string s, bool includeComma)
		{	
			var comma = (includeComma) ? "," : String.Empty;
			
			if (String.IsNullOrEmpty(s))
				return " NULL" + comma;
			else		
				return "'" + s.Replace("'", "''").TrimEnd() + "'" + comma;
		}
		#endregion
	}
}