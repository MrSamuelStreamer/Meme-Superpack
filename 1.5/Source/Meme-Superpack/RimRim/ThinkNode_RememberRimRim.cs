using RimWorld;
using Verse;
using Verse.AI;

namespace MSS.MemeSuperpack.RimRim;

public class ThinkNode_RememberRimRim : ThinkNode
{
	public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
	{
		TaleRecorder.RecordTale(MemeSuperPackDefOf.MSSMeme_WatchedRimRim, pawn);
		return ThinkResult.NoJob;
	}
}
