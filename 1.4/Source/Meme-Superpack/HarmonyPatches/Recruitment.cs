using HarmonyLib;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack.HarmonyPatches;

[HarmonyPatch(typeof(RecruitUtility), nameof(RecruitUtility.Recruit))]
public class Recruitment
{
	[HarmonyPostfix]
	public static void Recruit(Faction faction)
	{
		if (faction.IsPlayer) Find.MusicManagerPlay.ForceStartSong(MemeSuperPackDefOf.MSSMeme_BuckoDrinkMusic, false);
	}
}
