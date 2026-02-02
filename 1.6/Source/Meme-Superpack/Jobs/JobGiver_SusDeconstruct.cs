using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace MSS.MemeSuperpack.Jobs;

public class JobGiver_SusDeconstruct : ThinkNode_JobGiver
{
	public IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
	{
		foreach (
			Designation designation in pawn.Map.designationManager.SpawnedDesignationsOfDef(
				DesignationDefOf.Deconstruct
			)
		)
			yield return designation.target.Thing;
	}

	protected override Job TryGiveJob(Pawn pawn)
	{
		if (!pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Deconstruct))
			return null;

		Thing t = PotentialWorkThingsGlobal(pawn).RandomElementWithFallback();

		if (
			t is null
			|| !pawn.CanReserve((LocalTargetInfo)t)
			|| pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) == null
		)
			return null;

		if (t.TryGetComp(out CompExplosive comp) && comp.wickStarted)
			return null;

		Job job = JobMaker.MakeJob(MemeSuperPackDefOf.MSSMeme_SusDeconstruct, (LocalTargetInfo)t);

		return job;
	}
}
