using HarmonyLib;
using RimWorld.IO;
using Verse;

namespace MSS.MemeSuperpack.HarmonyPatches
{
	[HarmonyPatch(typeof(Translator), nameof(Translator.Translate), typeof(string))]
	public static class ConkCreetTranslator
	{
		[HarmonyPostfix]
		public static TaggedString Postfix(TaggedString result)
		{
			return result == null || !MemeSuperpackMod.settings.concreteUI
				? result
				: result
					.Replace("concrete", "conk creet baybee")
					.Replace("Concrete", "Conk creet baybee")
					.Replace("cement", "cement (das conk creet baybee)")
					.Replace("Cement", "Cement (das conk creet baybee)");
		}
	}

	[HarmonyPatch(typeof(Def), nameof(Def.LabelCap), MethodType.Getter)]
	public static class ConkCreetTranslatorLabel
	{
		[HarmonyPostfix]
		public static TaggedString Postfix(TaggedString result)
		{
			return result == null || !MemeSuperpackMod.settings.concreteUI
				? result
				: result
					.Replace("concrete", "conk creet baybee")
					.Replace("Concrete", "Conk creet baybee")
					.Replace("cement", "cement (das conk creet baybee)")
					.Replace("Cement", "Cement (das conk creet baybee)");
		}
	}

	[HarmonyPatch(typeof(DefInjectionPackage), "AddDataFromFile")]
	public static class NoSillytranslationsForDefInjectedFiles
	{
		public static string TogglePrefix = "MSSMeme_Silly_";

		[HarmonyPrefix]
		public static bool AddDataFromFile(
			VirtualFile file)
		{
			var keepFile = MemeSuperpackMod.settings.sillyTranslations || !file.Name.StartsWith(TogglePrefix);
			if (!keepFile) Log.Message("Skipping load of silly DefInjected file" + file.FullPath);
			return keepFile;
		}
	}

	[HarmonyPatch(typeof(LoadedLanguage), "LoadFromFile_Keyed")]
	public static class NoSillytranslationsForKeyed
	{
		public static string TogglePrefix = "MSSMeme_Silly_";

		[HarmonyPrefix]
		public static bool LoadFromFile_Keyed(VirtualFile file)
		{
			var keepFile = MemeSuperpackMod.settings.sillyTranslations || !file.Name.StartsWith(TogglePrefix);
			if (!keepFile) Log.Message("Skipping load of silly Keyed file" + file.FullPath);
			return keepFile;
		}
	}
}
