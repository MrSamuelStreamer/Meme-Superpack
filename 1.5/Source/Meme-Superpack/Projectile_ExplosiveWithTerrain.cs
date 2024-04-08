using RimWorld;
using Verse;

namespace MSS.MemeSuperpack
{
	public class Projectile_ExplosiveWithTerrain : Projectile_Explosive
	{
		public bool WasBlockedByShield = false;

		protected override void Explode()
		{
			Map map = this.Map;
			base.Explode();
			if (WasBlockedByShield) return;
			foreach (IntVec3 radialPatternInRadius in GenRadial.RadialPatternInRadius(10f))
			{
				IntVec3 c = Position + radialPatternInRadius;
				if (map != null && c.InBounds(map))
					map.terrainGrid.SetTerrain(c, TerrainDefOf.Concrete);
			}
		}

		protected override void Impact(Thing hitThing, bool blockedByShield = false)
		{
			WasBlockedByShield = blockedByShield;
			try
			{
				base.Impact(hitThing, blockedByShield);
			}
			finally
			{
				WasBlockedByShield = false;
			}
		}
	}
}
