using RimWorld;
using Verse;

namespace MSS.MemeSuperpack.Comps;

public class CompProperties_DirtHaver : CompProperties
{
	public bool dirtExpires = true;
	public float dirtExpireTime = GenDate.TicksPerDay;

	public CompProperties_DirtHaver()
	{
		compClass = typeof(CompDirtHaver);
	}
}
