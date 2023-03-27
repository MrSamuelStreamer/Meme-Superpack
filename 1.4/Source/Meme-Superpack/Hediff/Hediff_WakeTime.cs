using RimWorld;
using Verse;

namespace MSS.MemeSuperpack.Hediff;

public class Hediff_WakeTime : HediffWithComps
{
	public int lastSleepTick = -1;

	public override void PostMake()
	{
		base.PostMake();
		lastSleepTick = -1;
	}

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
			this.CurStage.label = $"{Severity} Hours";
		}
	}

	public override string LabelInBrackets => $"{(Find.TickManager.TicksGame - lastSleepTick) / 2500} Hours";

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref lastSleepTick, "lastSleepTick", -1);
	}
}
