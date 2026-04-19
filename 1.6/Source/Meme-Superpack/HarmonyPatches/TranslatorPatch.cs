using System.Collections.Generic;
using System.Diagnostics;
using HarmonyLib;
using RimWorld.IO;
using Verse;

namespace MSS.MemeSuperpack.HarmonyPatches;

/**
 * This class handles most of the coordination for the conk crete translations.
 * We use a pair of dictionaries rather than a proper LRU cache to save extra cycles
 * Because strings on the UI are likely to stay there tens or hundreds of times we avoid the bookkeeping by using plain dicts.
 * We then start a fresh dict every 5 mins ish but keep the old one around to let us copy over currently active or common translations without re-translating.
 */
public static class ConkCreetSubstitute
	{
		public const string SillyTogglePrefix = "MSSMeme_Silly_";
		private const double RotateEverySeconds = 300.0;
		private const double OverlapSeconds = 60.0;

		private static readonly long RotateEveryTicks = (long)(Stopwatch.Frequency * RotateEverySeconds);
		private static readonly long OverlapTicks = (long)(Stopwatch.Frequency * OverlapSeconds);

		private static readonly object Lock = new();
		private static Dictionary<string, string> _currentCache = new(64 *  1024);
		private static Dictionary<string, string> _previousCache;
		private static long _nextRotateAt = Stopwatch.GetTimestamp() + RotateEveryTicks;
		private static long _endOverlapAt;

		public static string ApplyTranslation(string s)
		{
			if (string.IsNullOrEmpty(s))
				return s;

			lock (Lock)
			{
				if (_currentCache.TryGetValue(s, out string hit))
					return hit;

				if (_previousCache != null && _previousCache.TryGetValue(s, out hit))
				{
					_currentCache[s] = hit;
					return hit;
				}
			}

			string transformed = s
				.Replace("concrete", "conk creet baybee")
				.Replace("Concrete", "Conk creet baybee")
				.Replace("cement", "cement (das conk creet baybee)")
				.Replace("Cement", "Cement (das conk creet baybee)");

			lock (Lock)
			{
				long now = Stopwatch.GetTimestamp();

				if (_previousCache != null && now >= _endOverlapAt)
					_previousCache = null;

				if (now >= _nextRotateAt)
				{
					_previousCache = _currentCache;
					_currentCache = new Dictionary<string, string>(64 *  1024);
					_endOverlapAt = now + OverlapTicks;
					_nextRotateAt = now + RotateEveryTicks;
				}

				_currentCache[s] = transformed;
			}

			return transformed;
		}

		public static void Rewrite(ref TaggedString result)
		{
			if (!MemeSuperpackMod.settings.concreteUI)
				return;
			string raw = result.RawText;
			if (raw == null)
				return;
			string rewritten = ApplyTranslation(raw);
			if (!ReferenceEquals(rewritten, raw))
				result = new TaggedString(rewritten);
		}

		public static bool ShouldKeepSillyFile(VirtualFile file, string kind)
		{
			bool keepFile =
				MemeSuperpackMod.settings.sillyTranslations || !file.Name.StartsWith(SillyTogglePrefix);
			if (!keepFile)
				Log.Message($"Skipping load of silly {kind} file{file.FullPath}");
			return keepFile;
		}
	}

[HarmonyPatch(typeof(Translator), nameof(Translator.Translate), typeof(string))]
public static class ConkCreetTranslator
{
	[HarmonyPostfix]
	public static void Postfix(ref TaggedString __result) =>
		ConkCreetSubstitute.Rewrite(ref __result);
}

[HarmonyPatch(typeof(Def), nameof(Def.LabelCap), MethodType.Getter)]
public static class ConkCreetTranslatorLabel
{
	[HarmonyPostfix]
	public static void Postfix(ref TaggedString __result) =>
		ConkCreetSubstitute.Rewrite(ref __result);
}



[HarmonyPatch(typeof(DefInjectionPackage), "AddDataFromFile")]
public static class NoSillytranslationsForDefInjectedFiles
{
	[HarmonyPrefix]
	public static bool AddDataFromFile(VirtualFile file) =>
		ConkCreetSubstitute.ShouldKeepSillyFile(file, "DefInjected");
}

[HarmonyPatch(typeof(LoadedLanguage), "LoadFromFile_Keyed")]
public static class NoSillytranslationsForKeyed
{
	[HarmonyPrefix]
	public static bool LoadFromFile_Keyed(VirtualFile file) =>
		ConkCreetSubstitute.ShouldKeepSillyFile(file, "Keyed");
}
