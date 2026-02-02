using UnityEngine;
using Verse;

namespace MSSMeme;

public class MSSMeme_BalloonPawnFlyerWorker : PawnFlyerWorker
{
	public MSSMeme_BalloonPawnFlyerWorker(PawnFlyerProperties properties)
		: base(properties) { }

	public override float AdjustedProgress(float t)
	{
		AnimationCurve progressCurve = properties.ProgressCurve;
		return progressCurve == null || progressCurve.length == 0 ? t : progressCurve.Evaluate(t);
	}

	public override float GetHeight(float t) => t;
}
