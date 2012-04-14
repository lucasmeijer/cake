using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace bs.Tests
{
	class TargetWithoutSources : DependencyGraphTests
	{
		[SetUp]
		public new void Setup()
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
			                                           		Settings = new TargetBuildSettings() { InputFiles = new HashSet<string>()}
			                                           	});
		}

	}
}
