using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace bs
{
	static class FileAssert
	{
		static public void Contains(string file, string contents)
		{
			Assert.AreEqual(contents, File.ReadAllText(file));
		}
	}
}
