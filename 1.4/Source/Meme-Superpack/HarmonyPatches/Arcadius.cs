using HarmonyLib;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack.HarmonyPatches;

public class Arcadius
{
	[HarmonyPatch(typeof(PawnGenerator), nameof(PawnGenerator.GeneratePawn), new[] { typeof(PawnGenerationRequest) })]
	public static class ArcadiusRelation
	{
		[HarmonyPostfix]
		public static Pawn Postfix(Pawn result, PawnGenerationRequest request)
		{
			if (MemeSuperpackMod.settings.arcadius && !GameComponent_ArcadiusRelationManager.GeneratingArcadius &&
			    GameComponent_ArcadiusRelationManager.GetArcadius() is { } arcadius && result.RaceProps is
				    { Humanlike: true, IsFlesh: true, intelligence: Intelligence.Humanlike } &&
			    (!result.relations?.DirectRelationExists(MemeSuperPackDefOf.MSSMeme_Arcadius, arcadius) ?? true))
			{
				result.relations?.AddDirectRelation(MemeSuperPackDefOf.MSSMeme_Arcadius, arcadius);
			}

			return result;
		}
	}

	// Currently removed as it causes weird flickering.
	// [HarmonyPatch(typeof(SocialCardUtility), "ShouldShowPawnRelations")]
	// public static class ArcadiusRelationCardUtil
	// {
	// 	[HarmonyPostfix]
	// 	public static bool Postfix(bool result, Pawn pawn)
	// 	{
	// 		return result || (MemeSuperpackMod.settings.arcadius && GameComponent_ArcadiusRelationManager.GetArcadius() == pawn);
	// 	}
	// }
}
