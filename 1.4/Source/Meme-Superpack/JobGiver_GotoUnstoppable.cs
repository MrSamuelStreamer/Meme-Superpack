using RimWorld;
using Verse;
using Verse.AI;

namespace MSS.MemeSuperpack
{
	public class JobGiver_GotoUnstoppable : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 forcedGotoPosition = pawn.mindState.forcedGotoPosition;
			if (!forcedGotoPosition.IsValid)
				return null;
			using (PawnPath path = pawn.Map.pathFinder.FindPath(pawn.Position, forcedGotoPosition,
				       TraverseParms.For(pawn, mode: TraverseMode.PassAllDestroyableThings)))
			{
				IntVec3 cellBefore;
				Thing blocker = path.FirstBlockingBuilding(out cellBefore, pawn);
				if (blocker != null)
				{
					Job job = DigUtility.PassBlockerJob(pawn, blocker, cellBefore, true, true);
					if (job != null)
						return job;
				}
			}

			Job jobGoto = JobMaker.MakeJob(JobDefOf.Goto, forcedGotoPosition);
			jobGoto.expiryInterval = 10000;
			jobGoto.locomotionUrgency = LocomotionUrgency.Walk;
			jobGoto.canBashDoors = true;
			jobGoto.canBashFences = true;
			return jobGoto;
		}
	}
}
