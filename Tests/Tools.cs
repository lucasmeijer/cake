using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace cake.Tests
{
	class Tools
	{
		public static void SetupCleanCurrentDirectory()
		{
			const string dirName = "Workspace";
			if (Directory.Exists(dirName))
				Directory.Delete(dirName,true);
			
			Directory.CreateDirectory(dirName);
			Directory.SetCurrentDirectory(dirName);
		}
	}
}
