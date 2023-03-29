using RimWorld;
using Verse;

namespace MSS.MemeSuperpack;

public class IncidentWorker_TerrainDasConkCreetBaybee : IncidentWorker
{
	protected virtual bool SendLetter => true;

	protected override bool CanFireNowSub(IncidentParms parms) =>
		base.CanFireNowSub(parms) && TryFindRootCell(parms.target as Map, out IntVec3 _);

	public void MakeConkCreetBaybee(Map map, IntVec3 cell)
	{
		foreach (IntVec3 radialPatternInRadius in GenRadial.RadialPatternInRadius(10f))
		{
			IntVec3 c = cell + radialPatternInRadius;
			if (c.InBounds(map))
				map.terrainGrid.SetTerrain(c, TerrainDefOf.Concrete);
		}
	}

	protected override bool TryExecuteWorker(IncidentParms parms)
	{
		Map target = (Map)parms.target;
		IntVec3 cell;
		if (!TryFindRootCell(target, out cell))
			return false;
		MakeConkCreetBaybee(target, cell);
		if (SendLetter)
			SendStandardLetter(parms, new LookTargets(cell, target));
		return true;
	}

	protected virtual bool TryFindRootCell(Map map, out IntVec3 cell) => CellFinderLoose.TryFindRandomNotEdgeCellWith(10,
		x => map.terrainGrid.TerrainAt(x) != TerrainDefOf.Concrete && map.reachability.CanReachColony(x), map, out cell);
}
