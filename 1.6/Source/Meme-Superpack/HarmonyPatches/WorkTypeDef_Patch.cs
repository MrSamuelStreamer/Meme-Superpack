using HarmonyLib;
using MSS.MemeSuperpack;
using Verse;

namespace MSS.MemeSuperpack.HarmonyPatches;

[HarmonyPatch(typeof(WorkTypeDef))]
public static class WorkTypeDef_Patch
{
	[HarmonyPatch(nameof(WorkTypeDef.VisibleNow))]
	[HarmonyPostfix]
	public static void VisibleNow_Patch(WorkTypeDef __instance, ref bool __result)
	{
		if (__instance != MemeSuperPackDefOf.MSSMeme_AnomalyPrevention)
			return;

		if (!MemeSuperpackMod.settings.EnableDirtJobs)
			__result = false;
	}
}
