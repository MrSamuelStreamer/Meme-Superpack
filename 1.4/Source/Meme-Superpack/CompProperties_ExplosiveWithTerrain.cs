using RimWorld;
using Verse;

namespace MSS.MemeSuperpack
{
	public class CompProperties_ExplosiveWithTerrain : CompProperties_Explosive
	{
		public TerrainDef terrain = null;

		public CompProperties_ExplosiveWithTerrain() => compClass = typeof(Comp_ExplosiveWithTerrain);

		public override void ResolveReferences(ThingDef parentDef)
		{
			if (terrain == null) terrain = TerrainDefOf.Concrete;
		}
	}
}
