using System.IO;
using NUnit.Framework;

namespace cake
{
	static class FileAssert
	{
		static public void Contains(string file, string contents)
		{
			Assert.AreEqual(contents, File.ReadAllText(file));
		}
	}
}
