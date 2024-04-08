using RimWorld;
using Verse;

namespace MSS.MemeSuperpack
{
	public class CompAbilityEffect_ComeAnimal : CompAbilityEffect_WithDuration
	{
		public new CompProperties_AbilityComeAnimal Props => (CompProperties_AbilityComeAnimal)props;

		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);

			PawnKindDef pawnKind = Props.PawnKind();
			Map map = parent.pawn.MapHeld;
			if (pawnKind == null || !TryFindEntryCell(map, out IntVec3 entryCell)) return;

			var numberToWanderIn = Rand.RangeInclusive(1, parent.pawn.GetPsylinkLevel());
			var lurkTime = Rand.RangeInclusive(Props.pawnsStayTicksMin, Props.pawnsStayTicksMax);
			var labelPlural = pawnKind.GetLabelPlural(numberToWanderIn);

			Log.Message($"{numberToWanderIn} {labelPlural} will hang around for {lurkTime} ticks at {target}");
			for (var index = 0; index < numberToWanderIn; ++index)
			{
				IntVec3 loc = CellFinder.RandomSpawnCellForPawnNear(entryCell, map, 10);
				Pawn newPawn = PawnGenerator.GeneratePawn(pawnKind);
				GenSpawn.Spawn(newPawn, loc, map, Rot4.Random);
				newPawn.mindState.exitMapAfterTick = Find.TickManager.TicksGame + lurkTime;
				if (target.IsValid)
					newPawn.mindState.forcedGotoPosition = CellFinder.RandomClosewalkCellNear(target.Cell, map, 10);
				if (Rand.Chance(Props.manhunterChance))
				{
					newPawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent);
					return;
				}

				if (Props.fightTarget && target.HasThing) newPawn.mindState.enemyTarget = target.Thing;
				if (!Props.fightWithSummoner ||
				    !newPawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.BerserkWarcall,
					    causedByPsycast: true)) return;
				newPawn.mindState.mentalStateHandler.CurState.sourceFaction = parent.pawn.Faction;
				newPawn.mindState.mentalStateHandler.CurState.forceRecoverAfterTicks =
					GetDurationSeconds(parent.pawn).SecondsToTicks();
			}
		}

		private static bool TryFindEntryCell(Map map, out IntVec3 entryCell) =>
			RCellFinder.TryFindRandomPawnEntryCell(out entryCell, map, CellFinder.EdgeRoadChance_Animal + 0.2f);
	}
}
