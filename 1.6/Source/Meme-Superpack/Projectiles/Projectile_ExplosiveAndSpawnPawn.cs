using RimWorld;
using UnityEngine;
using Verse;

namespace MSS.MemeSuperpack;

public class Projectile_ExplosiveAndSpawnPawn : Projectile_Explosive
{
	protected override void Impact(Thing hitThing, bool blockedByShield = false)
	{
		Map map = Map;
		base.Impact(hitThing, blockedByShield);

		IntVec3 loc = Position;

		foreach (IntVec3 c in GenAdjFast.AdjacentCells8Way(Position))
		{
			if (c.GetFirstBuilding(map) == null && c.Standable(map) && Rand.Chance(.75f))
			{
				Pawn p = PawnGenerator.GeneratePawn(
					MemeSuperPackDefOf.MSSMeme_BabyCritter,
					Find.FactionManager.OfInsects
				);

				GenSpawn.Spawn(p, loc, map);
			}
		}

		foreach (Pawn pawn in map.mapPawns.AllHumanlike)
		{
			TryAddMemory(pawn);
		}
	}

	protected virtual void TryAddMemory(Pawn pawn)
	{
		if (
			pawn.needs?.mood?.thoughts?.memories?.GetFirstMemoryOfDef(
				MemeSuperPackDefOf.MSSMeme_BabyCannonWTF
			) != null
		)
			return;
		Thought_Memory newThought = (Thought_Memory)
			ThoughtMaker.MakeThought(MemeSuperPackDefOf.MSSMeme_BabyCannonWTF);
		pawn.needs?.mood?.thoughts?.memories?.TryGainMemory(newThought);
	}
}
