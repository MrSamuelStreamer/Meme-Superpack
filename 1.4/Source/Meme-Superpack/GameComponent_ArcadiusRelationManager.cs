using System.Linq;
using RimWorld;
using Verse;

namespace MSS.MemeSuperpack;

public class GameComponent_ArcadiusRelationManager : GameComponent
{
	public Pawn Arcadius;

	public static Pawn CachedArcadius;
	public static bool GeneratingArcadius = false;

	public GameComponent_ArcadiusRelationManager(Game game)
	{
	}

	public static Pawn GetArcadius()
	{
		if (CachedArcadius != null) return CachedArcadius;
		if (Current.Game.components.Find(gc => gc.GetType() == typeof(GameComponent_ArcadiusRelationManager)) is
		    GameComponent_ArcadiusRelationManager arcManager)
			CachedArcadius = arcManager.Arcadius;
		return CachedArcadius;
	}

	public override void StartedNewGame()
	{
		GenerateArcadius();
	}

	public Pawn GenerateArcadius()
	{
		GeneratingArcadius = true;
		try
		{
			Arcadius = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.Colonist, Faction.OfAncients,
				PawnGenerationContext.NonPlayer, forceDead: true, forceGenerateNewPawn: true,
				forcedTraits: new[] { TraitDefOf.Bisexual }, fixedGender: Gender.Male, colonistRelationChanceFactor: 0,
				biologicalAgeRange: new FloatRange(54, 2000), forceBaselinerChance: 1f));

			if (Arcadius != null)
			{
				Arcadius.Name = new NameSingle("Arcadius");

				// Prevent Arcadius being removed
				Find.WorldPawns.ForcefullyKeptPawns.Add(Arcadius);

				foreach (Pawn p in PawnsFinder.All_AliveOrDead.Where(pawn =>
					         pawn.RaceProps is { } raceProps && raceProps.Humanlike && raceProps.IsFlesh &&
					         raceProps.intelligence == Intelligence.Humanlike && pawn != Arcadius &&
					         !pawn.relations.DirectRelations.Exists(dr => dr.def == MemeSuperPackDefOf.MSSMeme_Arcadius)))
				{
					p.relations.AddDirectRelation(MemeSuperPackDefOf.MSSMeme_Arcadius, Arcadius);
				}
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
		Scribe_References.Look(ref Arcadius, "pawn");
		if (Scribe.mode != LoadSaveMode.PostLoadInit) return;
		if (Arcadius is null) GenerateArcadius();
	}
}
