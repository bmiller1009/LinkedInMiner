using System;
using Logging;

namespace LinkedInMiner.Tags
{	
	/// <summary>
	/// Tag factory.
	/// </summary>
	internal static class TagFactory
    {	
		#region Public Methods
		/// <summary>
		/// Gets the tag parser.
		/// </summary>
		/// <returns>
		/// The tag parser.
		/// </returns>
		/// <param name='html'>
		/// Matched html
		/// </param>
		/// <param name='logger'>
		/// Logger.
		/// </param>
		/// <param name='semiKnownID'>
		/// Semi-known id of the contact
		/// </param>
        public static TagParser GetTagParser(string html, Logger logger, int? semiKnownID)
        {
            if(html.Contains("Anonymous LinkedIn User"))
				return new AnonTagParser(html, logger);
			else if(html.Contains("redherring"))
				return new SemiKnownTagParser(html, logger);
			else
				return new IdentTagParser(html, logger, semiKnownID);
        }
		#endregion
    }
}