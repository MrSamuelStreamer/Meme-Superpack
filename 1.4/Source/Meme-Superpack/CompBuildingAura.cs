using System.Linq;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack;

public class CompBuildingAura : ThingComp
{
	private IntVec3 _priorLocation = IntVec3.Invalid;

	private CompProperties_BuildingAura Props => (CompProperties_BuildingAura)props;

	public override void CompTick()
	{
		base.CompTick();
		if (!MemeSuperpackMod.settings.buildingAura || parent.Position == _priorLocation ||
		    Find.TickManager.TicksGame % Props.spawnTicks != 0 ||
		    !parent.Position.IsValid) return;
		if (_priorLocation.IsValid)
		{
			ThingDef stuff = Props.stuff.RandomElement();
			ThingDef def = Props.buildables.Where(b =>
					GenConstruct.CanBuildOnTerrain(b, _priorLocation, parent.Map, Rot4.Random,
						stuffDef: b.MadeFromStuff ? stuff : null))
				.RandomElement();

			if (Props.blueprint)
			{
				GenConstruct.PlaceBlueprintForBuild(def, _priorLocation, parent.Map, Rot4.Random, Faction.OfPlayer, stuff);
			}
			else
			{
				GenSpawn.Spawn(ThingMaker.MakeThing(def, stuff), _priorLocation, parent.Map, Rot4.Random);
			}
		}

		_priorLocation = parent.Position;
	}
}
