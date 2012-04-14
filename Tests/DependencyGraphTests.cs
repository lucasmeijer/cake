﻿using System;
using System.IO;
using NUnit.Framework;

namespace bs.Tests
{
	[TestFixture]
	public class DependencyGraphTests
	{
		[SetUp]
		public void Setup()
		{
			const string dirName = "Workspace";
			if (Directory.Exists(dirName))
				Directory.Delete(dirName,true);
			
			Directory.CreateDirectory(dirName);
			Directory.SetCurrentDirectory(dirName);

			_depGraph = new DependencyGraph();
		}

		protected const string defaultSourceFile = "input.txt";
		protected const string defaulttargetFile = "output.txt";

		protected DependencyGraph _depGraph;

		protected void ThrowIfDepgraphGenerates()
		{
			_depGraph.GenerateCallback += (target, instructions) => { throw new InvalidOperationException(); };
		}
	}
}