using RimWorld;
using Verse;

namespace MSS.MemeSuperpack;

public class IncidentWorker_Icewolf : IncidentWorker
{
	protected virtual bool SendLetter => true;

	protected override bool CanFireNowSub(IncidentParms parms) =>
		MemeSuperpackMod.settings.icewolf && base.CanFireNowSub(parms) &&
		Find.CurrentMap.mapPawns.AllPawns.Exists(p => p.Name?.ToStringFull?.ToLowerInvariant().Contains("icewo") ?? false);

	protected override bool TryExecuteWorker(IncidentParms parms)
	{
		Map target = (Map)parms.target;
		Pawn iceWolf =
			Find.CurrentMap.mapPawns.AllPawns.Find(p => p.Name?.ToStringFull?.ToLowerInvariant().Contains("icewo") ?? false);
		if (iceWolf == null) return false;
		var thingList = ThingSetMakerDefOf.Meteorite.root.Generate();
		RCellFinder.TryFindRandomCellNearWith(iceWolf.Position, _ => true, target, out IntVec3 cell, 5, 10);
		SkyfallerMaker.SpawnSkyfaller(ThingDefOf.MeteoriteIncoming, thingList, cell, target);
		if (SendLetter)
			SendStandardLetter(parms, new LookTargets(iceWolf), new NamedArgument(iceWolf.Name.ToStringFull, "ICEWOLF_name"));
		return true;
	}
}
