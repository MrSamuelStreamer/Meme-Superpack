using RimWorld;
using Verse;

namespace MSS.MemeSuperpack.Waifu;

public class ThoughtWorker_Precept_IsWaifu_Social : ThoughtWorker_Precept_Social
{
	protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn) =>
		ModsConfig.IdeologyActive && otherPawn.IsWaifuFor(p) ? ThoughtState.ActiveDefault : ThoughtState.Inactive;
}
