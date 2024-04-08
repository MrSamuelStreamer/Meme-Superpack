using System.Collections.Generic;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack.Hediff;

public class HediffCompProperties_SkillIssue : HediffCompProperties
{
	public Dictionary<SkillDef, int> skillLosses = new();
	public int defaultSkillLoss = 5;
	public bool allSkills = false;

	public HediffCompProperties_SkillIssue() => compClass = typeof(HediffComp_SkillIssue);
}