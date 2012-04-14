using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace bs.Tests
{
	[TestFixture]
	public class IncludeScannerTest
	{
		[Test]
		public void CanFindSimpleInclude()
		{
			RunTest("#include \"windows.h\"", new [] {"windows.h"});
		}

		[Test]
		public void CanFindIncludeWithSquareBrackets()
		{
			RunTest("#include <windows.h>", new[] { "windows.h" });
		}

		[Test]
		public void CanFindMultipleIncludes()
		{
			RunTest("#include <windows.h>\n#include \"myfolder/myfile.h\"", new[] { "windows.h", "myfolder/myfile.h" });
		}

		[Test]
		public void CorrectlyParseRealHardOne()
		{
			RunTest("#include <time.h>  /* XXX should use <ctime> */", new[] { "time.h" });
		}

		private static void RunTest(string fileContents, IEnumerable<string> expected)
		{
			var scanner = new IncludeScanner();
			var result = scanner.ScanText(fileContents).ToList();
			CollectionAssert.AreEquivalent(expected, result);
		}
	}
}
