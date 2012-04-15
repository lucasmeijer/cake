using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace cake
{
	public delegate IEnumerable<string> ScanFileForIncludes(string file);

	public class IncludeScanner
	{
		static readonly Regex _regex = new Regex("^[ 	]*#[ 	]*include[ 	]*[<\"]([^\">]*)[\">].*$", RegexOptions.Multiline);

		public IEnumerable<string> Scan(string file)
		{
			return ScanText(File.ReadAllText(file));
		}

		public IEnumerable<string> ScanText(string fileContents)
		{
			var matches = _regex.Matches(fileContents);
			return from Match match in matches select match.Groups[1].ToString();
		}
	}
}