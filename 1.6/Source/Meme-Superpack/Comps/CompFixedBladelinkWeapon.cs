using System;
using System.Reflection;
using HarmonyLib;
using RimWorld;

namespace MSS.MemeSuperpack.Comps;

public class CompFixedBladelinkWeapon : CompBladelinkWeapon
{
	public static Lazy<FieldInfo> traits = new(() =>
		AccessTools.Field(typeof(CompBladelinkWeapon), "traits")
	);
	public new CompProperties_FixedBladelinkWeapon Props =>
		props as CompProperties_FixedBladelinkWeapon;

	public override void PostPostMake()
	{
		traits.Value.SetValue(this, Props.traits);
	}
}
