using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack;

public class IncidentWorker_Warcaskets : IncidentWorker
{
	public static Lazy<TraitDef> WarcasketTrait =
		new(() => DefDatabase<TraitDef>.GetNamedSilentFail("VFEP_WarcasketTrait"));

	public static Lazy<ChemicalDef> PsychiteDef =
		new(() => DefDatabase<ChemicalDef>.GetNamedSilentFail("Psychite"));

	public static Lazy<HediffDef> AddictionDef =
		new(() => DefDatabase<HediffDef>.GetNamedSilentFail("PsychiteAddiction"));

	protected virtual bool SendLetter => true;

	protected override bool CanFireNowSub(IncidentParms parms) =>
		WarcasketTrait.Value != null &&
		base.CanFireNowSub(parms) &&
		Find.CurrentMap.mapPawns.AllPawnsSpawned.Exists(p => p.story?.traits?.HasTrait(WarcasketTrait.Value) ?? false);

	protected override bool TryExecuteWorker(IncidentParms parms)
	{
		Map target = (Map)parms.target;
		List<Pawn> pawns = new();
		foreach (Pawn pawn in Find.CurrentMap.mapPawns.AllPawnsSpawned.Where(p =>
			         p.story?.traits?.HasTrait(WarcasketTrait.Value) ?? false))
		{
			Verse.Hediff addictionHediff = AddictionUtility.FindAddictionHediff(pawn, PsychiteDef.Value);
			if (addictionHediff == null)
			{
				addictionHediff = HediffMaker.MakeHediff(AddictionDef.Value, pawn);
				pawn.health.AddHediff(addictionHediff);
				pawns.Add(pawn);
			}

			if (addictionHediff.Severity < 0.69f) addictionHediff.Severity = 0.69f;
		}

		if (SendLetter && pawns.Count > 0) SendStandardLetter(parms, new LookTargets(pawns));
		return pawns.Count > 0;
	}
}
