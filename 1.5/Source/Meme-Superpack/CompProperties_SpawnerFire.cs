using Verse;

namespace MSS.MemeSuperpack
{
	public class CompProperties_SpawnerFire : CompProperties
	{
		public int fireSize = 5;
		public int spawnTicks = 666;
		public float spawnRadius = 3f;
		public bool behindOnly = false;

		public CompProperties_SpawnerFire() => compClass = typeof(CompSpawnerFire);
	}
}
