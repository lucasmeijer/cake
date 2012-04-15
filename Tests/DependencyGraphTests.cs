using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace cake.Tests
{
	public class DependencyGraphTests
	{
		[SetUp]
		public void Setup()
		{
			Tools.SetupCleanCurrentDirectory();

			_buildHistory = new BuildHistory();
			_depGraph = new DependencyGraph(_buildHistory);
		}

		protected const string defaultSourceFile = "input.txt";
		protected const string defaulttargetFile = "output.txt";

		protected DependencyGraph _depGraph;
		protected BuildHistory _buildHistory;

		protected void ThrowIfDepgraphGenerates()
		{
			_depGraph.GenerateCallback += settings => { throw new InvalidOperationException(); };
		}
	}
}