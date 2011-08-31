using System;
namespace LinkedInMiner
{
	public abstract class AbstractTagFactory 
	{
    	public abstract AnonTag GetAnonTag();
    	public abstract SemiKnownTag CreateSemiKnownTag();
    	public abstract IdentTag CreateIdentTag();
	}

}

