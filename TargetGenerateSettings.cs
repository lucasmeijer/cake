using System;
using System.Collections.Generic;

namespace cake
{
	public class TargetGenerateSettings
	{
		public ITargetGeneratingAction Action { get; private set; }
		public string ActionHash { get; private set; }
		public HashSet<string> InputFiles { get; private set; }
		public HashSet<string> OutputFiles { get; private set; }
		
		public TargetGenerateSettings(ITargetGeneratingAction action, IEnumerable<string> inputFiles, string outputFile)
			: this(action,inputFiles, new[] {outputFile})
		{
		}

		public TargetGenerateSettings(ITargetGeneratingAction action, IEnumerable<string> inputFiles, IEnumerable<string> outputFiles)
		{
			OutputFiles = new HashSet<string>(outputFiles);
			Action = action;
			ActionHash = Action.GetActionHash();
			InputFiles = new HashSet<string>(inputFiles);
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