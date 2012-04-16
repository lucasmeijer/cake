namespace cake
{
	internal static class CakePath
	{
		public static string Combine(string dir, string includedFile)
		{
			if (dir.Length == 0)
				return includedFile;
			return dir + "/" + includedFile;
		}
	}
}