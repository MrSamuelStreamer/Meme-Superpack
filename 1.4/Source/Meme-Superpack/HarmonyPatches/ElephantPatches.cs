using System.Text;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack.HarmonyPatches;

public class ElephantPatches
{
	[HarmonyPatch(typeof(MassUtility), nameof(MassUtility.Capacity))]
	public static class CECompatibilityCheck
	{
		[HarmonyPrefix]
		public static bool Capacity(ref float __result, Pawn p, StringBuilder explanation = null)
		{
			if (p.def.defName != "MSSMeme_CarryElephant") return true;

			// Mass of a pyramid
			__result = MassUtility.CanEverCarryAnything(p) ? 100000 : 0;

			if (explanation == null) return false;
			if (explanation.Length > 0)
				explanation.AppendLine();
			explanation.Append("  - " + p.LabelShortCap + ": " + __result.ToStringMassOffset());

			return false;
		}
	}
}
