using Verse;

namespace MSS.MemeSuperpack
{
	public class CompProperties_TerrainTrail : CompProperties
	{
		public int spawnTicks = 175;
		public TerrainDef terrain;

		public CompProperties_TerrainTrail() => compClass = typeof(CompTerrainTrail);
	}
}
