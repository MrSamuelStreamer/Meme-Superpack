using System.Linq;
using MSS.MemeSuperpack.Comps;
using MSS.MemeSuperpack.ModExtensions;
using RimWorld;
using Verse;
using Verse.AI;

namespace MSS.MemeSuperpack.Jobs;

public class JobGiver_UseVent : ThinkNode_JobGiver
{
	public VentModExtension Extension =>
		MemeSuperPackDefOf.MSSMeme_UseVent.GetModExtension<VentModExtension>();

	protected override Job TryGiveJob(Pawn pawn)
	{
		CompImpostor compImpostor = pawn.GetComp<CompImpostor>();
		if (compImpostor is not { IsSus: true })
			return null;

		Thing vent = pawn
			.Map?.listerThings.AllThings.Where(t => Extension.VentableThings.Contains(t.def))
			.Where(t => t.Position.InAllowedArea(pawn))
			.RandomElementWithFallback();

		if (vent != null)
		{
			return JobMaker.MakeJob(MemeSuperPackDefOf.MSSMeme_UseVent, vent);
		}

		return null;
	}
}
