using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace bs.Tests
{
	class TargetWithoutSources : DependencyGraphTests
	{
		[Test]
		public void WillGenerateTargetWithoutSources()
		{
			DependencyGraph depGraph = SetupGraphWithOneTargetWithoutSources();

			depGraph.RequestTarget(defaulttargetFile);
			FileAssert.Contains(defaulttargetFile, "Hello");
		}

		[Test]
		public void WontGenerateTargetWithoutSourcesTwice()
		{
			DependencyGraph depGraph = SetupGraphWithOneTargetWithoutSources();

			depGraph.RequestTarget(defaulttargetFile);
			FileAssert.Contains(defaulttargetFile, "Hello");
			depGraph.GenerateCallback += (target, instructions) => { throw new InvalidOperationException(); };
			depGraph.RequestTarget(defaulttargetFile);
		}

		private static DependencyGraph SetupGraphWithOneTargetWithoutSources()
		{
			var depGraph = new DependencyGraph();
			depGraph.RegisterTarget(defaulttargetFile, new TargetBuildInstructions()
			                                           	{
			                                           		Action = (target,sources) => File.WriteAllText(target, "Hello"),
			                                           		SourceFiles = new string[0],
			                                           	});
			return depGraph;
		}

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
			FileAssert.Contains(defaultSourceFile, "One");
			
			File.WriteAllText(defaultSourceFile, "Two");
			depGraph.RequestTarget(defaulttargetFile);
			FileAssert.Contains(defaultSourceFile, "Two");
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
