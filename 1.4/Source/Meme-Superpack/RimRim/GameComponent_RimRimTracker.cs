using RimWorld;
using Verse;

namespace MSS.MemeSuperpack.RimRim;

public class GameComponent_RimRimTracker : GameComponent
{
	private static int _nextPossibleLetterTick = 0;

	public override void GameComponentTick()
	{
		base.GameComponentTick();
		var ticksGame = Find.TickManager.TicksGame;
		if (_nextPossibleLetterTick > ticksGame || ticksGame % 1000 != 0 ||
		    GenLocalDate.HourOfDay(Find.CurrentMap) != 16) return;
		_nextPossibleLetterTick = ticksGame + 55000; // 22H
		if (!ModLister.IdeologyInstalled ||
		    Find.FactionManager.OfPlayer.ideos.GetPrecept(MemeSuperPackDefOf.MSSMeme_RimRim_Demanded) == null) return;
		Find.LetterStack.ReceiveLetter(LetterMaker.MakeLetter("Where RimRim!",
			"It's 16:00, where RimRim!\n\nAny RimRims currently under your control may throw a tantrum if they don't get their daily RimRim soon.\n\nYes, this is expected every single day with no breaks for illness, holiday or sewage leaking into your house. Get used to it.",
			LetterDefOf.NegativeEvent));
	}

	public GameComponent_RimRimTracker(Game game)
	{
	}
}
