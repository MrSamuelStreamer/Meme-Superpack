using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack;

public static class StockpileUtility
{
	public static bool FindStockpiles(Map map, [NotNull] ref Stack<IntVec3> stockpileLocations)
	{
		var beaconLocations = Building_OrbitalTradeBeacon.AllPowered(map)
			.Select(b => b.TradeableCells.RandomElementWithFallback(b.Position));
		var zoneLocations = map.zoneManager.AllZones.Where(z =>
				z is Zone_Stockpile s && s.AllContainedThings.Where(t => t.FlammableNow).Sum(t => t.MarketValue) > 200f)
			.Select(z => z.Cells.RandomElementWithFallback(z.Position));
		foreach (IntVec3 cell in beaconLocations.Concat(zoneLocations)) stockpileLocations.Push(cell);
		return stockpileLocations.Count > 0;
	}
}
