using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace bs.Tests
{
	class SimpleCopyDepGraph : DependencyGraphTests
	{
		[Test]
		public void ThrowsIfDependencyDoesNotExist()
		{
			DependencyGraph depGraph = SetupSimpleCopyDepGraph();
			Assert.Throws<MissingDependencyException>(() => depGraph.RequestTarget(defaulttargetFile));
		}

		[Test]
		public void RegeneratesWhenSourceChanges()
		{
			DependencyGraph depGraph = SetupSimpleCopyDepGraph();
			Assert.Throws<MissingDependencyException>(() => depGraph.RequestTarget(defaulttargetFile));

			File.WriteAllText(defaultSourceFile, "One");
			depGraph.RequestTarget(defaulttargetFile);
			FileAssert.Contains(defaulttargetFile, "One");
			
			File.WriteAllText(defaultSourceFile, "Two");
			depGraph.RequestTarget(defaulttargetFile);
			FileAssert.Contains(defaulttargetFile, "Two");
		}

		private static DependencyGraph SetupSimpleCopyDepGraph()
		{
			var depGraph = new DependencyGraph();

			depGraph.RegisterTarget(defaulttargetFile, new TargetBuildInstructions()
			                                           	{
			                                           		Action = (target,sources) => File.Copy(sources.Single(), target, true),
			                                           		SourceFiles = new[] { defaultSourceFile }
			                                           	});
			return depGraph;
		}
	}
}
