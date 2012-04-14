using System.Collections.Generic;

namespace bs
{
	public class TargetBuildSettings
	{
		public HashSet<string> InputFiles;

		//todo: figure out how to write a sane Equals
		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			var other = obj as TargetBuildSettings;
			if (other == null) return false;
			return other.InputFiles.Equals(InputFiles);
		}

		public override int GetHashCode()
		{
			return (InputFiles != null ? InputFiles.GetHashCode() : 0);
		}
	}
}