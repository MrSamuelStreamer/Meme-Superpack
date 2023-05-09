using System.Linq;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack;

public class IncidentWorker_GetStickbuggedLol : IncidentWorker
{
	protected virtual bool SendLetter => true;

	protected override bool CanFireNowSub(IncidentParms parms) => MemeSuperpackMod.settings.getStickbugged;

	public bool MaybeSpawnSkyfaller(IntVec3 cell, Map target) => Rand.Chance(0.069f) &&
	                                                             SkyfallerMaker.SpawnSkyfaller(MemeSuperPackDefOf.MSSMeme_StickbugIncoming,
		                                                             PawnGenerator.GeneratePawn(MemeSuperPackDefOf.MSSMeme_Stickbug), cell, target) != null;

	public IntVec3 StickbugWalkIn(Map target, int currentTicks)
	{
		IntVec3 loc = CellFinder.RandomSpawnCellForPawnNear(CellFinder.RandomEdgeCell(target), target, 12);
		Pawn pawn = GenSpawn.Spawn(PawnGenerator.GeneratePawn(MemeSuperPackDefOf.MSSMeme_Stickbug), loc, target, Rot4.Random) as Pawn;
		if (pawn?.mindState is not { } mindState) return loc;
		mindState.exitMapAfterTick = currentTicks + Rand.Range(20000, 180000);
		mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Roaming);
		RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith(c => c.Walkable(target), target, out IntVec3 gotoCell);
		mindState.forcedGotoPosition = gotoCell;
		return loc;
	}

	protected override bool TryExecuteWorker(IncidentParms parms)
	{
		Map target = (Map)parms.target;
		Pawn around = Find.CurrentMap.mapPawns.FreeAdultColonistsSpawned.RandomElement();
		if (around == null) return false;

		var count = Rand.Range(42, 69);
		var ticks = Find.TickManager.TicksGame;

		LookTargets targets = MemeSuperpackMod.settings.stickbugsCanSkyfall && Rand.Bool
			? new LookTargets(GenRadial.RadialCellsAround(around.Position, 3, Rand.Range(20, 40)).Where(c => c.Standable(target))
				.TakeWhile(cell => MaybeSpawnSkyfaller(cell, target) ? count > 0 : count-- > 0).Select(cell => new TargetInfo(cell, target)))
			: new LookTargets(Enumerable.Range(0, count).Select(_ => new TargetInfo(StickbugWalkIn(target, ticks), target)));

		if (!SendLetter) return true;
		SendStandardLetter(parms, targets);
		Find.TickManager.Pause();

		return true;
	}
}
