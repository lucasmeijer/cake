﻿using System.IO;
using System.Linq;
using NUnit.Framework;

namespace bs.Tests
{
	class TargetWithoutSources : DependencyGraphTests
	{
		[SetUp]
		public void Setup()
		{
			SetupGraphWithOneTargetWithoutSources();
		}

		[Test]
		public void WillGenerateTargetWithoutSources()
		{
			_depGraph.RequestTarget(defaulttargetFile);
			FileAssert.Contains(defaulttargetFile, "Hello");
		}

		[Test]
		public void WontGenerateTargetWithoutSourcesTwice()
		{
			_depGraph.RequestTarget(defaulttargetFile);
			FileAssert.Contains(defaulttargetFile, "Hello");
			ThrowIfDepgraphGenerates();
			_depGraph.RequestTarget(defaulttargetFile);
		}

		[Test]
		public void WillGenerateTargetIfItWasNeverBuiltBefore()
		{
			File.WriteAllText(defaulttargetFile, "One");
			_depGraph.RequestTarget(defaulttargetFile);
			FileAssert.Contains(defaulttargetFile, "Hello");
		}

		private void SetupGraphWithOneTargetWithoutSources()
		{
			_depGraph.RegisterTarget(defaulttargetFile, new TargetBuildInstructions()
			                                           	{
			                                           		Action = (target,sources) => File.WriteAllText(target, "Hello"),
			                                           		SourceFiles = new string[0],
			                                           	});
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
