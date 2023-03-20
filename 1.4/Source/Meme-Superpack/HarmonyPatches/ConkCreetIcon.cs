using System;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace MSS.MemeSuperpack.HarmonyPatches
{
	[HarmonyPatch(typeof(BuildableDef), nameof(BuildableDef.GetUIIconForStuff))]
	public static class ConkCreetUIIcons
	{
		private static Lazy<Texture2D> ConkCreetTexture =
			new(() => ContentFinder<Texture2D>.Get("Memes/ConkCreetBaybeeSmall"));

		[HarmonyPostfix]
		public static Texture2D Postfix(Texture2D result, ThingDef stuff)
		{
			return stuff == null || !stuff.defName.Contains("oncrete") ? result : ConkCreetTexture.Value;
		}
	}

	[HarmonyPatch(typeof(BuildableDef), "ResolveIcon")]
	public static class ConkCreetUIIconResolve
	{
		private static Lazy<Texture2D> ConkCreetTexture =
			new(() => ContentFinder<Texture2D>.Get("Memes/ConkCreetBaybeeSmall"));

		[HarmonyPostfix]
		public static void Postfix(BuildableDef __instance)
		{
			if (!__instance.defName.Contains("oncrete")) return;
			__instance.uiIcon = ConkCreetTexture.Value;
		}
	}
}
