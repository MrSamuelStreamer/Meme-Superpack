using System;
using MSS.MemeSuperpack;
using MSSMeme;
using RimWorld;
using Verse;
using Verse.AI;

namespace MSS.MemeSuperpack;

public class MSSMeme_FloatMenuOptionProvider_Extract : FloatMenuOptionProvider
{
	protected override bool Drafted => true;

	protected override bool Undrafted => true;

	protected override bool Multiselect => false;

	protected override bool RequiresManipulation => true;

	public override bool SelectedPawnValid(Pawn pawn, FloatMenuContext context)
	{
		return base.SelectedPawnValid(pawn, context)
			&& (pawn.Downed || (pawn.guilt?.IsGuilty ?? false));
	}

	protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
	{
		if (!clickedPawn.Downed && !clickedPawn.guilt.IsGuilty)
		{
			return null;
		}

		if (!context.FirstSelectedPawn.CanReach(clickedPawn, PathEndMode.ClosestTouch, Danger.Deadly))
		{
			return new FloatMenuOption(
				"MSSMeme_CannotExtract".Translate((NamedArgument)clickedPawn)
					+ ": "
					+ "NoPath".Translate().CapitalizeFirst(),
				null
			);
		}

		if (!MSSMeme_PawnFlyerBalloon.BedAvailableFor(clickedPawn, out Building_Bed _))
		{
			return new FloatMenuOption(
				"MSSMeme_CannotExtract".Translate((NamedArgument)clickedPawn)
					+ ": "
					+ "MSSMeme_NoBed".Translate().CapitalizeFirst(),
				null
			);
		}

		return FloatMenuUtility.DecoratePrioritizedTask(
			new FloatMenuOption(
				"MSSMeme_Extract".Translate((NamedArgument)clickedPawn),
				(Action)(
					() =>
					{
						clickedPawn.SetForbidden(false, false);
						Job job = JobMaker.MakeJob(MemeSuperPackDefOf.MSSMeme_ExtractTarget, clickedPawn);
						job.count = 1;
						context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job);
					}
				)
			),
			context.FirstSelectedPawn,
			clickedPawn
		);
	}
}
