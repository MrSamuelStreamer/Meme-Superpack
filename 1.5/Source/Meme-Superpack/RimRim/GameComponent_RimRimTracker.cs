using System.Linq;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack.RimRim;

public class GameComponent_RimRimTracker : GameComponent
{
	private static int _nextPossibleLetterTick = 0;

	public override void GameComponentTick()
	{
		base.GameComponentTick();
		if (!MemeSuperpackMod.settings.whereRimRim) return;
		var ticksGame = Find.TickManager.TicksGame;
		if (_nextPossibleLetterTick > ticksGame || ticksGame % 1000 != 0 ||
		    GenLocalDate.HourOfDay(Find.CurrentMap) != 18) return;
		_nextPossibleLetterTick = ticksGame + 55000; // 22H
		if (!ModLister.IdeologyInstalled ||
		    Find.FactionManager.OfPlayer.ideos.GetPrecept(MemeSuperPackDefOf.MSSMeme_RimRim_Demanded) == null ||
		    !Find.CurrentMap.PlayerPawnsForStoryteller.Any(p =>
			    (p.ideo?.Ideo?.HasPrecept(MemeSuperPackDefOf.MSSMeme_RimRim_Demanded) ?? false) &&
			    (MemeSuperPackDefOf.MSSMeme_RimRim_Missed.Worker?.CurrentState(p).Active ?? false))) return;
		Find.LetterStack.ReceiveLetter(LetterMaker.MakeLetter("MSSMeme_WhereRimRimLetter".TranslateSimple(),
			"MSSMeme_WhereRimRimLetterText".Translate(),
			LetterDefOf.NegativeEvent));
	}

	public GameComponent_RimRimTracker(Game game)
	{
	}
}
