using RimWorld;
using Verse;

namespace MSS.MemeSuperpack.Hediff;

public class HediffGiver_EnsureHediff : HediffGiver
{
	public NeedDef requiredNeed = null;

	public override void OnIntervalPassed(Pawn pawn, Verse.Hediff cause)
	{
		if (requiredNeed != null && pawn.needs?.TryGetNeed(requiredNeed) is null ) return;
		Verse.Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(hediff);
		if (firstHediffOfDef == null) TryApply(pawn);
	}
}
