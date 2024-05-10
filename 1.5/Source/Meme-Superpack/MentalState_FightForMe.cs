using RimWorld;
using Verse;
using Verse.AI;

namespace MSS.MemeSuperpack
{
	public class MentalState_FightForMe : MentalState
	{
		public override bool ForceHostileTo(Thing t) =>
			(t is Pawn targetPawn && (sourceFaction == null
				? GenHostility.IsActiveThreatToPlayer(targetPawn)
				: GenHostility.IsActiveThreatTo(targetPawn, sourceFaction))) ||
			(t.Faction != null && ForceHostileTo(t.Faction));

		public override bool ForceHostileTo(Faction f) => f.HostileTo(sourceFaction ?? Faction.OfPlayer);
	}
}
