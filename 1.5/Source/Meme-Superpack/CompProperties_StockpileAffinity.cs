using Verse;

namespace MSS.MemeSuperpack
{
	public class CompProperties_StockpileAffinity : CompProperties
	{
		public int lingerTicks = 20000;

		public CompProperties_StockpileAffinity() => compClass = typeof(CompStockpileAffinity);
	}
}
