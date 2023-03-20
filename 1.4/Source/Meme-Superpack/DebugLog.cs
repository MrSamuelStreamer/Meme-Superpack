using System.Diagnostics;

namespace MSS.MemeSuperpack
{
	static class Log
	{
		[Conditional("DEBUG")]
		public static void Debug(string x)
		{
			Verse.Log.Message(x);
		}

		public static void Message(string s)
		{
			Verse.Log.Message(s);
		}
	}
}
