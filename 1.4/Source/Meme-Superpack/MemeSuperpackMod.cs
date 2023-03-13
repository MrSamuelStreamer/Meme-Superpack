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
			Log.Message("Hello world from Meme-Superpack");

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
	}
}
