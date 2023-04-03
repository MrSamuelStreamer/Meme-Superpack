using Verse;

namespace MSS.MemeSuperpack;

public class CompTerrainTrail : ThingComp
{
	private IntVec3 _priorLocation = IntVec3.Invalid;

	private CompProperties_TerrainTrail Props => (CompProperties_TerrainTrail)props;

	public override void CompTick()
	{
		base.CompTick();
		if (parent.Position == _priorLocation || Find.TickManager.TicksGame % Props.spawnTicks != 0 ||
		    !parent.Position.IsValid) return;
		_priorLocation = parent.Position;
		parent.Map.terrainGrid.SetTerrain(_priorLocation, Props.terrain);
	}
}
