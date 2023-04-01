using RimWorld;
using UnityEngine;
using Verse;

namespace MSS.MemeSuperpack.Hediff;

public class Hediff_WakeTime : HediffWithComps
{
	public int lastSleepTick = -1;

	public override void Tick()
	{
		base.Tick();
		if (Find.TickManager.TicksGame % 1500 != 0) return;
		if (!pawn.Awake())
		{
			lastSleepTick = Find.TickManager.TicksGame;
			Severity = 0;
		}
		else
		{
			// ReSharper disable once PossibleLossOfFraction
			Severity = (Find.TickManager.TicksGame - lastSleepTick) / 2500;
		}
	}

	public override string LabelInBrackets => $"{(Find.TickManager.TicksGame - lastSleepTick) / 2500} Hours";
	public override bool ShouldRemove => false;
	public override bool Visible => pawn.Awake() && lastSleepTick > 0;
	public override string Description => CurStage.label;

	public override Color LabelColor =>
		CurStageIndex switch
		{
			0 => Color.white,
			1 => Color.yellow,
			_ => Color.red
		};

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref lastSleepTick, "lastSleepTick", -1);
	}
}
