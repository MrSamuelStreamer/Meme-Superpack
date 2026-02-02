using UnityEngine;
using Verse;

namespace MSS.MemeSuperpack;

[StaticConstructorOnStartup]
public static class Textures
{
	public static Texture2D MSSMeme_OskarianBed_Upgrade = ContentFinder<Texture2D>.Get(
		"UI/MSSMeme_OskarianBed_Upgrade"
	);
}
