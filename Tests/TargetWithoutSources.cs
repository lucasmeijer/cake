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

		[Test]
		public void RegenerateWhenActionHashChanges()
		{

		}

		
		

		private void SetupGraphWithOneTargetWithoutSources()
		{
			var simpleAction = new SimpleAction((t,s) => File.WriteAllText(t, "Hello"), "hash1");
			
			_depGraph.RegisterTarget(defaulttargetFile, new TargetGenerateInstructions()
			                                           	{
			                                           		Action = simpleAction,
			                                           		Settings = new TargetGenerateSettings() { InputFiles = new HashSet<string>()}
			                                           	});
		}



		
	}
}
