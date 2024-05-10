using System.Linq;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack.Waifu;

public class ThoughtWorker_Precept_Waifu_Present : ThoughtWorker_Precept
{
	protected override ThoughtState ShouldHaveThought(Pawn p) =>
		!ModsConfig.IdeologyActive || (p.Ideo?.RequiresWaifu() == false || p.Ideo.GetWaifu() == p)
			? ThoughtState.Inactive
			: (p.MapHeld?.mapPawns?.AllPawnsSpawned?.Any(otherPawn => otherPawn.IsWaifuFor(p)) ?? false)
				? ThoughtState.ActiveAtStage(1)
				: ThoughtState.ActiveAtStage(0, "No waifus on the map");
}
