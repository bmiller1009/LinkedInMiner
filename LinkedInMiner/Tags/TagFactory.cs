using System;
using Logging;

namespace LinkedInMiner.Tags
{
	internal static class TagFactory
    {
        public static TagParser GetTagParser(string html, Logger logger, int? semiKnownID)
        {
            if(html.Contains("Anonymous LinkedIn User"))
				return new AnonTagParser(html, logger);
			else if(html.Contains("redherring"))
				return new SemiKnownTagParser(html, logger);
			else
				return new IdentTagParser(html, logger, semiKnownID);
        }
    }
}