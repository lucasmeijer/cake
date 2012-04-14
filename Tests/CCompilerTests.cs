using System.IO;
using NUnit.Framework;

namespace cake.Tests
{
	[TestFixture]
	public class CCompilerTests
	{
		[Test]
		public void GetInputFilesFindsRecursiveHeaderDependencies()
		{
			File.WriteAllText("test.c", "#include <test.h>\nvoid main() {}");
			File.WriteAllText("test.h", "#include <shared.h>");
			File.WriteAllText("shared.h", "//like a boss");
			var task = new CCompilerTask("test.o", "test.c", new[] {""});
			var inputfiles = task.GetInputFiles();

			CollectionAssert.AreEquivalent(new [] {"test.c","test.h","shared.h"}, inputfiles);
		}
	}
}
