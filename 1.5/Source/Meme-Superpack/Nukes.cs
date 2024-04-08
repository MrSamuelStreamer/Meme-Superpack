using System;
using System.Collections.Generic;
using Verse;

namespace MSS.MemeSuperpack
{
	public class Nukes : Def
	{
		public List<ThingDef> nukeDefs;

		public static Lazy<HashSet<ThingDef>> NukeDefs => new(() =>
			new HashSet<ThingDef>(DefDatabase<Nukes>.GetNamedSilentFail("MSSMeme_Nukes")?.nukeDefs ?? new List<ThingDef>()));
	}
}
