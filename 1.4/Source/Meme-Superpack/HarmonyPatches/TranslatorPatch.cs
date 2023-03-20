using HarmonyLib;
using Verse;

namespace MSS.MemeSuperpack.HarmonyPatches
{
	[HarmonyPatch(typeof(Translator), nameof(Translator.Translate), typeof(string))]
	public static class ConkCreetTranslator
	{
		[HarmonyPostfix]
		public static TaggedString Postfix(TaggedString result)
		{
			return result
				.Replace("concrete", "conk creet baybee")
				.Replace("Concrete", "Conk creet baybee")
				.Replace("cement", "cement (das conk creet baybee)")
				.Replace("Cement", "Cement (das conk creet baybee)");
		}
	}

	[HarmonyPatch(typeof(Def), nameof(Def.LabelCap), MethodType.Getter)]
	public static class ConkCreetTranslatorLabel
	{
		[HarmonyPostfix]
		public static TaggedString Postfix(TaggedString result)
		{
			return result
				.Replace("concrete", "conk creet baybee")
				.Replace("Concrete", "Conk creet baybee")
				.Replace("cement", "cement (das conk creet baybee)")
				.Replace("Cement", "Cement (das conk creet baybee)");
		}
	}
}
