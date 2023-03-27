using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace MSS.MemeSuperpack;

public class GameCondition_Police : GameCondition
{
	private bool _isRed;
	private static WeatherDef _forcedWeather = WeatherDef.Named("RainyThunderstorm");
	public const float MaxSunGlow = 0.5f;
	private const float Glow = 0.25f;
	private const float SkyColorStrength = 0.075f;
	private const float OverlayColorStrength = 1.5f;
	private const float BaseBrightness = 0.73f;

	public Color CurrentColor => _isRed ? Color.red : Color.blue;

	private bool BrightInAllMaps
	{
		get
		{
			List<Map> maps = Find.Maps;
			for (int index = 0; index < maps.Count; ++index)
			{
				if (GenCelestial.CurCelestialSunGlow(maps[index]) <= 0.5)
					return false;
			}

			return true;
		}
	}

	public override int TransitionTicks => 200;

	public override bool AllowEnjoyableOutsideNow(Map map) => false;
	public override float SkyGazeChanceFactor(Map map) => 0f;
	public override WeatherDef ForcedWeather() => _forcedWeather;

	public override float SkyGazeJoyGainFactor(Map map) => 0f;

	public override float SkyTargetLerpFactor(Map map) =>
		GameConditionUtility.LerpInOutValue(this, TransitionTicks);

	public override SkyTarget? SkyTarget(Map map)
	{
		Color currentColor = CurrentColor;
		SkyColorSet colorSet = new SkyColorSet(Color.Lerp(Color.white, currentColor, SkyColorStrength) * Brightness(map),
			Color.grey, Color.Lerp(Color.white, currentColor, OverlayColorStrength) * Brightness(map), 1f);
		return new SkyTarget(Mathf.Max(GenCelestial.CurCelestialSunGlow(map), Glow), colorSet,
			0.6f, 0.5f);
	}

	private float Brightness(Map map) => Mathf.Max(MaxSunGlow, GenCelestial.CurCelestialSunGlow(map));

	public override void GameConditionTick()
	{
		if (TicksPassed % 100 == 0) _isRed = !_isRed;
	}

}
