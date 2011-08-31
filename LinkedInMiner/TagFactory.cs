using System;
namespace LinkedInMiner
{
	public class TagFactory : AbstractTagFactory
	{	
		public TagFactory ()
		{
		}
		
		#region implemented abstract members of LinkedInMiner.AbstractTagFactory
		public override AnonTag GetAnonTag ()
		{
			throw new System.NotImplementedException();
		}
		
		
		public override SemiKnownTag CreateSemiKnownTag ()
		{
			throw new System.NotImplementedException();
		}
		
		
		public override IdentTag CreateIdentTag ()
		{
			throw new System.NotImplementedException();
		}
	}
}

