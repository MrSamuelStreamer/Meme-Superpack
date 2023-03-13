using System.Diagnostics;

namespace MSS.MemeSuperpack
{
	static class Log
	{
		[Conditional("DEBUG")]
		public static void Message(string x)
		{
			Verse.Log.Message(x);
		}
	}
}
