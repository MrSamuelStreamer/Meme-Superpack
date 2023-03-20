using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack
{
	public class CompStockpileAffinity : ThingComp
	{
		private int _nextMove;
		private Stack<IntVec3> _stockpileLocations = new();

		private CompProperties_StockpileAffinity Props => (CompProperties_StockpileAffinity)props;

		private bool CanMove => parent is Pawn pawn && pawn.Awake() &&
		                        (pawn.mindState.exitMapAfterTick <= 0 ||
		                         GenTicks.TicksGame < pawn.mindState.exitMapAfterTick);

		public override void CompTick()
		{
			base.CompTick();
			if (GenTicks.TicksGame < _nextMove || !CanMove) return;
			TryMoveToStockpile();
			_nextMove = GenTicks.TicksGame + Props.lingerTicks;
		}

		public void TryMoveToStockpile()
		{
			if (parent is not Pawn pawn || (_stockpileLocations.Count == 0 && !PopulateStockpiles())) return;
			IntVec3 cell = CellFinder.RandomClosewalkCellNear(_stockpileLocations.Pop(), parent.Map, 2);
			pawn.mindState.forcedGotoPosition = cell;
		}

		public bool PopulateStockpiles()
		{
			var stockpileLocations = Building_OrbitalTradeBeacon.AllPowered(parent.Map)
				.Select(b => b.TradeableCells.RandomElementWithFallback(b.Position)).ToList();
			stockpileLocations.AddRange(parent.Map.zoneManager.AllZones.Where(z =>
					z is Zone_Stockpile s && s.AllContainedThings.Where(t => t.FlammableNow).Sum(t => t.MarketValue) > 200f)
				.Select(z => z.Cells.RandomElementWithFallback(z.Position)));
			stockpileLocations.Shuffle();
			stockpileLocations.ForEach(_stockpileLocations.Push);
			return stockpileLocations.Count > 0;
		}
	}
}
