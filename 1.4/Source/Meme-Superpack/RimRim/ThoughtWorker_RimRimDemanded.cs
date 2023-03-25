using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack.RimRim;

public class ThoughtWorker_RimRimDemanded : ThoughtWorker_Precept
{
	public Lazy<HashSet<ThoughtDef>> WatchedRimRimThoughts = new(() => new HashSet<ThoughtDef>()
	{
		MemeSuperPackDefOf.MSSMeme_RimRim_Watched_Poor,
		MemeSuperPackDefOf.MSSMeme_RimRim_Watched_Meh,
		MemeSuperPackDefOf.MSSMeme_RimRim_Watched_Good
	});

	[ItemCanBeNull] public Lazy<ResearchProjectDef> ElectricityResearch =
		new(() => DefDatabase<ResearchProjectDef>.GetNamedSilentFail("Electricity"));

	[ItemCanBeNull]
	public Lazy<JoyKindDef> TelevisionJoyKind = new(() => DefDatabase<JoyKindDef>.GetNamedSilentFail("Television"));

	public static bool PlayerCouldUseTelevision = false;
	public static int LastTickCheckedForTelevision = 0;

	public bool PlayerHasTelevision()
	{
		if (PlayerCouldUseTelevision) return true;
		if (Find.TickManager.TicksGame < LastTickCheckedForTelevision + GenTicks.TickLongInterval) return false;

		PlayerCouldUseTelevision = Find.Maps.Exists(m => m.IsPlayerHome && m.listerBuildings.ColonistsHaveBuilding(t =>
			(t.def?.building?.joyKind ?? JoyKindDefOf.Social) == TelevisionJoyKind.Value));
		LastTickCheckedForTelevision = Find.TickManager.TicksGame;
		return PlayerCouldUseTelevision;
	}

	protected override ThoughtState ShouldHaveThought(Pawn p)
	{
		if (p.Faction != Faction.OfPlayer ||
		    !(p.Faction.def.techLevel >= TechLevel.Industrial || (ElectricityResearch.Value?.IsFinished ?? false) ||
		      PlayerHasTelevision()) || p.IsSlave ||
		    p.needs?.mood?.thoughts?.memories == null ||
		    !(p.Ideo?.HasPrecept(MemeSuperPackDefOf.MSSMeme_RimRim_Demanded) ?? false) ||
		    p.needs.mood.thoughts.memories.Memories.Exists(m => WatchedRimRimThoughts.Value.Contains(m.def)))
			return ThoughtState.Inactive;
		return ThoughtState.ActiveDefault;
	}

	public static bool CanHaveThought(Pawn pawn) => ModLister.IdeologyInstalled &&
	                                                MemeSuperPackDefOf.MSSMeme_RimRim_Missed.Worker.CurrentState(pawn)
		                                                .Active;
}
