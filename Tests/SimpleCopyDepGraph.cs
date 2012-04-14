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
		[SetUp]
		public void Setup()
		{
			SetupSimpleCopyDepGraph();
		}
		

		[Test]
		public void ThrowsIfDependencyDoesNotExist()
		{
			Assert.Throws<MissingDependencyException>(() => _depGraph.RequestTarget(defaulttargetFile));
		}

		[Test]
		public void RegeneratesWhenSourceChanges()
		{
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
			Assert.Throws<MissingDependencyException>(() => _depGraph.RequestTarget(defaulttargetFile));

			WriteToSourceRunDepGraphAndVerifyTarget("One");

			ThrowIfDepgraphGenerates();
			_depGraph.RequestTarget(defaulttargetFile);
		}

		[Test]
		public void WillNotRegeneratesWhenTargetGotManuallyUpdated()
		{
			Assert.Throws<MissingDependencyException>(() => _depGraph.RequestTarget(defaulttargetFile));

			WriteToSourceRunDepGraphAndVerifyTarget("One");

			File.WriteAllText(defaulttargetFile, "Three");
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
