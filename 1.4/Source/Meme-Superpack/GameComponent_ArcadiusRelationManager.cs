using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack;

public class GameComponent_ArcadiusRelationManager : GameComponent
{
	public Pawn Arcadius
	{
		get => _arcadius;
		set
		{
			_arcadius = value;
			CachedArcadius = value;
		}
	}

	public static Pawn CachedArcadius;
	public static bool GeneratingArcadius = false;
	public Pawn _arcadius;
	public int nextArcadiusTick = 1000;

	public GameComponent_ArcadiusRelationManager(Game game)
	{
	}

	public static Pawn GetArcadius(bool forceGenerateIfAbsent = false)
	{
		return null;
	}

	public override void FinalizeInit()
	{
		CachedArcadius = null;
	}

	public static IEnumerable<Pawn> GetAllArcadiusByHediff() => PawnsFinder.All_AliveOrDead
		.Where(p => p.health.hediffSet.HasHediff(MemeSuperPackDefOf.MSSMeme_YChromosomalAdam));

	public static void ClearYChromosomalAdamRelations() => PawnsFinder.All_AliveOrDead.AsParallel().ForAll(RemoveYChromosomalAdamRelations);

	public static void CleanUpArcadius()
	{
		ClearYChromosomalAdamRelations();
		foreach (Pawn arcadius in GetAllArcadiusByHediff())
		{
			Find.WorldPawns.ForcefullyKeptPawns.Remove(arcadius);
			Verse.Hediff hediff = arcadius.health.hediffSet.GetFirstHediffOfDef(MemeSuperPackDefOf.MSSMeme_YChromosomalAdam);
			if (hediff == null) return;
			arcadius.health.RemoveHediff(hediff);
		}

		CachedArcadius = null;
		if (Current.Game.components.Find(gc => gc.GetType() == typeof(GameComponent_ArcadiusRelationManager)) is GameComponent_ArcadiusRelationManager arcManager)
			arcManager.Arcadius = null;
	}

	public static void RemoveYChromosomalAdamRelations(Pawn pawn)
	{
		// Remove any prior Y-ChromosomalAdam relations
		var relations = pawn?.relations?.DirectRelations?.Where(r => r.def == MemeSuperPackDefOf.MSSMeme_Arcadius || r.def == null || r.otherPawn == null).ToList();
		if (relations == null) return;
		foreach (DirectPawnRelation r in relations)
		{
			try
			{
				pawn.relations?.RemoveDirectRelation(r);
			}
			catch (Exception)
			{
				try
				{
					pawn.relations?.DirectRelations?.Remove(r);
				}
				catch (Exception)
				{
					// ignored
				}
			}
		}
	}

	public override void ExposeData()
	{
		Scribe_References.Look(ref _arcadius, "pawn");
		if (Scribe.mode != LoadSaveMode.PostLoadInit) return;
		CleanUpArcadius();
		_arcadius = null;
	}
}
