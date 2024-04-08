using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace MSS.MemeSuperpack
{
	public class MemeSuperpackMod : Mod
	{
		public static Settings settings;

		public MemeSuperpackMod(ModContentPack content) : base(content)
		{
			Log.Debug("Meme-Superpack is loaded, prepare yourself");

			// initialize settings
			settings = GetSettings<Settings>();

#if DEBUG
			Harmony.DEBUG = true;
#endif

			Harmony harmony = new Harmony("MrSamuelStreamer.rimworld.Meme-Superpack.main");
			harmony.PatchAll();
		}

		public override void DoSettingsWindowContents(Rect inRect)
		{
			base.DoSettingsWindowContents(inRect);
			settings.DoWindowContents(inRect);
		}

		public override string SettingsCategory()
		{
			return "Meme-Superpack";
		}

		public static List<PawnKindDef> CECompatiblePawnKinds() =>
			DefDatabase<PawnKindDef>.AllDefsListForReading
				.Where(d => d.GetModExtension<CECompatibility>()?.CECompatible ?? false)
				.ToList();
	}
}
