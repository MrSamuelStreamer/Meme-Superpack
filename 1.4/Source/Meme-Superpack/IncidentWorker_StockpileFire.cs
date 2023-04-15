using System.Collections.Generic;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack;

public class IncidentWorker_StockpileFire : IncidentWorker
{
	private Stack<IntVec3> stockpileLocations = new();
	protected virtual bool SendLetter => true;

	protected override bool CanFireNowSub(IncidentParms parms) =>
		MemeSuperpackMod.settings.stockpileFires && base.CanFireNowSub(parms) &&
		TryFindRootCell(parms.target as Map, out _);

	public void TryStartFire(Map map, IntVec3 cell)
	{
		foreach (IntVec3 radialPatternInRadius in GenRadial.RadialPatternInRadius(5f))
		{
			IntVec3 c = cell + radialPatternInRadius;
			if (!c.InBounds(map) || !(FireUtility.ChanceToStartFireIn(c, map) > 0) ||
			    !GenSight.LineOfSight(cell, c, map, true)) continue;
			if (Rand.Chance(0.1f)) FilthMaker.TryMakeFilth(c, map, ThingDefOf.Filth_Fuel);
			if (Rand.Chance(0.4f)) FireUtility.TryStartFireIn(c, map, Rand.Range(0.3f, 0.8f));
		}
	}

	protected override bool TryExecuteWorker(IncidentParms parms)
	{
		Map target = (Map)parms.target;
		IntVec3 cell = stockpileLocations.Pop();
		if (!cell.IsValid) return false;
		TryStartFire(target, cell);
		if (SendLetter) SendStandardLetter(parms, new LookTargets(cell, target));
		return true;
	}

	protected virtual bool TryFindRootCell(Map map, out IntVec3 cell)
	{
		cell = (stockpileLocations.Count <= 0 && !StockpileUtility.FindStockpiles(map, ref stockpileLocations))
			? IntVec3.Invalid
			: stockpileLocations.Peek();
		return cell.IsValid;
	}
}
