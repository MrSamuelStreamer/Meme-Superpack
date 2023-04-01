using System;
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

	public static bool FindStockpileWithItem(Map map, ThingDef thingDef, int minInZone, out Zone stockpile)
	{
		var bestStockpile = map.zoneManager.AllZones.Select(z =>
				new Tuple<int, Zone>(
					z is Zone_Stockpile s
						? s.AllContainedThings
							.Where(t => t.def == thingDef)
							.Sum(t => t.stackCount)
						: 0, z))
			.MaxByWithFallback(t => t.Item1, null);
		stockpile = (bestStockpile?.Item1 ?? 0) >= minInZone ? bestStockpile?.Item2 : null;
		return stockpile != null;
	}
}
