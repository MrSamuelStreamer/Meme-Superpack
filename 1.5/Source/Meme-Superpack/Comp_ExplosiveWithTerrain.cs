using RimWorld;
using Verse;

namespace MSS.MemeSuperpack;

public class Comp_ExplosiveWithTerrain : CompExplosive
{
	public new CompProperties_ExplosiveWithTerrain Props => (CompProperties_ExplosiveWithTerrain)props;

	public override void PostDestroy(DestroyMode mode, Map previousMap)
	{
		var radius = ExplosiveRadius();
		if (radius <= 0f || mode != DestroyMode.KillFinalize || !Props.explodeOnKilled) return;
		IntVec3 cell = parent.PositionHeld;
		Detonate(previousMap, true);
		foreach (IntVec3 radialPatternInRadius in GenRadial.RadialPatternInRadius(radius))
		{
			IntVec3 c = cell + radialPatternInRadius;
			if (c.InBounds(previousMap))
				previousMap.terrainGrid.SetTerrain(c, Props.terrain);
		}
	}
}