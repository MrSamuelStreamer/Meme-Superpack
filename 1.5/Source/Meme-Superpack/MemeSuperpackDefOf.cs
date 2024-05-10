using RimWorld;
using Verse;

namespace MSS.MemeSuperpack
{
	[DefOf]
	public class MemeSuperPackDefOf
	{
		[MayRequireIdeology] public static ThoughtDef MSSMeme_RimRim_Watched_Good;
		[MayRequireIdeology] public static ThoughtDef MSSMeme_RimRim_Watched_Meh;
		[MayRequireIdeology] public static ThoughtDef MSSMeme_RimRim_Watched_Poor;
		[MayRequireIdeology] public static ThoughtDef MSSMeme_RimRim_Missed;

		[MayRequireIdeology] public static PreceptDef MSSMeme_RimRim_Demanded;
		[MayRequireIdeology] public static PreceptDef MSSMeme_IdeoRole_Waifu;

		[MayRequireIdeology] public static HistoryEventDef MSSMeme_WaifuDied;

		public static PawnKindDef MSSMeme_Stickbug;

		public static ThoughtDef MSSMeme_AwakeThought;

		public static SongDef MSSMeme_BuckoDrinkMusic;

		public static HediffDef MSSMeme_YChromosomalAdam;

		public static ThingDef MSSMeme_StickbugIncoming;

		[MayRequireIdeology] public static TaleDef MSSMeme_WatchedRimRim;

		static MemeSuperPackDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MemeSuperPackDefOf));
		}
	}
}
