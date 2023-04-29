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

	public GameComponent_ArcadiusRelationManager(Game game)
	{
	}

	public static Pawn GetArcadius(bool generateIfAbsent = false)
	{
		if (!MemeSuperpackMod.settings.arcadius) return null;
		if (CachedArcadius != null) return CachedArcadius;
		if (Current.Game.components.Find(gc => gc.GetType() == typeof(GameComponent_ArcadiusRelationManager)) is
		    GameComponent_ArcadiusRelationManager arcManager)
			CachedArcadius = arcManager.Arcadius ?? (generateIfAbsent ? arcManager.GenerateArcadius(true) : null);
		return CachedArcadius;
	}

	public override void StartedNewGame()
	{
		CachedArcadius = null;
		if (!MemeSuperpackMod.settings.arcadius) return;
		GenerateArcadius(false);
	}

	public override void LoadedGame()
	{
		CachedArcadius = null;
		if (Arcadius == null || PawnsFinder.All_AliveOrDead.AsParallel().Contains(Arcadius)) return;
		CleanUpArcadius();
		if (!MemeSuperpackMod.settings.arcadius) return;
		GenerateArcadius(false);
	}

	public static Pawn GetArcadiusByHediff() => PawnsFinder.All_AliveOrDead
		.FirstOrFallback(p => p.health.hediffSet.HasHediff(MemeSuperPackDefOf.MSSMeme_YChromosomalAdam));

	public static IEnumerable<Pawn> GetAllArcadiusByHediff() => PawnsFinder.All_AliveOrDead
		.Where(p => p.health.hediffSet.HasHediff(MemeSuperPackDefOf.MSSMeme_YChromosomalAdam));

	public void SetYChromosomalAdamRelation(Pawn arcadius) => PawnsFinder.All_AliveOrDead.AsParallel().Where(pawn =>
		pawn.RaceProps is { Humanlike: true, IsFlesh: true, intelligence: Intelligence.Humanlike } && pawn != Arcadius).ForAll(p =>
	{
		RemoveYChromosomalAdamRelations(p);
		if (arcadius != null) p.relations.AddDirectRelation(MemeSuperPackDefOf.MSSMeme_Arcadius, arcadius);
	});

	public static void ClearYChromosomalAdamRelations() => PawnsFinder.All_AliveOrDead.AsParallel().ForAll(RemoveYChromosomalAdamRelations);

	public static void CleanUpArcadius()
	{
		ClearYChromosomalAdamRelations();
		foreach (Pawn arcadius in GetAllArcadiusByHediff())
		{
			Find.WorldPawns.ForcefullyKeptPawns.Remove(arcadius);
			arcadius.health.RemoveHediff(arcadius.health.hediffSet.GetFirstHediffOfDef(MemeSuperPackDefOf.MSSMeme_YChromosomalAdam));
		}

		CachedArcadius = null;
		if (Current.Game.components.Find(gc => gc.GetType() == typeof(GameComponent_ArcadiusRelationManager)) is GameComponent_ArcadiusRelationManager arcManager)
			arcManager.Arcadius = null;
	}

	public static void RemoveYChromosomalAdamRelations(Pawn pawn)
	{
		// Remove any prior Y-ChromosomalAdam relations
		var relations = pawn?.relations?.DirectRelations?.Where(r => r.def == MemeSuperPackDefOf.MSSMeme_Arcadius).ToList();
		if (relations == null) return;
		foreach (DirectPawnRelation r in relations)
		{
			try
			{
				pawn.relations?.RemoveDirectRelation(r);
			}
			catch (Exception)
			{
				pawn.relations?.DirectRelations?.Remove(r);
			}
		}
	}

	public Pawn GenerateArcadius(bool force)
	{
		if (!MemeSuperpackMod.settings.arcadius) return null;
		if (Arcadius != null && !force) return Arcadius;
		if (Arcadius == null)
		{
			// Check for uncached Arcadius by comp
			Arcadius = GetArcadiusByHediff();
			if (Arcadius != null)
			{
				SetYChromosomalAdamRelation(Arcadius);
				return Arcadius;
			}
		}

		if (Arcadius != null)
		{
			CleanUpArcadius();
			Arcadius = null;
		}

		GeneratingArcadius = true;
		try
		{
			Arcadius = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.Colonist, Faction.OfAncients, forceDead: true, forceGenerateNewPawn: true,
				forcedTraits: new[] { TraitDefOf.Bisexual }, fixedGender: Gender.Male, colonistRelationChanceFactor: 0,
				biologicalAgeRange: new FloatRange(54, 2000), forceBaselinerChance: 1f));
			if (Arcadius != null)
			{
				Arcadius.Name = new NameSingle("Arcadius");
				Arcadius.health.AddHediff(MemeSuperPackDefOf.MSSMeme_YChromosomalAdam);
				// Prevent Arcadius being removed
				Find.WorldPawns.ForcefullyKeptPawns.Add(Arcadius);
				SetYChromosomalAdamRelation(Arcadius);
			}
		}
		finally
		{
			GeneratingArcadius = false;
		}

		return Arcadius;
	}

	public override void ExposeData()
	{
		Scribe_Deep.Look(ref _arcadius, "pawn");
		if (Scribe.mode != LoadSaveMode.PostLoadInit) return;
		if (Arcadius is null) GenerateArcadius(false);
	}
}
