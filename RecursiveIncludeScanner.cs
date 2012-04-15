using System;
using System.Collections.Generic;
using System.IO;

namespace cake
{
	public class RecursiveIncludeScanner
	{
		private readonly string[] _includePaths;
		private readonly IncludeScanner _includeScanner;

		public RecursiveIncludeScanner(string[] includePaths, IncludeScanner scanner)
		{
			_includePaths = includePaths;
			_includeScanner = scanner;
		}

		public IEnumerable<string> GetFilesIncludedBy(string file, DependencyGraph depGraph)
		{
			foreach (var includedFile in _includeScanner.Scan(file))
			{
				var foundIncludeFile = FindSpecifiedIncludeFileInSearchDirs(depGraph,includedFile);
				if (foundIncludeFile == null)
					throw new InvalidOperationException("unable to find the header file: " + includedFile + " in the header search paths.");
				yield return foundIncludeFile;
				if (!File.Exists(foundIncludeFile))
					continue;
				foreach (var file2 in GetFilesIncludedBy(foundIncludeFile, depGraph))
					yield return file2;
			}
		}

		private string FindSpecifiedIncludeFileInSearchDirs(DependencyGraph depGraph, string includedFile)
		{
			return includedFile;
		}
	}
}