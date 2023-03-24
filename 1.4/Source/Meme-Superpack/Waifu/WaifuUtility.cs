using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack.Waifu;

public static class WaifuUtility
{
	public static bool IsWaifuFor(this Pawn possibleWaifu, Pawn pawn) =>
		pawn.Ideo?.GetPrecept(MemeSuperPackDefOf.MSSMeme_IdeoRole_Waifu) is Precept_Role precept &&
		precept.ChosenPawnSingle() == possibleWaifu;

	public static bool RequiresWaifu(this Ideo ideo) => ideo.HasPrecept(MemeSuperPackDefOf.MSSMeme_IdeoRole_Waifu);

	[CanBeNull]
	public static Pawn GetWaifu(this Ideo ideo) =>
		ideo?.GetPrecept(MemeSuperPackDefOf.MSSMeme_IdeoRole_Waifu) is Precept_Role precept
			? precept.ChosenPawnSingle()
			: null;
}
