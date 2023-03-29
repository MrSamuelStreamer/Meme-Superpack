using RimWorld;
using Verse;

namespace MSS.MemeSuperpack
{
	public class CompProperties_ExplosiveWithTerrain : CompProperties_Explosive
	{
		public TerrainDef terrain = TerrainDefOf.Concrete;

		public CompProperties_ExplosiveWithTerrain() => compClass = typeof(Comp_ExplosiveWithTerrain);
	}
}
