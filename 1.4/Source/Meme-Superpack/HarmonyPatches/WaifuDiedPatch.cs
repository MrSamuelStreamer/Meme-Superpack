using HarmonyLib;
using MSS.MemeSuperpack.Waifu;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack.HarmonyPatches
{
	[HarmonyPatch(typeof(TaleUtility), nameof(TaleUtility.Notify_PawnDied))]
	public static class WaifuDiedPatch
	{
		[HarmonyPrefix]
		public static bool Notify_PawnDied(Pawn victim, DamageInfo? dinfo)
		{
			if (ModsConfig.IdeologyActive && victim.Faction.IsPlayerSafe() && victim.Ideo.GetWaifu() == victim)
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(MemeSuperPackDefOf.MSSMeme_WaifuDied));

			return true;
		}
	}
}
