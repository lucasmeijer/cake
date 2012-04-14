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
			SetupSimpleCopyDepGraph();
			Assert.Throws<MissingDependencyException>(() => _depGraph.RequestTarget(defaulttargetFile));
		}

		[Test]
		public void RegeneratesWhenSourceChanges()
		{
			SetupSimpleCopyDepGraph();
			Assert.Throws<MissingDependencyException>(() => _depGraph.RequestTarget(defaulttargetFile));

			WriteToSourceRunDepGraphAndVerifyTarget("One");
			WriteToSourceRunDepGraphAndVerifyTarget("Two");
		}

		private void WriteToSourceRunDepGraphAndVerifyTarget(string contents)
		{
			File.WriteAllText(defaultSourceFile, contents);
			_depGraph.RequestTarget(defaulttargetFile);
			FileAssert.Contains(defaulttargetFile, contents);
		}

		[Test]
		public void WillNotRegeneratesWhenSourceDidNotChange()
		{
			SetupSimpleCopyDepGraph();
			Assert.Throws<MissingDependencyException>(() => _depGraph.RequestTarget(defaulttargetFile));

			WriteToSourceRunDepGraphAndVerifyTarget("One");

			ThrowIfDepgraphGenerates();
			_depGraph.RequestTarget(defaulttargetFile);
		}

		private void SetupSimpleCopyDepGraph()
		{
			_depGraph.RegisterTarget(defaulttargetFile, new TargetBuildInstructions()
			                                           	{
			                                           		Action = (target,sources) => File.Copy(sources.Single(), target, true),
			                                           		SourceFiles = new[] { defaultSourceFile }
			                                           	});
		}
	}
}
