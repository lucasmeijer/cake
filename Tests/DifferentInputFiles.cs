using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace bs.Tests
{
	[TestFixture]
	class DifferentInputFiles : DependencyGraphTests
	{
		[Test]
		public void WhenInputFilesChangeTargetGetsRebuilt()
		{
			int invocationCount = 0;
			Action<string, TargetBuildSettings> action = (t, s) =>
			{
				File.WriteAllText(t, "Hello");
				invocationCount++;
			};

			File.WriteAllText("file1", "one");
			File.WriteAllText("file2", "two");

			_depGraph.RegisterTarget(defaulttargetFile, MakeInstructions(action, "file1" ));
			_depGraph.RequestTarget(defaulttargetFile);
			Assert.AreEqual(1, invocationCount);

			_depGraph = new DependencyGraph(_buildHistory);
			_depGraph.RegisterTarget(defaulttargetFile, MakeInstructions(action, "file1","file2"));
			_depGraph.RequestTarget(defaulttargetFile);
			Assert.AreEqual(2, invocationCount);
		}

		private static TargetBuildInstructions MakeInstructions(Action<string, TargetBuildSettings> action, params string[] inputFiles)
		{
			var targetBuildInstructions = new TargetBuildInstructions();
			
			targetBuildInstructions.Action = action;
			targetBuildInstructions.Settings = new TargetBuildSettings();
			targetBuildInstructions.Settings.InputFiles = new HashSet<string>(inputFiles);
			return targetBuildInstructions;
		}
	}
}
