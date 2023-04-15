using System.Linq;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack;

public class IncidentWorker_Homeless : IncidentWorker
{
	protected virtual bool SendLetter => true;

	protected override bool CanFireNowSub(IncidentParms parms) =>
		MemeSuperpackMod.settings.bedFires && base.CanFireNowSub(parms) && parms.target is Map map &&
		map.listerBuildings.allBuildingsColonist.Exists(b => b.def.IsBed && b.FlammableNow && b.GetAssignedPawn() != null);

	protected override bool TryExecuteWorker(IncidentParms parms)
	{
		Map target = (Map)parms.target;
		var beds = target.listerBuildings.allBuildingsColonist
			.Where(b => b.def.IsBed && b.FlammableNow && b.GetAssignedPawn() != null).ToList();
		if (beds.Count <= 0) return false;
		foreach (Building b in beds)
		{
			var radius = Rand.Range(1f, 2f);
			IntVec3 position = b.Position;
			if (Rand.Chance(0.1f)) b.Destroy(DestroyMode.KillFinalize);
			FireBurstUtility.ThrowFuelTick(position, radius, target);
			GenExplosion.DoExplosion(position, target, radius, DamageDefOf.Flame, null);
			FireUtility.TryStartFireIn(position, target, Rand.Range(0.420f, 0.69f));
		}

		if (SendLetter) SendStandardLetter(parms, new LookTargets(beds));
		return true;
	}
}
