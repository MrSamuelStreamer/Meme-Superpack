using RimWorld;

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

		public static TaleDef MSSMeme_WatchedRimRim;

		static MemeSuperPackDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MemeSuperPackDefOf));
		}
	}
}
