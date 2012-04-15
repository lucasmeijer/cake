using System;
using System.Collections.Generic;
using System.IO;
using cake.Tests;

namespace cake
{
	public class CCompilerTask
	{
		private readonly string _targetFile;
		private readonly string _sourceFile;
		private readonly string[] _includePaths;

		public CCompilerTask(string targetFile, string sourceFile, string[] includePaths)
		{
			_targetFile = targetFile;
			_sourceFile = sourceFile;
			_includePaths = includePaths;
		}

		public IEnumerable<string> GetInputFiles(DependencyGraph depGraph)
		{
			var scanner = new RecursiveIncludeScanner(_includePaths, depGraph.IsTargetRegistered,new IncludeScanner().Scan);
			yield return _sourceFile;
			foreach (var file in scanner.GetFilesIncludedBy(_sourceFile))
				yield return file;
		}

		public void RegisterWithDepGraph(DependencyGraph depGraph)
		{
			var action = new SimpleAction(Execute, "todo");
			var settings = new TargetGenerateSettings(action, GetInputFiles(depGraph), _targetFile);
			depGraph.RegisterTarget(settings);
		}

		public void Execute(TargetGenerateSettings settings)
		{
			//todo
			File.Copy(_sourceFile, _targetFile, true);
		}
	}
}