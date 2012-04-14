using System.Collections.Generic;
using bs.Tests;

namespace bs
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

		public IEnumerable<string> GetInputFiles()
		{
			var scanner = new RecursiveIncludeScanner(_includePaths, new IncludeScanner());
			yield return _sourceFile;
			foreach (var file in scanner.GetFilesIncludedBy(_sourceFile))
				yield return file;
		}
	}
}