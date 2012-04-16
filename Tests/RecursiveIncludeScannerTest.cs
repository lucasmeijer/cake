using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;

namespace cake.Tests
{
	[TestFixture]
	class RecursiveIncludeScannerTest
	{
		[SetUp]
		public void Setup()
		{
			Tools.SetupCleanCurrentDirectory();
		}

		[Test]
		public void CanFindHeaderInSameDirectory()
		{
			var ris = new RecursiveIncludeScanner(new string[0], file=>false,ScanFileMock);
			File.WriteAllText("myheader.h", "//boss");
			var result = ris.GetFilesIncludedBy("test.c");
			Assert.AreEqual("myheader.h",result.Single());
		}

		[Test]
		public void FailToFindNonExistingHeader()
		{
			WillFileBeGeneratedAtPath willFileBeGeneratedAtPath = file => false;
			var ris = new RecursiveIncludeScanner(new string[0], willFileBeGeneratedAtPath, ScanFileMock);
			
			Assert.Throws<MissingHeaderException>(()=>ris.GetFilesIncludedBy("test.c"));
		}

		[Test]
		public void CanFindHeaderWithSubDirectorySpecification()
		{
			var ris = new RecursiveIncludeScanner(new string[0], file => false, f=>ScanFileMock(f,"sub1/myheader.h"));
			Directory.CreateDirectory("sub1");
			File.WriteAllText("sub1/myheader.h", "//boss");
			var result = ris.GetFilesIncludedBy("test.c");
			Assert.AreEqual("sub1/myheader.h", result.Single());
		}

		[Test]
		public void CanFindHeaderInIncludePath()
		{
			var ris = new RecursiveIncludeScanner(new[] {"sub1"}, file => false, f => ScanFileMock(f, "myheader.h"));
			Directory.CreateDirectory("sub1");
			File.WriteAllText("sub1/myheader.h", "//boss");
			var result = ris.GetFilesIncludedBy("test.c");
			Assert.AreEqual("sub1/myheader.h", result.Single());
		}

		private static IEnumerable<string> ScanFileMock(string file)
		{
			return ScanFileMock(file, "myheader.h");
		}
		private static IEnumerable<string> ScanFileMock(string file,string headertofind)
		{
			if (file != "test.c")
				yield break;
			yield return headertofind;
		}
	}
}
