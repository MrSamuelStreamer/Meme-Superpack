using System.Linq;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack;

public class IncidentWorker_OutOfSteel : IncidentWorker
{
	protected virtual bool SendLetter => true;

	protected override bool CanFireNowSub(IncidentParms parms) =>
		MemeSuperpackMod.settings.steelLoss && base.CanFireNowSub(parms) &&
		StockpileUtility.FindStockpileWithItem(parms.target as Map, ThingDefOf.Steel, 100, out _);

	public void TryStartFire(Map map, IntVec3 cell)
	{
		foreach (IntVec3 radialPatternInRadius in GenRadial.RadialPatternInRadius(5f))
		{
			IntVec3 c = cell + radialPatternInRadius;
			if (!c.InBounds(map) || !(FireUtility.ChanceToStartFireIn(c, map) > 0) ||
			    !GenSight.LineOfSight(cell, c, map, true)) continue;
			if (Rand.Chance(0.1f)) FilthMaker.TryMakeFilth(c, map, ThingDefOf.Filth_Fuel);
			if (Rand.Chance(0.4f)) FireUtility.TryStartFireIn(c, map, Rand.Range(0.3f, 0.8f), instigator: null);
		}
	}

	protected override bool TryExecuteWorker(IncidentParms parms)
	{
		Map target = (Map)parms.target;
		if (!StockpileUtility.FindStockpileWithItem(parms.target as Map, ThingDefOf.Steel, 100, out Zone zone))
			return false;
		var destroyed = 0;
		foreach (Thing thing in zone.AllContainedThings.Where(t => t.def == ThingDefOf.Steel))
		{
			destroyed += thing.stackCount;
			thing.Destroy();
		}

		if (SendLetter && destroyed > 0)
			SendStandardLetter(parms, new LookTargets(zone.Position, target), new NamedArgument(destroyed, "DESTROYED"),
				new NamedArgument(zone.label, "ZONE_label"));
		return destroyed > 0;
	}
}
