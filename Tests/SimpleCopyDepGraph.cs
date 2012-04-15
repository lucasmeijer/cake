using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace cake.Tests
{
	class SimpleCopyDepGraph : DependencyGraphTests
	{
		[SetUp]
		public new void Setup()
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
			WriteToSourceRunDepGraphAndVerifyTarget("One");

			ThrowIfDepgraphGenerates();
			_depGraph.RequestTarget(defaulttargetFile);
		}

		[Test]
		public void WillNotRegeneratesWhenTargetGotManuallyUpdated()
		{
			WriteToSourceRunDepGraphAndVerifyTarget("One");

			File.WriteAllText(defaulttargetFile, "Three");
			ThrowIfDepgraphGenerates();
			_depGraph.RequestTarget(defaulttargetFile);
		}

		private void SetupSimpleCopyDepGraph()
		{
			var action = new SimpleAction(settings => File.Copy(settings.InputFiles.Single(), settings.OutputFiles.Single(), true), "hash");
			_depGraph.RegisterTarget(new TargetGenerateSettings(action, new[] {defaultSourceFile}, defaulttargetFile));
		}
	}
}
