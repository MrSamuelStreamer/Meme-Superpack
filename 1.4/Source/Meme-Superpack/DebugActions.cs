using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace MSS.MemeSuperpack;

public static class DebugActions
{
	[DebugAction("Memes", "FORCE CLEAR ALL relations!!!",
		allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static void ForceClearAllRels()
	{
		FieldInfo directRelationsField = AccessTools.Field(typeof(Pawn_RelationsTracker), "directRelations");
		PawnsFinder.All_AliveOrDead.Where(p => p.relations != null).Do(pawn => directRelationsField.SetValue(pawn.relations, new List<DirectPawnRelation>()));
	}

	[DebugAction("Memes", "Clear ALL relations!!!",
		allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static void ClearAllRels()
	{
		PawnsFinder.All_AliveOrDead.ForEach(pawn =>
		{
			var relationsDirectRelations = new List<DirectPawnRelation>(pawn.relations?.DirectRelations ?? new List<DirectPawnRelation>());
			relationsDirectRelations?.ForEach(r =>
			{
				try
				{
					pawn.relations.TryRemoveDirectRelation(r?.def, r?.otherPawn);
				}
				catch (Exception)
				{
					relationsDirectRelations?.Remove(r);

					try
					{
						if ((r?.def?.reflexive ?? false) && r.otherPawn is { } otherPawn)
						{
							var directRelations = otherPawn.relations?.DirectRelations;
							DirectPawnRelation directPawnRelation = directRelations?.Find(x => x?.def == r.def && x?.otherPawn == pawn);
							directPawnRelation?.def?.Worker?.OnRelationRemoved(otherPawn, pawn);
							directRelations?.Remove(directPawnRelation);
						}

						r?.def?.Worker?.OnRelationRemoved(pawn, r.otherPawn);
					}
					catch (Exception)
					{
						// ignored
					}
				}
			});
		});
		PawnsFinder.All_AliveOrDead.ForEach(pawn => pawn.relations.ClearAllRelations());
	}

	[DebugAction("Memes", "Clear broken relations", false, false, false, 0, false,
		allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static void ClearBrokenRels()
	{
		var sum = PawnsFinder.All_AliveOrDead.Sum(pawn =>
		{
			var relsRemoved = pawn.relations?.DirectRelations?.RemoveAll(r => r?.def == null || r?.otherPawn == null) ?? 0;
			if (relsRemoved > 0) Debug.Log($"Cleaning {relsRemoved} from pawn {pawn.Name?.ToStringFull}");
			return relsRemoved;
		});
		Debug.Log($"Cleaned up {sum} relations\nAdding back any missing reflexive relationships");
		sum = 0;
		foreach (Pawn pawn in PawnsFinder.All_AliveOrDead)
		{
			foreach (DirectPawnRelation r in new List<DirectPawnRelation>(pawn.relations?.DirectRelations ?? new List<DirectPawnRelation>()))
			{
				Debug.Log($"Processing {r}");
				if (r.def.reflexive && !r.def.implied && r.otherPawn != null &&
				    (!r.otherPawn?.relations?.DirectRelationExists(r.def, pawn) ?? false))
				{
					try
					{
						r.otherPawn.relations.AddDirectRelation(r.def, pawn);
					}
					catch (Exception)
					{
						pawn.relations?.DirectRelations?.Remove(r);
						pawn.relations?.AddDirectRelation(r.def, r.otherPawn);
					}

					sum++;
				}
			}
		}

		Debug.Log($"Attempted to add back {sum} missing reflexive relations");
	}

	[DebugAction("Memes", "Remove AllFather - Arcadius", false, false, false, 0, false,
		allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static void RemoveYChromosomalAdam()
	{
		GameComponent_ArcadiusRelationManager.CleanUpArcadius();
	}

	[DebugAction("Memes", "Gaslighting - DarkSolar", false, false, false, 0, false,
		allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static void ForceDarkSolar()
	{
		GameComponent_MemeTracker.Instance.StartGasLighting(GameComponent_MemeTracker.GaslightingTopic.DarkSolar);
	}

	[DebugAction("Memes", "Gaslighting - Stop", false, false, false, 0, false,
		allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static void StopGaslighting()
	{
		GameComponent_MemeTracker.Instance.EndGaslighting();
	}

	[DebugAction("Memes", "Grignr Attacks", false, false, false, 0, false,
		allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static void GrignrAttack()
	{
		GameComponent_MemeTracker.Instance.GrignrAttack();
	}

	[DebugAction("Memes", "Toggle Eclipse", false, false, false, 0, false,
		allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static void ToggleEclipse()
	{
		if (Find.World.GameConditionManager.ConditionIsActive(GameConditionDefOf.Eclipse))
		{
			Find.World.GameConditionManager.ActiveConditions.Find(x => x.def == GameConditionDefOf.Eclipse).End();
		}
		else
		{
			IncidentParms incidentParms = new()
			{
				forced = true,
				target = Find.World
			};
			DefDatabase<IncidentDef>.GetNamed("Eclipse").Worker.TryExecute(incidentParms);
		}
	}
}
