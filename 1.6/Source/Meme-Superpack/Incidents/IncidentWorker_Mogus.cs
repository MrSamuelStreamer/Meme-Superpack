using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack.Incidents;

public class IncidentWorker_Mogus : IncidentWorker
{
	public static List<PawnKindDef> ValidPawnKindDef =>
		[
			MemeSuperPackDefOf.MSSMeme_MogusKind_Blue,
			MemeSuperPackDefOf.MSSMeme_MogusKind_Red,
			MemeSuperPackDefOf.MSSMeme_MogusKind_Green,
			// MemeSuperPackDefOf.MSSMeme_MogusKind_Yellow
		];

	protected override bool CanFireNowSub(IncidentParms parms)
	{
		return MemeSuperpackMod.settings.EnableMogus
			&& base.CanFireNowSub(parms)
			&& TryFindEntryCell((Map)parms.target, out IntVec3 _);
	}

	public static bool TryFindEntryCell(Map map, out IntVec3 cell)
	{
		return CellFinder.TryFindRandomEdgeCellWith(
			c => map.reachability.CanReachColony(c) && !c.Fogged(map),
			map,
			CellFinder.EdgeRoadChance_Neutral,
			out cell
		);
	}

	public virtual Pawn SpawnPawn(Map map)
	{
		Pawn pawn = PawnGenerator.GeneratePawn(
			new PawnGenerationRequest(
				ValidPawnKindDef.RandomElement(),
				Faction.OfPlayer,
				forceGenerateNewPawn: true
			)
		);

		IntVec3 cell;
		if (!TryFindEntryCell(map, out cell))
		{
			cell =
				CellFinderLoose.TryGetRandomCellWith(
					c => c.Standable(map) && !c.Fogged(map),
					map,
					1000,
					out var found
				) && found.IsValid
					? found
					: CellFinder.RandomCell(map);
		}
		GenSpawn.Spawn(pawn, cell, map);

		pawn.guest?.Notify_PawnRecruited();
		pawn.caller?.DoCall();

		if (def.pawnHediff != null)
		{
			pawn.health.AddHediff(def.pawnHediff);
		}

		return pawn;
	}

	protected override bool TryExecuteWorker(IncidentParms parms)
	{
		if (!MemeSuperpackMod.settings.EnableMogus)
			return false;

		Map target = (Map)parms.target;
		if (!TryFindEntryCell(target, out IntVec3 _))
		{
			return false;
		}

		SpawnPawn(target);

		return true;
	}
}
