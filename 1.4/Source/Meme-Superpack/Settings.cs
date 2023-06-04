using System;
using System.Collections.Generic;
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
		public bool getStickbugged = true;
		public bool stickbugsCanSkyfall = false;
		public bool coalInRecipes = true;
		public bool floorTrails = true;
		public bool combustionAnimalsCanDoorbash = true;

		private readonly Listing_Standard _options = new();
		private const float RowHeight = 32f;
		private const float Indent = 9f;

		private enum Tab
		{
			Features,
			Events,
			UI
		}

		private static Tab _tab = Tab.Features;

		private Rect DrawTabs(Rect rect)
		{
			List<TabRecord> tabsList = new()
			{
				new TabRecord("Features", () => _tab = Tab.Features,
					_tab == Tab.Features),
				new TabRecord("Events", () => _tab = Tab.Events,
					_tab == Tab.Events),
				new TabRecord("UI", () => _tab = Tab.UI,
					_tab == Tab.UI)
			};

			Rect tabRect = rect.ContractedBy(0, RowHeight);
			TabDrawer.DrawTabs(tabRect, tabsList);

			return tabRect.GetInnerRect();
		}

		public void DoWindowContents(Rect wrect)
		{
			Rect viewPort = DrawTabs(wrect);
			_options.Begin(viewPort);

			switch (_tab)
			{
				case Tab.Events:
					DrawEventsSettings(viewPort);
					break;
				case Tab.Features:
					DrawFeaturesSettings(viewPort);
					break;
				case Tab.UI:
					DrawUISettings(viewPort);
					break;
				default:
					throw new ArgumentException($"Unknown tab selected: {_tab.ToString()}");
			}

			_options.End();
		}

		private void DrawUISettings(Rect viewPort)
		{
			_options.CheckboxLabeled("Allow conk creet icons and extra translations", ref concreteUI,
				"Dynamically translates concrete to conk creet everywhere it can and replaces any icons with the meme image.");
			_options.CheckboxLabeled("Allow Hiding mineable Coal Type", ref coalTypeHidden, "Make the UI not tell you which of the many coals you are mining");
			_options.CheckboxLabeled("Allow Silly Translations", ref sillyTranslations, "Sillier translations of base-game / other mods e.g. Gamer Slop");
		}

		private void DrawFeaturesSettings(Rect viewPort)
		{
			_options.CheckboxLabeled("Allow Icewolf Bad luck", ref icewolf, "Some events target pawns called Icewolf more than usual");
			_options.CheckboxLabeled("Allow kick a nuke", ref kickNukes,
				"Identical to normal tantrum mental break but preferentially targets nukes to destroy with a memey description.");
			_options.CheckboxLabeled("Allow Nukes to explode from melee", ref explosiveNukes,
				"Nukes have been made to not explode when you destroy them, while realistic this is much less fun so this adds back the explosions.");
			_options.CheckboxLabeled("Allow CE Spam", ref ceSpam,
				"Just fills your log with spam about every mod not being CE compatible unless they explicitly made themselves CE compatible.");
			_options.CheckboxLabeled("Allow Building Aura", ref buildingAura, "Things with the building aura will place down buildings or blueprints randomly");
			_options.CheckboxLabeled("Allow Where RimRim", ref whereRimRim, "Daily reminder letter, very annoying");
			_options.CheckboxLabeled("Allow Stockpile affinity", ref stockpileAffinity, "Some creatures will want to go and hang out near your stockpiles.");
			_options.CheckboxLabeled("Allow Meme Resource Spawns", ref memeResourceSpawns, "Allow many many types of coal, fools steel etc");
			_options.CheckboxLabeled("Allow Coal enhanced recipes", ref coalInRecipes, "Allow coal being required for building certain common things like multi-analysers");
			_options.CheckboxLabeled("Allow Stickbuggs to skyfall", ref stickbugsCanSkyfall,
				"Allow the stickbug event to drop stickbugs from the sky. This is funny but be warned it can cause cave-ins.");
			_options.CheckboxLabeled("Allow conk creet to be pretty", ref beautifulConcrete, "Give concrete a decent beauty bonus");
			_options.CheckboxLabeled("Allow Awake", ref awake, "Allow Awake hediff which gives various buffs and debuffs based off how long they've been awake.");
			_options.CheckboxLabeled("Allow combustion animals to door bash", ref combustionAnimalsCanDoorbash,
				"Allow a flaming elephant to bust down your doors to hang out near your stockpile.");
			_options.CheckboxLabeled("Allow floor changing trails like concrete", ref floorTrails,
				"Allow animals to leave behind trails of other floor types, just make sure not to let one wander through your throne room.");
			_options.CheckboxLabeled("Allow Arcadius", ref arcadius, "Makes everyone related to Arcadius, heavily recommended to not turn on! It has caused a lot of errors.");
		}

		private void DrawEventsSettings(Rect viewPort)
		{
			_options.CheckboxLabeled("Allow Steel Loss", ref steelLoss, "All the steel in one stockpile just vanishes. Make sure to split your steel!");
			_options.CheckboxLabeled("Allow bed fires", ref bedFires, "Beds suddenly catch fire");
			_options.CheckboxLabeled("Allow stockpile fires", ref stockpileFires, "Stockpile randomly catches fire to encourage fire-foam poppers");
			_options.CheckboxLabeled("Allow conk creet terrain conversion", ref concreteTerrainConversion, "A random patch of terrain becomes concrete");
			_options.CheckboxLabeled("Allow warcasket event", ref warcasketEvent, "All the warcaskets will get addicted to yayo");
			_options.CheckboxLabeled("Allow Gas-lighting", ref gaslighting, "You'll get a letter that changes a base mechanic of the game like solar panels working in darkness");
			_options.CheckboxLabeled("Allow Diseases", ref diseases, "Allow meme diseases to spawn naturally and as incidents");
			_options.CheckboxLabeled("Allow Getting Stickbugged", ref getStickbugged, "Stickbugs turn up then leave");
			_options.CheckboxLabeled("Allow Police", ref police, "Silly lighting change event with a mood debuff");
			_options.CheckboxLabeled("Allow Grignr", ref grignr, "Allow a random shard of Grignr to attack the colony (Once per game only)\nThis Does not send a letter!");
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
			Scribe_Values.Look(ref getStickbugged, "getStickbugged", true);
			Scribe_Values.Look(ref stickbugsCanSkyfall, "stickbugsCanSkyfall", false);
			Scribe_Values.Look(ref coalInRecipes, "coalInRecipes", true);
			Scribe_Values.Look(ref floorTrails, "floorTrails", true);
			Scribe_Values.Look(ref combustionAnimalsCanDoorbash, "combustionAnimalsCanDoorbash", true);
		}
	}
}
