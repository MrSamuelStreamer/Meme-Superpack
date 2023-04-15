using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack.HarmonyPatches;

/**
 * Should really make this a Def that others can use but hardcoding for now for speed
 * Should also really precache these
 */
public class JokeSaver
{
	[HarmonyPatch(typeof(ThingDef), nameof(ThingDef.SpecialDisplayStats))]
	public static class RedactMinableThings
	{
		private static string MinableThingLabel = "Stat_MineableThing_Name".Translate().CapitalizeFirst();

		[HarmonyPostfix]
		public static IEnumerable<StatDrawEntry> SpecialDisplayStats(IEnumerable<StatDrawEntry> result)
		{
			foreach (StatDrawEntry entry in result)
			{
				if (!MemeSuperpackMod.settings.coalTypeHidden || entry.LabelCap != MinableThingLabel ||
				    !(entry.ValueString.Contains("Coal") || entry.ValueString.Contains("coal"))) yield return entry;
			}
		}
	}
}
