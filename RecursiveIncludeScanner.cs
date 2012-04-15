using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace cake
{
	public class RecursiveIncludeScanner
	{
		private readonly string[] _includePaths;
		private readonly WillFileBeGeneratedAtPath _willFileBeGeneratedAtPath;
		private readonly ScanFileForIncludes _scanFileForIncludes;

		public RecursiveIncludeScanner(IEnumerable<string> includePaths, WillFileBeGeneratedAtPath willFileBeGeneratedAtPath, ScanFileForIncludes scanFileForIncludes)
		{
			_includePaths = includePaths.ToArray();
			_willFileBeGeneratedAtPath = willFileBeGeneratedAtPath;
			_scanFileForIncludes = scanFileForIncludes;
		}

		public IEnumerable<string> GetFilesIncludedBy(string file)
		{
			foreach (var includedFile in _scanFileForIncludes(file))
			{
				var foundIncludeFile = FindSpecifiedIncludeFileInSearchDirs(file,includedFile);
				if (foundIncludeFile == null)
					throw new MissingHeaderException(file, includedFile);
				yield return foundIncludeFile;
				
				//this can happen if we found the right includefile in the depgraph, but it hasn't been generated yet.
				if (!File.Exists(foundIncludeFile))
					continue;

				foreach (var file2 in GetFilesIncludedBy(foundIncludeFile))
					yield return file2;
			}
		}

		private string FindSpecifiedIncludeFileInSearchDirs(string includingFile, string includedFile)
		{
			var includeDirs = new List<string>() {Path.GetDirectoryName(includingFile)};
			includeDirs.AddRange(_includePaths);

			foreach(var dir in includeDirs)
			{
				var file_in_dir = Path.Combine(dir,Path.GetFileName(includedFile));
				if (File.Exists(file_in_dir))
					return file_in_dir;
				if (_willFileBeGeneratedAtPath(file_in_dir))
					return file_in_dir;
			}
			return null;
		}
	}
}