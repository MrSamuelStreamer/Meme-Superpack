using RimWorld;
using Verse;

namespace MSS.MemeSuperpack.Incidents;

public class IncidentWorker_TaffRaidEnemy : IncidentWorker_RaidEnemy
{
	protected override bool CanFireNowSub(IncidentParms parms)
	{
		return base.CanFireNowSub(parms) && MemeSuperpackMod.settings.EnableTaffRaids;
	}

	protected override bool TryExecuteWorker(IncidentParms parms)
	{
		parms.faction = Find.FactionManager.FirstFactionOfDef(MemeSuperPackDefOf.MSSMeme_TaffsFaction);

		if (
			parms.faction == null
			|| parms.faction.defeated
			|| parms.faction.AllyOrNeutralTo(Faction.OfPlayer)
		)
			return false;

		return base.TryExecuteWorker(parms);
	}
}
