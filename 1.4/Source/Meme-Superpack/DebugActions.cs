using System.Linq;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack;

public static class DebugActions
{
	[DebugAction("Memes", "Reset AllFather - Arcadius", false, false, false, 0, false,
		allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static void ReApplyYChromosomalAdam()
	{
		if (GameComponent_ArcadiusRelationManager.GetArcadius() is not { } arcadius) return;
		foreach (Pawn p in PawnsFinder.All_AliveOrDead.Where(pawn =>
			         pawn.RaceProps is { } raceProps && raceProps.Humanlike && raceProps.IsFlesh &&
			         raceProps.intelligence == Intelligence.Humanlike && pawn != arcadius &&
			         !pawn.relations.DirectRelations.Exists(dr => dr.def == MemeSuperPackDefOf.MSSMeme_Arcadius)))
		{
			p.relations.AddDirectRelation(MemeSuperPackDefOf.MSSMeme_Arcadius, arcadius);
		}
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
			IncidentParms incidentParms = new IncidentParms()
			{
				forced = true,
				target = Find.World
			};
			DefDatabase<IncidentDef>.GetNamed("Eclipse").Worker.TryExecute(incidentParms);
		}
	}
}
