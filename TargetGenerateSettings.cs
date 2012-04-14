using System;
using System.Collections.Generic;

namespace bs
{
	public class TargetGenerateSettings
	{
		public HashSet<string> InputFiles { get; private set; }
		public string ActionHash { get; private set; }

		public TargetGenerateSettings(HashSet<string> inputFiles, string actionHash)
		{
			InputFiles = inputFiles;
			ActionHash = actionHash;
		}

		//todo: figure out how to write a sane Equals
		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;
			
			var other = obj as TargetGenerateSettings;
			if (other == null) 
				return false;

			if (ActionHash != other.ActionHash) 
				return false;
			
			return other.InputFiles.SetEquals(InputFiles);
		}

		public override int GetHashCode()
		{
			return (InputFiles != null ? InputFiles.GetHashCode() : 0);
		}
	}
}