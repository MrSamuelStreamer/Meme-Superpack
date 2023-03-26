using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace MSS.MemeSuperpack.HarmonyPatches;

/**
 * Should really make this a Def that others can use but hardcoding for now for speed
 * Should also really precache these
 */
[HarmonyPatch]
public class DarkSolarPowerOutput
{
	[HarmonyTargetMethods]
	public static IEnumerable<MethodBase> TargetMethods()
	{
		yield return AccessTools.PropertyGetter(typeof(CompPowerPlantSolar), "DesiredPowerOutput");
		if (ModLister.GetActiveModWithIdentifier("vanillaexpanded.vfepower") != null)
			yield return AccessTools.PropertyGetter(AccessTools.TypeByName("VanillaPowerExpanded.CompPowerAdvancedSolar"),
				"DesiredPowerOutput");
	}

	private static readonly float FullSunPowerBaseGame =
		(float)AccessTools.Field(typeof(CompPowerPlantSolar), "FullSunPower").GetRawConstantValue();

	private static readonly Lazy<float> FullSunPowerVfePower = new(() =>
		(float)AccessTools.Field(AccessTools.TypeByName("VanillaPowerExpanded.CompPowerAdvancedSolar"), "FullSunPower")
			.GetRawConstantValue());

	private static readonly PropertyInfo RoofedPowerOutputFactorBaseGame =
		AccessTools.Property(typeof(CompPowerPlantSolar), "RoofedPowerOutputFactor");

	private static readonly Lazy<PropertyInfo> RoofedPowerOutputFactorVfePower = new(() =>
		AccessTools.Property(AccessTools.TypeByName("VanillaPowerExpanded.CompPowerAdvancedSolar"),
			"RoofedPowerOutputFactor"));

	[HarmonyPrefix]
	public static bool DesiredPowerOutput(ref float __result, CompPowerPlant __instance)
	{
		if (GameComponent_MemeTracker.Instance?.CurrentGaslightingTopic !=
		    GameComponent_MemeTracker.GaslightingTopic.DarkSolar) return true;
		__result =
			Mathf.Lerp(__instance is CompPowerPlantSolar ? FullSunPowerBaseGame : FullSunPowerVfePower.Value, 0,
				__instance.parent.Map.skyManager.CurSkyGlow) *
			(float)(__instance is CompPowerPlantSolar
				? RoofedPowerOutputFactorBaseGame
				: RoofedPowerOutputFactorVfePower.Value).GetValue(__instance);
		return false;
	}
}
