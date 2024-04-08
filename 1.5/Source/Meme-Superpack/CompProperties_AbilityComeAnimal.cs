using System.Collections.Generic;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack
{
	public class CompProperties_AbilityComeAnimal : CompProperties_AbilityEffectWithDuration
	{
		public List<PawnKindDef> possiblePawnKinds;
		public int pawnsStayTicksMin = 90000;
		public int pawnsStayTicksMax = 150000;
		public float manhunterChance = 0.2f;
		public bool fightWithSummoner = false;
		public bool fightTarget = true;

		public List<PawnKindDef> PossiblePawnKinds => (possiblePawnKinds?.Count ?? 0) > 0
			? possiblePawnKinds
			: MemeSuperpackMod.CECompatiblePawnKinds();

		public PawnKindDef PawnKind()
		{
			return PossiblePawnKinds.RandomElementByWeight(pkd =>
				pkd.GetModExtension<CECompatibility>()?.CECompatibilityRating ?? pkd.RaceProps?.wildness ?? 0.1f);
		}
	}
}
