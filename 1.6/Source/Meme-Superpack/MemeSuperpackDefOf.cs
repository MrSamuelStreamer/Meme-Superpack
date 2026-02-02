using RimWorld;
using Verse;

namespace MSS.MemeSuperpack
{
	[DefOf]
	public class MemeSuperPackDefOf
	{
		[MayRequireIdeology]
		public static ThoughtDef MSSMeme_RimRim_Watched_Good;

		[MayRequireIdeology]
		public static ThoughtDef MSSMeme_RimRim_Watched_Meh;

		[MayRequireIdeology]
		public static ThoughtDef MSSMeme_RimRim_Watched_Poor;

		[MayRequireIdeology]
		public static ThoughtDef MSSMeme_RimRim_Missed;

		[MayRequireIdeology]
		public static PreceptDef MSSMeme_RimRim_Demanded;

		[MayRequireIdeology]
		public static PreceptDef MSSMeme_IdeoRole_Waifu;

		[MayRequireIdeology]
		public static PreceptDef MSSMeme_RimRim_Ritual;

		[MayRequireIdeology]
		public static HistoryEventDef MSSMeme_WaifuDied;

		public static PawnKindDef MSSMeme_Stickbug;

		public static ThoughtDef MSSMeme_AwakeThought;

		public static SongDef MSSMeme_BuckoDrinkMusic;

		public static HediffDef MSSMeme_YChromosomalAdam;

		public static ThingDef MSSMeme_StickbugIncoming;

		public static readonly XenotypeDef MSSMeme_Taff;
		public static readonly FactionDef MSSMeme_TaffsFaction;

		public static readonly ThoughtDef MSSMeme_Marked;

		[MayRequireIdeology]
		public static TaleDef MSSMeme_WatchedRimRim;

		public static ThingDef MSSMeme_Balloon;
		public static ThingDef MSSMeme_PawnFlyer_Balloon;
		public static JobDef MSSMeme_ExtractTarget;
		public static readonly PawnKindDef MSSMeme_BabyCritter;
		public static readonly ThoughtDef MSSMeme_BabyCannonWTF;
		public static readonly JobDef MSSMeme_UseVent;
		public static readonly JobDef MSSMeme_SusDeconstruct;
		public static readonly JobDef MSSMeme_SusDoBill;
		public static readonly PawnKindDef MSSMeme_MogusKind_Blue;
		public static readonly PawnKindDef MSSMeme_MogusKind_Red;
		public static readonly PawnKindDef MSSMeme_MogusKind_Green;
		public static readonly PawnKindDef MSSMeme_MogusKind_Yellow;
		public static readonly SoundDef MSSMeme_EmergencyMeetingKlaxon;
		public static readonly AbilityDef MSSMeme_EmergencyMeeting;
		public static readonly ThinkTreeDef MSSMeme_ImpostorBehavior;
		public static readonly ThingDef MSSMeme_Mogus;
		public static readonly PawnKindDef MSSMeme_Dirtman;
		public static readonly JobDef MSSMeme_PutALittleDirtUnderThePillow;
		public static readonly WorkTypeDef MSSMeme_AnomalyPrevention;
		public static readonly ResearchProjectDef MSSMeme_Oskarian_Technology;
		public static readonly HediffDef MSSMeme_WellSlept;
		public static readonly RulePackDef MSSMeme_Nonsense;
		public static readonly TaleDef MSSMeme_Nonsense_Tale;
		public static readonly ThoughtDef MSSMeme_Nonsense_Thought_Bad;
		public static readonly ThoughtDef MSSMeme_Nonsense_Thought_Neutral;
		public static readonly ThoughtDef MSSMeme_Nonsense_Thought_Good;

		static MemeSuperPackDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MemeSuperPackDefOf));
		}
	}
}
