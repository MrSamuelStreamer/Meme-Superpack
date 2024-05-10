using System.Linq;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack.Hediff;

public class HediffComp_SkillIssue : HediffComp
{
	public HediffCompProperties_SkillIssue Props => (HediffCompProperties_SkillIssue)props;


	public override void CompPostPostAdd(DamageInfo? dinfo)
	{
		foreach (SkillRecord skill in Pawn.skills.skills.Where(s =>
			         Props.allSkills || Props.skillLosses.ContainsKey(s.def)))
		{
			skill.levelInt -= Props.skillLosses.GetWithFallback(skill.def, Props.defaultSkillLoss);
		}
	}

	public override void CompPostPostRemoved()
	{
		foreach (SkillRecord skill in Pawn.skills.skills.Where(s =>
			         Props.allSkills || Props.skillLosses.ContainsKey(s.def)))
		{
			skill.levelInt += Props.skillLosses.GetWithFallback(skill.def, Props.defaultSkillLoss);
		}
	}

	public override string CompDescriptionExtra => "\n\nSkill Issues:\n" +
		Pawn.skills.skills
			.Where(s => Props.allSkills || Props.skillLosses.ContainsKey(s.def))
			.Select(s => $"\t{s.def.LabelCap}:\t{Props.skillLosses.GetWithFallback(s.def, Props.defaultSkillLoss)}")
			.ToLineList();
}
