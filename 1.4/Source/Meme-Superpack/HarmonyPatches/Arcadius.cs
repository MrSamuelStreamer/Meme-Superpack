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
			if (!GameComponent_ArcadiusRelationManager.GeneratingArcadius &&
			    GameComponent_ArcadiusRelationManager.GetArcadius() is { } arcadius && result.RaceProps is
				    { Humanlike: true, IsFlesh: true, intelligence: Intelligence.Humanlike } &&
			    !result.relations.DirectRelationExists(MemeSuperPackDefOf.MSSMeme_Arcadius, arcadius))
				result.relations.AddDirectRelation(MemeSuperPackDefOf.MSSMeme_Arcadius, arcadius);
			return result;
		}
	}

	[HarmonyPatch(typeof(SocialCardUtility), "ShouldShowPawnRelations")]
	public static class ArcadiusRelationCardUtil
	{
		[HarmonyPostfix]
		public static bool Postfix(bool result, Pawn pawn)
		{
			return result || GameComponent_ArcadiusRelationManager.GetArcadius() == pawn;
		}
	}
}
