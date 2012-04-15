using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cake
{
	class MissingHeaderException : Exception
	{
		public MissingHeaderException(string file, string includedFile) : base("Unable to find header: "+includedFile+" which is being included by "+file)
		{
		}
	}
}
