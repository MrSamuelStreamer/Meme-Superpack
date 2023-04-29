using System.Linq;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack;

public static class DebugActions
{
	[DebugAction("Memes", "Clear broken relations", false, false, false, 0, false,
		allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static void ClearBrokenRels()
	{
		var sum = PawnsFinder.All_AliveOrDead.AsParallel().Sum(pawn =>
		{
			var relsRemoved = pawn.relations?.DirectRelations?.RemoveAll(r => r.def == null || r.otherPawn == null) ?? 0;
			if (relsRemoved > 0) Verse.Log.Message($"Cleaning {relsRemoved} from pawn {pawn.Name?.ToStringFull}");
			return relsRemoved;
		});
		Verse.Log.Message($"Cleaned up {sum} relations");
	}

	[DebugAction("Memes", "Reset AllFather - Arcadius", false, false, false, 0, false,
		allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static void ReApplyYChromosomalAdam()
	{
		GameComponent_ArcadiusRelationManager.CleanUpArcadius();
		Pawn arcadius = GameComponent_ArcadiusRelationManager.GetArcadius(true);
		Verse.Log.Message($"Reset Y-Chromosomal Adam to {arcadius?.Name?.ToStringFull ?? "[Not Set]"} - {arcadius?.ThingID ?? (-1).ToString()}");
	}

	[DebugAction("Memes", "Remove AllFather - Arcadius", false, false, false, 0, false,
		allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static void RemoveYChromosomalAdam()
	{
		GameComponent_ArcadiusRelationManager.CleanUpArcadius();
	}

	[DebugAction("Memes", "Gaslighting - DarkSolar", false, false, false, 0, false,
		allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static void ForceDarkSolar()
	{
		GameComponent_MemeTracker.Instance.StartGasLighting(GameComponent_MemeTracker.GaslightingTopic.DarkSolar);
	}

	[DebugAction("Memes", "Gaslighting - Stop", false, false, false, 0, false,
		allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static void StopGaslighting()
	{
		GameComponent_MemeTracker.Instance.EndGaslighting();
	}

	[DebugAction("Memes", "Grignr Attacks", false, false, false, 0, false,
		allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static void GrignrAttack()
	{
		GameComponent_MemeTracker.Instance.GrignrAttack();
	}

	[DebugAction("Memes", "Toggle Eclipse", false, false, false, 0, false,
		allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static void ToggleEclipse()
	{
		if (Find.World.GameConditionManager.ConditionIsActive(GameConditionDefOf.Eclipse))
		{
			Find.World.GameConditionManager.ActiveConditions.Find(x => x.def == GameConditionDefOf.Eclipse).End();
		}
		else
		{
			IncidentParms incidentParms = new()
			{
				forced = true,
				target = Find.World
			};
			DefDatabase<IncidentDef>.GetNamed("Eclipse").Worker.TryExecute(incidentParms);
		}
	}
}
