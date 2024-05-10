using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace MSS.MemeSuperpack
{
	public class MentalState_KickANuke : MentalState_Tantrum
	{
		public Stack<Thing> kickables = new();

		public override void MentalStateTick()
		{
			if (target == null || target.Destroyed)
				RecoverFromState();
			else if (!target.Spawned ||
			         !pawn.CanReach((LocalTargetInfo)target, PathEndMode.Touch, Danger.Deadly))
			{
				if (!TryFindNewTarget())
				{
					RecoverFromState();
				}
				else
				{
					Messages.Message(
						"MessageTargetedTantrumChangedTarget".Translate((NamedArgument)pawn.LabelShort,
							(NamedArgument)target.Label, (NamedArgument)target.Label, pawn.Named("PAWN"),
							target.Named("OLDTARGET"), target.Named("TARGET")).AdjustedFor(pawn),
						(Thing)pawn, MessageTypeDefOf.NegativeEvent);
					base.MentalStateTick();
				}
			}
			else
				base.MentalStateTick();
		}

		public override void PreStart()
		{
			base.PreStart();
			TryFindNewTarget();
		}

		private bool TryFindNewTarget()
		{
			if (kickables.Count <= 0 && SmashableUtility.GetSmashableThingsNear(pawn, pawn.Position, kickables,
				    customValidator: thing => Nukes.NukeDefs.Value.Contains(thing.def)) <= 0) return false;
			while (kickables.Count > 0 && (target == null || target.Destroyed || !target.Spawned ||
			                               !pawn.CanReach((LocalTargetInfo)target, PathEndMode.Touch, Danger.Deadly)))
			{
				target = kickables.Pop();
			}

			return target is { Destroyed: false, Spawned: true } &&
			       pawn.CanReach((LocalTargetInfo)target, PathEndMode.Touch, Danger.Deadly);
		}

		public override TaggedString GetBeginLetterText()
		{
			if (target != null)
				return (TaggedString)def.beginLetter
					.Formatted(pawn.NameShortColored, (NamedArgument)target.Label,
						pawn.Named("PAWN"), target.Named("TARGET")).AdjustedFor(pawn).Resolve().CapitalizeFirst();
			Verse.Log.Error("No target. This should have been checked in this mental state's worker.");
			return (TaggedString)"";
		}
	}

	public class MentalStateWorker_KickANuke : MentalStateWorker
	{
		public override bool StateCanOccur(Pawn pawn)
		{
			if (!MemeSuperpackMod.settings.kickNukes || !base.StateCanOccur(pawn)) return false;
			return SmashableUtility.GetSmashableThingsNear(pawn, pawn.Position, new Stack<Thing>(),
				customValidator: thing => Nukes.NukeDefs.Value.Contains(thing.def)) > 0;
		}
	}

	public static class SmashableUtility
	{
		public static int GetSmashableThingsNear(
			Pawn pawn,
			IntVec3 near,
			Stack<Thing> outCandidates,
			Predicate<Thing> customValidator = null,
			int extraMinBuildingOrItemMarketValue = 0,
			int maxDistance = 50)
		{
			outCandidates.Clear();
			if (!pawn.CanReach(near, PathEndMode.OnCell, Danger.Deadly) || near.GetRegion(pawn.Map) is not { } region)
				return 0;

			TraverseParms traverseParams = TraverseParms.For(pawn);

			bool RegionProcessor(Region r)
			{
				var thingList = r.ListerThings.ThingsInGroup(ThingRequestGroup.HaulableEver);
				foreach (Thing thing in thingList.Where(t =>
					         t.Position.InHorDistOf(near, maxDistance) &&
					         TantrumMentalStateUtility.CanSmash(pawn, t, true, customValidator,
						         extraMinBuildingOrItemMarketValue)))
				{
					outCandidates.Push(thing);
				}

				return false;
			}

			RegionTraverser.BreadthFirstTraverse(region, (_, to) => to.Allows(traverseParams, false), RegionProcessor,
				maxDistance);
			return outCandidates.Count;
		}
	}
}
