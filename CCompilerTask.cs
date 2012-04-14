using System;
using System.Collections.Generic;
using bs.Tests;

namespace bs
{
	public class CCompilerTask
	{
		private readonly string _targetFile;
		private readonly string _sourceFile;

		public CCompilerTask(string targetFile, string sourceFile)
		{
			_targetFile = targetFile;
			_sourceFile = sourceFile;
		}

		public IEnumerable<string> GetInputFiles()
		{
			yield return _sourceFile;
			foreach (var file in GetFilesIncludedBy(_sourceFile))
				yield return file;
		}

		private static IEnumerable<string> GetFilesIncludedBy(string file)
		{
			var includeScanner = new IncludeScanner();
			foreach(var includedFile in includeScanner.Scan(file))
			{
				var foundIncludeFile = FindSpecifiedIncludeFileInSearchDirs(includedFile);
				if (foundIncludeFile == null)
					throw new InvalidOperationException("unable to find the header file: "+includedFile+" in the header search paths.");
				yield return foundIncludeFile;
				foreach (var file2 in GetFilesIncludedBy(foundIncludeFile))
					yield return file2;
			}
		}

		private static string FindSpecifiedIncludeFileInSearchDirs(string includedFile)
		{
			return includedFile;
		}
	}
}