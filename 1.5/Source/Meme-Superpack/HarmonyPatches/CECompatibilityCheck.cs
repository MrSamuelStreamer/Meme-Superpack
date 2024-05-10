using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack.HarmonyPatches
{
	[HarmonyPatch(typeof(GameComponentUtility), nameof(GameComponentUtility.FinalizeInit))]
	public static class CECompatibilityCheck
	{
		[HarmonyPostfix]
		public static void Postfix()
		{
			if (!MemeSuperpackMod.settings.ceSpam) return;
			Log.Message("Checking CE Compatibility.");
			var modCECompatibilityList =
				DefDatabase<ModCECompatibilityList>.GetNamed("MSSMeme_ModCECompatibilityList", false)?.compatibleModPackageIds
					?.ToHashSet() ?? new HashSet<string>();
			modCECompatibilityList.AddRange(DefDatabase<ModCECompatibility>.AllDefs
				.Select(mcp => mcp.modContentPack.PackageId).ToHashSet());
			var ceIncompatibleMods = new List<string>();
			var ceCompatibleMods = new List<string>();
			foreach (ModMetaData mod in ModLister.AllInstalledMods.Where(mmd => mmd.Active))
			{
				var modDesc = $"{mod.Name} ({mod.PackageId})";
				(modCECompatibilityList.Contains(mod.PackageId) ? ceCompatibleMods : ceIncompatibleMods).Add(modDesc);
				if (ceIncompatibleMods.Contains(modDesc)) Log.Message($"{modDesc}) is not CE Compatible... for shame!");
			}

			Log.Message($"{ceCompatibleMods.Count} / {ceCompatibleMods.Count + ceIncompatibleMods.Count} are CE Compatible.");
			if (ceIncompatibleMods.Count > 0)
			{
				Verse.Log.Error(
					"Remember, it is every mod author's responsibility to make sure their mods are CE Compatible. Ask them to add an `MSS.MemeSuperpack.ModCECompatibility` Def or patch their mod into the list in the `MSSMeme_ModCECompatibilityList` Def");
			}

			Find.LetterStack.ReceiveLetter(LetterMaker.MakeLetter(
				$"CE Compatibility {ceCompatibleMods.Count} / {ceCompatibleMods.Count + ceIncompatibleMods.Count}",
				$"Congratulations on activating CE. The more of your mods are CE Compatible the more EPIC your Rimworld experience will be.\n\nYou have the following CE Compatible mods:\n{ceCompatibleMods.Join(delimiter: "\n  ")}",
				LetterDefOf.PositiveEvent));
		}
	}
}
