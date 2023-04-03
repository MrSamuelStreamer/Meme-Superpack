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
		foreach (Building b in beds)
		{
			var radius = Rand.Range(1f, 3f);
			IntVec3 position = b.Position;
			if (Rand.Chance(0.69f)) b.Destroy(DestroyMode.KillFinalize);
			FireBurstUtility.ThrowFuelTick(position, radius, target);
			GenExplosion.DoExplosion(position, target, radius, DamageDefOf.Flame, null);
			FireUtility.TryStartFireIn(position, target, Rand.Range(0.420f, 0.69f));
		}

		if (SendLetter) SendStandardLetter(parms, new LookTargets(beds));
		return true;
	}
}
