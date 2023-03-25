using System.Linq;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack;

public static class DebugActions
{
	[DebugAction("General", "Reset AllFather - Arcadius", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static void ReApplyYChromosomalAdam()
	{
		if (GameComponent_ArcadiusRelationManager.GetArcadius() is not {} arcadius) return;
		foreach (Pawn p in PawnsFinder.All_AliveOrDead.Where(pawn =>
			         pawn.RaceProps is { } raceProps && raceProps.Humanlike && raceProps.IsFlesh &&
			         raceProps.intelligence == Intelligence.Humanlike && pawn != arcadius &&
			         !pawn.relations.DirectRelations.Exists(dr => dr.def == MemeSuperPackDefOf.MSSMeme_Arcadius)))
		{
			p.relations.AddDirectRelation(MemeSuperPackDefOf.MSSMeme_Arcadius, arcadius);
		}
	}
}
