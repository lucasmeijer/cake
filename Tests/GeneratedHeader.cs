using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace cake.Tests
{
	[TestFixture]
	public class GeneratedHeader : DependencyGraphTests
	{
		[Test]
		[Ignore]
		public void Test()
		{
			var generateHeaderAction = new SimpleAction(s => File.Copy(s.InputFiles.Single(), s.OutputFiles.Single(), true));
			var generateHeaderSettings = new TargetGenerateSettings(generateHeaderAction, new[] { "file1" }, "myheader.h");

			File.WriteAllText("file1","//theboss");
			File.WriteAllText("test.c","#include <myheader.h>");
			
			//temp:
			File.WriteAllText("myheader.h","doesalreadyexisttemporary" );
			var compilecppfile = new CCompilerTask("test.o", "test.c", new string[] {});

			_depGraph.RegisterTarget(generateHeaderSettings);
			compilecppfile.RegisterWithDepGraph(_depGraph);

			_depGraph.RequestTarget("test.o");
		}
	}
}
