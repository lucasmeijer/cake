using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Collections.Generic;

namespace bs.Tests
{
	[TestFixture]
	public class CCompilerTests
	{
		[Test]
		public void Test()
		{
			File.WriteAllText("test.c", "void main() {}");
			var task = new CCompilerTask("test.o", "test.c");
			var inputfiles = task.GetInputFiles();

			CollectionAssert.AreEquivalent(new List<string> {"test.c"}, inputfiles);
		}
	}
}
