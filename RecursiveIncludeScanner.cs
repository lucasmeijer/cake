using System;
using System.Collections.Generic;

namespace bs
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

		public IEnumerable<string> GetFilesIncludedBy(string file)
		{
			foreach (var includedFile in _includeScanner.Scan(file))
			{
				var foundIncludeFile = FindSpecifiedIncludeFileInSearchDirs(includedFile);
				if (foundIncludeFile == null)
					throw new InvalidOperationException("unable to find the header file: " + includedFile + " in the header search paths.");
				yield return foundIncludeFile;
				foreach (var file2 in GetFilesIncludedBy(foundIncludeFile))
					yield return file2;
			}
		}

		private string FindSpecifiedIncludeFileInSearchDirs(string includedFile)
		{
			return includedFile;
		}
	}
}