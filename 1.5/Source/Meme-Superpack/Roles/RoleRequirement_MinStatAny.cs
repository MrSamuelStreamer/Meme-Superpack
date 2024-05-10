using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack.Roles;

public class RoleRequirement_MinStatAny : RoleRequirement
{
	public List<StatModifier> stats;
	private string labelCached;

	public override string GetLabel(Precept_Role role)
	{
		if (labelCached == null)
			labelCached = stats.Count != 1
				? "MSSMeme_RoleRequirementStatAny".Translate() + ": " + stats.Select(GetStatStr).ToCommaList()
				: (string)("MSSMeme_RoleRequirementStat".Translate() + ": " + GetStatStr(stats[0]));
		return labelCached;

		static string GetStatStr(StatModifier requirement) => requirement.stat.LabelCap + " " + requirement.value;
	}

	public override bool Met(Pawn pawn, Precept_Role role) =>
		stats.Exists(minStat => pawn.GetStatValue(minStat.stat) >= minStat.value);
}
