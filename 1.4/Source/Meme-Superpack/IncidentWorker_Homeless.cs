using System.Linq;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack;

public class IncidentWorker_Homeless : IncidentWorker
{
	protected virtual bool SendLetter => true;

	protected override bool CanFireNowSub(IncidentParms parms) =>
		base.CanFireNowSub(parms) && parms.target is Map map &&
		map.listerBuildings.allBuildingsColonist.Exists(b => b.def.IsBed && b.FlammableNow);

	protected override bool TryExecuteWorker(IncidentParms parms)
	{
		Map target = (Map)parms.target;
		var beds = target.listerBuildings.allBuildingsColonist.Where(b => b.def.IsBed && b.FlammableNow).ToList();
		if (beds.Count <= 0) return false;
		foreach (Building b in beds) FireUtility.TryStartFireIn(b.Position, target, Rand.Range(0.420f, 0.69f));
		if (SendLetter) SendStandardLetter(parms, new LookTargets(beds));
		return true;
	}
}
