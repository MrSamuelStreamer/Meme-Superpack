using RimWorld;
using Verse;

namespace MSS.MemeSuperpack.Roles;

public class RoleRequirement_SpecificGender : RoleRequirement
{
	public Gender gender;

	public override string GetLabel(Precept_Role role) => labelKey.Translate((NamedArgument)gender.GetLabel());

	public override bool Met(Pawn pawn, Precept_Role role) => pawn.gender == gender;
}
