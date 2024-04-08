using System.Collections.Generic;
using Verse;

namespace MSS.MemeSuperpack
{
	public class CompProperties_BuildingAura : CompProperties
	{
		public bool blueprint = true;
		public int spawnTicks = 237;
		public List<ThingDef> buildables;
		public List<ThingDef> stuff;

		public CompProperties_BuildingAura() => compClass = typeof(CompBuildingAura);
	}
}
