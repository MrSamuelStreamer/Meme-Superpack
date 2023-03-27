using Verse;

namespace MSS.MemeSuperpack.Hediff;

public class HediffGiver_EnsureHediff : HediffGiver
{
	public override void OnIntervalPassed(Pawn pawn, Verse.Hediff cause)
	{
		Verse.Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(hediff);
		if (firstHediffOfDef == null) TryApply(pawn);
	}
}
