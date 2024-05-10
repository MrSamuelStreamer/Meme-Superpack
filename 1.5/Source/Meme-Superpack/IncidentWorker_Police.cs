using System.Linq;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack;

public class IncidentWorker_Police : IncidentWorker_MakeGameCondition
{
	private const int EnsureMinDurationTicks = 5000;

	protected override bool CanFireNowSub(IncidentParms parms) => MemeSuperpackMod.settings.police &&
	                                                              base.CanFireNowSub(parms) && Enumerable.Any(Find.Maps,
		                                                              t => t.IsPlayerHome && !NightWillEndSoon(t));

	private static bool NightWillEndSoon(Map map) => GenCelestial.CurCelestialSunGlow(map) < 0.5 ||
	                                                 GenCelestial.CelestialSunGlow(map,
		                                                 Find.TickManager.TicksAbs + EnsureMinDurationTicks) < 0.5;
}
