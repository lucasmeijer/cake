using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace cake.Tests
{
	[TestFixture]
	class BenchMark
	{
		[Test]
		[Ignore]
		public void Test()
		{
			var depGraph = new DependencyGraph();

			var libDirs = Directory.GetDirectories("c:/generate_libs/src/jam");
			var libTargets = new List<string>();			
			foreach(var libDir in libDirs)
			{
				var objFiles = new List<String>();
				foreach(var file in Directory.GetFiles(libDir,"*.cpp"))
				{
					string objFile = file + ".o";
					var ctask = new CCompilerTask(objFile, file, new string[]{});
					ctask.RegisterWithDepGraph(depGraph);
					objFiles.Add(objFile);
				}

				var linkAction = new SimpleAction(s =>
				                                  	{
				                                  		var sb = new StringBuilder();
				                                  		foreach (var inputFile in s.InputFiles)
				                                  			sb.Append(File.ReadAllText(inputFile));
				                                  		File.WriteAllText(s.OutputFiles.Single(), sb.ToString());
				                                  	});

				string libTarget = Path.GetFileNameWithoutExtension(libDir) + ".lib";
				var linkSettings = new TargetGenerateSettings(linkAction, objFiles, libTarget);
				depGraph.RegisterTarget(linkSettings);
				libTargets.Add(libTarget);
			}
			depGraph.RequestTarget(libTargets[0]);
		}
	}
}
