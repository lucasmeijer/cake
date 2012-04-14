using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace cake.Tests
{
	[TestFixture]
	class DifferentInputFiles : DependencyGraphTests
	{
		[Test]
		public void WhenInputFilesChangeTargetGetsRebuilt()
		{
			int invocationCount = 0;
			var action = new SimpleAction((t,s) =>
			                              	{
			                              		File.WriteAllText(t, "Hello");
			                              		invocationCount++;
			                              	}, "hash");

			File.WriteAllText("file1", "one");
			File.WriteAllText("file2", "two");

			_depGraph.RegisterTarget(MakeInstructions(action, "file1" ));
			_depGraph.RequestTarget(defaulttargetFile);
			Assert.AreEqual(1, invocationCount);

			_depGraph = new DependencyGraph(_buildHistory);
			_depGraph.RegisterTarget(MakeInstructions(action, "file1","file2"));
			_depGraph.RequestTarget(defaulttargetFile);
			Assert.AreEqual(2, invocationCount);
		}

		private static TargetGenerateSettings MakeInstructions(ITargetGeneratingAction action, params string[] inputFiles)
		{
			return new TargetGenerateSettings(action, inputFiles, new[]{defaulttargetFile});
		}
	}
}
