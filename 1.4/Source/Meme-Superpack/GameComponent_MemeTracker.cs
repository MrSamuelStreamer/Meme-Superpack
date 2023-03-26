using System;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack;

public class GameComponent_MemeTracker : GameComponent
{
	public static GameComponent_MemeTracker Instance;

	public enum GaslightingTopic
	{
		None,
		DarkSolar
	}

	public GaslightingTopic CurrentGaslightingTopic = GaslightingTopic.None;
	public int GaslightingLastStartTick = -1;

	public GameComponent_MemeTracker(Game game)
	{
	}

	public override void FinalizeInit()
	{
		base.FinalizeInit();
		Instance = this;
	}

	public override void GameComponentTick()
	{
		base.GameComponentTick();
		// Check 3 times a day
		if (Find.TickManager.TicksGame % 20000 != 0) return;

		// Start gaslighting 1% chance
		if (CurrentGaslightingTopic == GaslightingTopic.None && Rand.Chance(0.01f) &&
		    (Enum.TryParse(Enum.GetNames(typeof(GaslightingTopic)).RandomElement(), out CurrentGaslightingTopic) &&
		     CurrentGaslightingTopic != GaslightingTopic.None)) StartGasLighting();

		// Disable current topic if running for at least 3 days 10% chance
		if (CurrentGaslightingTopic == GaslightingTopic.None || Find.TickManager.TicksGame <= GaslightingLastStartTick + 180000 ||
		    !Rand.Chance(0.1f)) return;
		EndGaslighting();
	}

	public void StartGasLighting(GaslightingTopic forcedTopic = GaslightingTopic.None)
	{
		if (forcedTopic != GaslightingTopic.None) CurrentGaslightingTopic = forcedTopic;
		GaslightingLastStartTick = Find.TickManager.TicksGame;
		Find.LetterStack.ReceiveLetter("MSSMeme_GaslightingStarted".TranslateSimple(),
			"MSSMeme_Gaslighting_Start".Translate(
				$"MSSMeme_GaslightingTopic_{CurrentGaslightingTopic.ToString()}".Translate()), LetterDefOf.NegativeEvent);
	}

	public void EndGaslighting()
	{
		Find.LetterStack.ReceiveLetter("MSSMeme_GaslightingEnded".TranslateSimple(),
			"MSSMeme_Gaslighting_End".Translate(
				$"MSSMeme_GaslightingTopic_{CurrentGaslightingTopic.ToString()}".Translate()), LetterDefOf.PositiveEvent);
		CurrentGaslightingTopic = GaslightingTopic.None;
	}

	public override void StartedNewGame()
	{
	}

	public override void ExposeData()
	{
		Scribe_Values.Look(ref CurrentGaslightingTopic, "CurrentGaslightingTopic", GaslightingTopic.None);
		Scribe_Values.Look(ref GaslightingLastStartTick, "GaslightingLastStartTick", -1);
	}
}
