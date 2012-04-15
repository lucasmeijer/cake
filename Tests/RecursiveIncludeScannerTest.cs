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
			
			Assert.Throws<MissingHeaderException>(()=>ris.GetFilesIncludedBy("test.c").ToArray());
		}

		private static IEnumerable<string> ScanFileMock(string file)
		{
			if (file != "test.c")
				yield break;
			yield return "myheader.h";
		}
	}
}
