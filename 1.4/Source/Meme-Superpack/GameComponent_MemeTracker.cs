using System;
using RimWorld;
using Verse;
using Verse.AI.Group;

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
	public bool grignrAttacked = false;

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

		var ticksGame = Find.TickManager.TicksGame;

		// Check 3 times a day
		if (ticksGame % 20000 != 0) return;

		// Start gaslighting 1% chance
		if (CurrentGaslightingTopic == GaslightingTopic.None && Rand.Chance(0.01f) &&
		    (Enum.TryParse(Enum.GetNames(typeof(GaslightingTopic)).RandomElement(), out CurrentGaslightingTopic) &&
		     CurrentGaslightingTopic != GaslightingTopic.None)) StartGasLighting();

		if (!grignrAttacked && Rand.Chance(0.001f))
		{
			GrignrAttack();
		}

		// Disable current topic if running for at least 3 days 10% chance
		if (CurrentGaslightingTopic == GaslightingTopic.None || ticksGame <= GaslightingLastStartTick + 180000 ||
		    !Rand.Chance(0.1f)) return;
		EndGaslighting();
	}

	public void GrignrAttack()
	{
		PawnKindDef grignrType = DefDatabase<PawnKindDef>.GetNamedSilentFail("Taggerung_ShardOfGrignr");
		if (grignrType == null) return;
		grignrAttacked = true;
		Faction faction = Find.FactionManager.RandomEnemyFaction(allowNonHumanlike: false, allowHidden: true);
		Pawn grignr = PawnGenerator.GeneratePawn(grignrType, faction);
		Map currentMap = Find.AnyPlayerHomeMap;
		RCellFinder.TryFindRandomPawnEntryCell(out IntVec3 loc, currentMap, 0.8f, true);
		GenSpawn.Spawn(grignr, loc, currentMap, Rot4.Random);

		LordJob lordJob = new LordJob_AssaultColony(faction, false, false, false, false, false, false, true);
		RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith(v => v.Standable(currentMap), currentMap,
			out grignr.mindState.forcedGotoPosition);

		Lord lord = LordMaker.MakeNewLord(faction, lordJob, currentMap, new[] { grignr });
		grignr.guest.Recruitable = true;
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
