using MSS.MemeSuperpack.Comps;
using RimWorld;
using Verse;
using Verse.AI;

namespace MSS.MemeSuperpack.Jobs;

public class JobGiver_EmergencyMeeting : ThinkNode_JobGiver
{
	protected override Job TryGiveJob(Pawn pawn)
	{
		CompImpostor compImpostor = pawn.GetComp<CompImpostor>();
		if (compImpostor is not { IsSus: true })
			return null;

		Ability ability = pawn.abilities?.GetAbility(MemeSuperPackDefOf.MSSMeme_EmergencyMeeting);

		if (ability is null || !ability.CanCast)
			return null;

		LocalTargetInfo target = pawn.Position;
		return !target.IsValid ? null : ability.GetJob(target, target);
	}
}
