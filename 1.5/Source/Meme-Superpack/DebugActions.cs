using LudeonTK;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack;

public static class DebugActions
{
	[DebugAction("Memes", "Gaslighting - DarkSolar", requiresRoyalty = false, requiresIdeology = false, requiresBiotech = false, requiresAnomaly = false, displayPriority = 0,
		hideInSubMenu = false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static void ForceDarkSolar()
	{
		GameComponent_MemeTracker.Instance.StartGasLighting(GameComponent_MemeTracker.GaslightingTopic.DarkSolar);
	}

	[DebugAction("Memes", "Gaslighting - Stop", requiresRoyalty = false, requiresIdeology = false, requiresBiotech = false, requiresAnomaly = false, displayPriority = 0,
		hideInSubMenu = false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static void StopGaslighting()
	{
		GameComponent_MemeTracker.Instance.EndGaslighting();
	}

	[DebugAction("Memes", "Grignr Attacks", requiresRoyalty = false, requiresIdeology = false, requiresBiotech = false, requiresAnomaly = false, displayPriority = 0,
		hideInSubMenu = false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static void GrignrAttack()
	{
		GameComponent_MemeTracker.Instance.GrignrAttack();
	}

	[DebugAction("Memes", "Toggle Eclipse", requiresRoyalty = false, requiresIdeology = false, requiresBiotech = false, requiresAnomaly = false, displayPriority = 0,
		hideInSubMenu = false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
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
