using System;
using System.Text;
using System.Text.RegularExpressions;

namespace LinkedInMiner.Helpers
{	
	/// <summary>
	/// Utility class for matching and parsing HTML strings using regular expressions
	/// </summary>
	/// <exception cref='IndexOutOfRangeException'>
	/// Is thrown when an attempt is made to access an element of an array with an index that is outside the bounds of the array.
	/// </exception>
	internal class ParserHelper
	{
		private ParserHelper ()
		{
		}
		
		#region Public Methods
		/// <summary>
		/// returns the first match of a regular expression
		/// </summary>
		/// <returns>
		/// The first match.
		/// </returns>
		/// <param name='html'>
		/// Html to match on
		/// </param>
		/// <param name='pattern'>
		/// Regular expression string
		/// </param>
		/// <exception cref='IndexOutOfRangeException'>
		/// Is thrown when an attempt is made to access an element of an array with an index that is outside the bounds of the array.
		/// </exception>
		public static string RegexReturnFirstMatch(string html, string pattern)
		{	
			try
			{
				var r = new Regex(pattern);
				
				if(r.Matches(html).Count == 0)
					return String.Empty;
				
		    	return r.Matches(html)[0].Value.Trim();
			}
			catch(IndexOutOfRangeException ex)
			{
				throw new IndexOutOfRangeException("No regex match found at index 0", ex);
			}
		}
		
		/// <summary>
		/// Gets the text between tags.
		/// </summary>
		/// <returns>
		/// The text between tags.
		/// </returns>
		/// <param name='tag'>
		/// Html string to extract from tags
		/// </param>
		public static string GetTextBetweenTags(string tag)
		{	
			//Not sure the tag needs 2 cleanings....
			CleanData(tag);
			
			var outputName = new StringBuilder();
			
			var posStartTag = tag.IndexOf('>');
			
			foreach(var c in tag.Substring(posStartTag + 1))
			{
				if(c == '<')
					break;
				
				outputName.Append(c);
			}
			
			return CleanData(outputName.ToString());
		}
		
		/// <summary>
		/// Removes unwanted characters from the html string
		/// </summary>
		/// <returns>
		/// Cleaned string
		/// </returns>
		/// <param name='data'>
		/// Data.
		/// </param>
		public static string CleanData(string data)
		{	
			data = data.Trim();
			data = data.Replace("amp;", String.Empty);
			
			return data;
		}
		#endregion
	}
}