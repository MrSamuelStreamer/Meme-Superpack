using System.Collections.Generic;
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
			return StockpileUtility.FindStockpiles(parent.Map, ref _stockpileLocations);
		}
	}
}
