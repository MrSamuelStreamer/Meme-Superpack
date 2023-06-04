using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack.HarmonyPatches;

public class Arcadius
{
	[HarmonyPatch(typeof(Pawn_RelationsTracker), nameof(Pawn_RelationsTracker.ExposeData))]
	public static class CleanBrokenRels
	{
		[HarmonyPrefix]
		public static bool Prefix(ref List<DirectPawnRelation> ___directRelations)
		{
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				___directRelations.RemoveAll(r => r.def == null || r.def.defName == "MSSMeme_Arcadius");
			}

			return true;
		}
	}
}
