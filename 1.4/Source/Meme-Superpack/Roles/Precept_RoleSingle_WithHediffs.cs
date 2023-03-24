using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack.Roles;

public class Precept_RoleSingle_WithBasicHediffs : Precept_RoleSingle
{
	public List<HediffDef> hediffs;

	public override void Assign(Pawn p, bool addThoughts)
	{
		Pawn priorPawn = ChosenPawnValue;
		base.Assign(p, addThoughts);
		if (priorPawn == ChosenPawnValue || (hediffs?.Count ?? 0) <= 0) return;
		if (priorPawn != null)
		{
			var hediffSet = hediffs.ToHashSet();
			var priorPawnHediffs = new List<Hediff>();
			priorPawn.health.hediffSet.GetHediffs(ref priorPawnHediffs, h => hediffSet.Contains(h.def));
			priorPawnHediffs.ForEach(h => priorPawn.health.RemoveHediff(h));
		}

		hediffs.ForEach(h => p.health.AddHediff(HediffMaker.MakeHediff(h, p)));
	}
}
