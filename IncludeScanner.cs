using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace bs
{
	internal class IncludeScanner
	{
		Regex _regex = new Regex("^[ 	]*#[ 	]*include[ 	]*[<\"]([^\">]*)[\">].*$", RegexOptions.Multiline);

		public IEnumerable<string> Scan(string file)
		{
			yield break;
		}

		public IEnumerable<string> ScanText(string fileContents)
		{
			var matches = _regex.Matches(fileContents);
			return from Match match in matches select match.Groups[1].ToString();
		}
	}
}