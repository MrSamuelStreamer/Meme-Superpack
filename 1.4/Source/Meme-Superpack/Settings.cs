using UnityEngine;
using Verse;

namespace MSS.MemeSuperpack
{
	public class Settings : ModSettings
	{
		//Use Mod.settings.setting to refer to this setting.
		public bool police = true;
		public bool icewolf = true;
		public bool steelLoss = true;
		public bool bedFires = true;
		public bool stockpileFires = true;
		public bool concreteTerrainConversion = true;
		public bool warcasketEvent = true;
		public bool kickNukes = true;
		public bool stockpileAffinity = true;
		public bool ceSpam = true;
		public bool gaslighting = true;
		public bool grignr = true;
		public bool buildingAura = true;
		public bool whereRimRim = true;
		public bool concreteUI = true;
		public bool coalTypeHidden = true;
		public bool sillyTranslations = true;
		public bool arcadius = false;
		public bool awake = true;
		public bool explosiveNukes = true;
		public bool beautifulConcrete = true;
		public bool diseases = true;
		public bool memeResourceSpawns = true;

		public void DoWindowContents(Rect wrect)
		{
			Listing_Standard options = new();
			options.Begin(wrect);

			options.Label("Events:");
			options.CheckboxLabeled("Allow Police", ref police);
			options.CheckboxLabeled("Allow Icewolf Bad luck", ref icewolf);
			options.CheckboxLabeled("Allow Steel Loss", ref steelLoss);
			options.CheckboxLabeled("Allow bed fires", ref bedFires);
			options.CheckboxLabeled("Allow stockpile fires", ref stockpileFires);
			options.CheckboxLabeled("Allow conk creet terrain conversion", ref concreteTerrainConversion);
			options.CheckboxLabeled("Allow conk creet icons and extra translations", ref concreteUI);
			options.CheckboxLabeled("Allow conk creet to be pretty", ref beautifulConcrete);
			options.CheckboxLabeled("Allow warcasket event", ref warcasketEvent);
			options.CheckboxLabeled("Allow kick a nuke", ref kickNukes);
			options.CheckboxLabeled("Allow Nukes to explode from melee", ref explosiveNukes);
			options.CheckboxLabeled("Allow CE Spam", ref ceSpam);
			options.CheckboxLabeled("Allow Gas-lighting", ref gaslighting);
			options.CheckboxLabeled("Allow Grignr", ref grignr);
			options.CheckboxLabeled("Allow Building Aura", ref buildingAura);
			options.CheckboxLabeled("Allow Where RimRim", ref whereRimRim);
			options.CheckboxLabeled("Allow Hiding mineable Coal Type", ref coalTypeHidden);
			options.CheckboxLabeled("Allow Silly Translations", ref sillyTranslations);
			options.CheckboxLabeled("Allow Arcadius", ref arcadius);
			options.CheckboxLabeled("Allow Stockpile affinity", ref stockpileAffinity);
			options.CheckboxLabeled("Allow Awake", ref awake);
			options.CheckboxLabeled("Allow Diseases", ref diseases);
			options.CheckboxLabeled("Allow Meme Resource Spawns", ref memeResourceSpawns);
			options.Gap();

			options.End();
		}

		public override void ExposeData()
		{
			Scribe_Values.Look(ref police, "police", true);
			Scribe_Values.Look(ref icewolf, "icewolf", true);
			Scribe_Values.Look(ref steelLoss, "steelLoss", true);
			Scribe_Values.Look(ref bedFires, "bedFires", true);
			Scribe_Values.Look(ref stockpileFires, "stockpileFires", true);
			Scribe_Values.Look(ref concreteTerrainConversion, "concreteTerrainConversion", true);
			Scribe_Values.Look(ref warcasketEvent, "warcasketEvent", true);
			Scribe_Values.Look(ref kickNukes, "kickNukes", true);
			Scribe_Values.Look(ref stockpileAffinity, "stockpileAffinity", true);
			Scribe_Values.Look(ref ceSpam, "ceSpam", true);
			Scribe_Values.Look(ref gaslighting, "gaslighting", true);
			Scribe_Values.Look(ref grignr, "grignr", true);
			Scribe_Values.Look(ref buildingAura, "buildingAura", true);
			Scribe_Values.Look(ref whereRimRim, "whereRimRim", true);
			Scribe_Values.Look(ref concreteUI, "concreteUI", true);
			Scribe_Values.Look(ref coalTypeHidden, "coalTypeHidden", true);
			Scribe_Values.Look(ref sillyTranslations, "sillyTranslations", true);
			Scribe_Values.Look(ref arcadius, "arcadius", true);
			Scribe_Values.Look(ref explosiveNukes, "explosiveNukes", true);
			Scribe_Values.Look(ref awake, "awake", true);
			Scribe_Values.Look(ref beautifulConcrete, "beautifulConcrete", true);
			Scribe_Values.Look(ref diseases, "diseases", true);
			Scribe_Values.Look(ref memeResourceSpawns, "memeResourceSpawns", true);
		}
	}
}
