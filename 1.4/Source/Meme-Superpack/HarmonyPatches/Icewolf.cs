using HarmonyLib;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack.HarmonyPatches;

public class Icewolf
{
	[HarmonyPatch(typeof(IncidentWorker_MeteoriteImpact), "TryFindCell")]
	public static class IceWolfMeteorAffinity
	{
		[HarmonyPrefix]
		public static bool Prefix(ref bool __result, out IntVec3 cell, Map map)
		{
			cell = IntVec3.Invalid;
			Pawn pawn = map.mapPawns.AllPawns.Find(p =>
				p.Name?.ToStringFull?.ToLowerInvariant().Contains("icewo") ?? false);

			if (pawn != null && MemeSuperpackMod.settings.icewolf)
				RCellFinder.TryFindRandomCellNearWith(pawn.Position, _ => true, pawn.Map, out cell, 5, 10);
			__result = cell.IsValid;
			return !__result;
		}
	}

	[HarmonyPatch(typeof(IncidentWorker_ShipChunkDrop), "TryFindShipChunkDropCell")]
	public static class IceWolfChunkAffinity
	{
		[HarmonyPrefix]
		public static bool Prefix(ref bool __result, Map map, int maxDist, out IntVec3 pos)
		{
			pos = IntVec3.Invalid;
			if (maxDist < 1000) return true;
			Pawn pawn = Find.CurrentMap.mapPawns.AllPawns.Find(p =>
				p.Name?.ToStringFull?.ToLowerInvariant().Contains("icewo") ?? false);

			if (pawn != null && MemeSuperpackMod.settings.icewolf)
				RCellFinder.TryFindRandomCellNearWith(pawn.Position, _ => true, map, out pos, 5, 10);
			__result = pos.IsValid;
			return !__result;
		}
	}
}
