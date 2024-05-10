using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack
{
	public class CompSpawnerFire : ThingComp
	{
		private Stack<IntVec3> _priorLocations = new();
		private IntVec3 _priorLocation;

		private CompProperties_SpawnerFire Props => (CompProperties_SpawnerFire)props;

		private bool CanSpawnFire => this.parent is not Hive parentThing || parentThing.CompDormant.Awake;

		public override void CompTick()
		{
			base.CompTick();
			if (parent.Position != _priorLocation)
			{
				_priorLocation = parent.Position;
				_priorLocations.Push(_priorLocation);
			}

			if (!parent.IsHashIntervalTick(Props.spawnTicks) || !CanSpawnFire) return;
			TrySpawnFire();
		}

		public void TrySpawnFire()
		{
			IntVec3 result = IntVec3.Invalid;
			if (parent.Map == null || (!Props.behindOnly && !CellFinder.TryFindRandomReachableNearbyCell(parent.Position,
				    parent.Map,
				    Props.spawnRadius, TraverseParms.For(TraverseMode.PassAllDestroyableThings),
				    x => x.TerrainFlammableNow(parent.Map), x => true, out result)))
				return;
			if (Props.behindOnly)
			{
				int pops = Math.Min(_priorLocations.Count - 1, (int)Props.spawnRadius);
				while (pops > 0)
				{
					result = _priorLocations.Pop();
					pops--;
				}
			}

			if (result != IntVec3.Invalid) FireUtility.TryStartFireIn(result, parent.Map, Props.fireSize, parent);
		}
	}
}
