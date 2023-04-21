using System.Linq;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack;

public class CompBuildingAura : ThingComp
{
	private IntVec3 _nextPossibleBuildPosition = IntVec3.Invalid;

	private CompProperties_BuildingAura Props => (CompProperties_BuildingAura)props;

	public override void CompTick()
	{
		base.CompTick();
		if (!MemeSuperpackMod.settings.buildingAura || parent.Position == _nextPossibleBuildPosition ||
		    Find.TickManager.TicksGame % Props.spawnTicks != 0 ||
		    !parent.Position.IsValid) return;
		TryPlace();
		UpdateLocation();
	}

	public virtual void UpdateLocation()
	{
		_nextPossibleBuildPosition = parent.Position;
	}

	public virtual void TryPlace()
	{
		if (!_nextPossibleBuildPosition.IsValid || !_nextPossibleBuildPosition.Standable(parent.MapHeld) ||
		    _nextPossibleBuildPosition.GetFirstBuilding(parent.MapHeld) != null ||
		    _nextPossibleBuildPosition.GetThingList(parent.MapHeld).Any(t => t.def.IsBlueprint)) return;
		ThingDef stuff = Props.stuff.RandomElement();
		ThingDef def = Props.buildables.Where(b =>
				GenConstruct.CanBuildOnTerrain(b, _nextPossibleBuildPosition, parent.Map, Rot4.Random,
					stuffDef: b.MadeFromStuff ? stuff : null))
			.RandomElement();

		if (Props.blueprint)
		{
			GenConstruct.PlaceBlueprintForBuild(def, _nextPossibleBuildPosition, parent.Map, Rot4.Random, Faction.OfPlayer,
				stuff);
		}
		else
		{
			GenSpawn.Spawn(ThingMaker.MakeThing(def, stuff), _nextPossibleBuildPosition, parent.Map, Rot4.Random);
		}
	}
}
