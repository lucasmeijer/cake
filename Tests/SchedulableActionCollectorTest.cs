using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cake;
using cake.Tests;
using NUnit.Framework;

namespace bs.Tests
{
	[TestFixture]
	class SchedulableActionCollectorTest
	{
		[Test]
		public void CanCollectSingleAction()
		{
			var depGraph = new DependencyGraph();
			var a = new SchedulableActionCollector(depGraph);

			var settings = new TargetGenerateSettings(new SimpleAction(s=> { }), new[] {"input"}, "output");
			depGraph.RegisterTarget(settings);
			var result = a.CollectActionsToGenerate("output").ToArray();

			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(settings,result[0].Settings);
		}
	}
}
