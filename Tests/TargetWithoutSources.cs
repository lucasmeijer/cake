using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace bs.Tests
{
	[TestFixture]
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
			_depGraph.RequestTarget(defaulttargetFile);
			_depGraph = new DependencyGraph(_buildHistory);

			bool invoked = false;
			var action = new SimpleAction((t, s) => { invoked=true; }, "hash2");
			_depGraph.RegisterTarget(defaulttargetFile, new TargetGenerateInstructions()
			{
				Action = action,
				Settings = new TargetGenerateSettings(new HashSet<string>(), action.GetActionHash())
			});
			_depGraph.RequestTarget(defaulttargetFile);
			Assert.IsTrue(invoked);
		}

		
		

		private void SetupGraphWithOneTargetWithoutSources()
		{
			var simpleAction = new SimpleAction((t,s) => File.WriteAllText(t, "Hello"), "hash1");
			
			_depGraph.RegisterTarget(defaulttargetFile, new TargetGenerateInstructions()
			                                           	{
			                                           		Action = simpleAction,
			                                           		Settings = new TargetGenerateSettings(new HashSet<string>(),simpleAction.GetActionHash())
			                                           	});
		}



		
	}
}
