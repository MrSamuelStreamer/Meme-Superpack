using RimWorld;
using Verse;

namespace MSS.MemeSuperpack.MSSFP.Projectiles
{
	/// <summary>
	/// Compatibility version of the plaque bullet projectile that directly references MSSFP
	/// This assembly is only loaded when MSSFP is present
	/// </summary>
	public class MSSMeme_Projectile_PlaqueBullet_Compatibility : Projectile
	{
		protected override void Impact(Thing hitThing, bool blockedByShield = false)
		{
			base.Impact(hitThing, blockedByShield);

			if (hitThing is Pawn targetPawn && !blockedByShield)
				ApplyPlaqueEffect(targetPawn);
		}

		private void ApplyPlaqueEffect(Pawn target)
		{
			if (Rand.Chance(0.01f))
			{
				ApplyAssClosingEffect(target);
				return;
			}

			if (Rand.Chance(0.5f))
				ApplyToothPain(target);
			else
				TransformToSign(target);
		}

		private void ApplyAssClosingEffect(Pawn target)
		{
			var hediff = DefDatabase<HediffDef>.GetNamed("MSS_MendedButtcrack", false);
			if (hediff != null)
			{
				target.health.AddHediff(hediff);
				Messages.Message(
					$"{target.NameShortColored} had their ass closed by the plaque gun!",
					target,
					MessageTypeDefOf.PositiveEvent
				);
			}
		}

		private void ApplyToothPain(Pawn target)
		{
			var hediff = DefDatabase<HediffDef>.GetNamed("MSSMeme_ToothPain", false);
			if (hediff != null)
			{
				target.health.AddHediff(hediff);
				Messages.Message(
					$"{target.NameShortColored} got tooth pain from the plaque gun!",
					target,
					MessageTypeDefOf.NegativeEvent
				);
			}
		}

		private void TransformToSign(Pawn target)
		{
			var map = target.Map;
			var position = target.Position;

			var signDef = DefDatabase<ThingDef>.GetNamed("MSSMeme_PlaqueSign", false);
			if (signDef == null)
			{
				Log.Error($"MSSMeme: Sign definition not found for {target.NameShortColored}");
				return;
			}

			var sign = ThingMaker.MakeThing(signDef);
			sign.SetPositionDirect(position);

			var signComp = sign.TryGetComp<MSS.MemeSuperpack.MSSFP.Comps.MSSMeme_Comp_PlaqueSign_Compatibility>();
			if (signComp == null)
			{
				Log.Error($"MSSMeme: Sign component not found for {target.NameShortColored}");
				return;
			}

			if (!signComp.StorePawn(target))
			{
				Log.Error($"MSSMeme: Failed to store pawn data for {target.NameShortColored}");
				return;
			}

			if (!GenPlace.TryPlaceThing(sign, position, map, ThingPlaceMode.Direct))
			{
				Log.Error($"MSSMeme: Failed to place sign for {target.NameShortColored}");
				return;
			}
		}
	}
}
