using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace cake.Tests
{
	[TestFixture]
	class SchedulableActionCollectorTest
	{
		DependencyGraph _depGraph;
		SchedulableActionCollector _collector;

		[SetUp]
		public void Setup()
		{
			_depGraph = new DependencyGraph();
			_collector = new SchedulableActionCollector(_depGraph);
		}

		[Test]
		public void CanCollectSingleAction()
		{
			var settings = new TargetGenerateSettings(new SimpleAction(s=> { }), new[] {"input"}, "output");
			_depGraph.RegisterTarget(settings);
			var result = _collector.CollectActionsToGenerate("output").ToArray();

			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(settings, result[0].Settings);
		}

		[Test]
		public void CanCollectMultipleActions()
		{
			var settings1 = new TargetGenerateSettings(new SimpleAction(s => { }), new[] { "file1" }, "file2");
			_depGraph.RegisterTarget(settings1);

			var settings2 = new TargetGenerateSettings(new SimpleAction(s => { }), new[] { "file2" }, "file3");
			_depGraph.RegisterTarget(settings2);

			var result = _collector.CollectActionsToGenerate("file3").ToArray();

			var expected = new[]
			               	{
			               		new SchedulableAction(settings1, new string[0] {}),
			               		new SchedulableAction(settings2, new string[1] {"file2"})
			               	};
			
			CollectionAssert.AreEquivalent(result,expected);
		}
	}
}
