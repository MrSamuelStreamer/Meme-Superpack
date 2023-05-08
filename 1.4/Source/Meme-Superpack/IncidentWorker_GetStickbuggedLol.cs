using System.Linq;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack;

public class IncidentWorker_GetStickbuggedLol : IncidentWorker
{
	protected virtual bool SendLetter => true;

	protected override bool CanFireNowSub(IncidentParms parms) => MemeSuperpackMod.settings.getStickbugged;

	protected override bool TryExecuteWorker(IncidentParms parms)
	{
		Map target = (Map)parms.target;
		Pawn around = Find.CurrentMap.mapPawns.FreeAdultColonistsSpawned.RandomElement();
		if (around == null) return false;
		foreach (IntVec3 cell in GenRadial.RadialCellsAround(around.Position, Rand.Range(5, 10), false).Where(c => c.Standable(target)))
		{
			SkyfallerMaker.SpawnSkyfaller(MemeSuperPackDefOf.MSSMeme_StickbugIncoming, PawnGenerator.GeneratePawn(MemeSuperPackDefOf.MSSMeme_Stickbug), cell, target);
		}

		if (!SendLetter) return true;
		SendStandardLetter(parms, new LookTargets(around.Position, target));
		Find.TickManager.Pause();

		return true;
	}
}
