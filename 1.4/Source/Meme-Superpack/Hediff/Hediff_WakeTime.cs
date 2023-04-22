using HarmonyLib;
using UnityEngine;
using Verse;

namespace MSS.MemeSuperpack.Hediff;

public class Hediff_WakeTime : HediffWithComps
{
	private Traverse<int> lastRestTick;

	public int LastRestTick
	{
		get
		{
			if (pawn.needs?.rest == null) lastRestTick = null;
			if (lastRestTick == null && pawn.needs?.rest is { } rest)
				lastRestTick = Traverse.Create(rest).Field<int>("lastRestTick");
			return lastRestTick?.Value ?? -999;
		}
	}

	public override void Tick()
	{
		base.Tick();
		if (Find.TickManager.TicksGame % 1500 != 0 || pawn.needs?.rest is not { } rest) return;
		if (rest.Resting)
		{
			Severity = 0;
		}
		else
		{
			// ReSharper disable once PossibleLossOfFraction
			Severity = (float)rest.CurCategory;
		}
	}

	public override void PostRemoved()
	{
		base.PostRemoved();
		lastRestTick = null;
	}

	public override string LabelInBrackets =>
		LastRestTick > 0 ? $"{(Find.TickManager.TicksGame - LastRestTick) / 2500} Hours" : null;

	public override bool ShouldRemove => pawn.needs?.rest == null;
	public override bool Visible => pawn.needs?.rest?.Resting == false;
	public override string Description => CurStage.label;

	public override Color LabelColor =>
		CurStageIndex switch
		{
			0 => Color.green,
			1 => Color.white,
			2 => Color.yellow,
			_ => Color.red
		};
}
