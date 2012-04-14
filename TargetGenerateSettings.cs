using System;
using System.Collections.Generic;

namespace cake
{
	public class TargetGenerateSettings
	{
		public ITargetGeneratingAction Action;
		public HashSet<string> InputFiles { get; private set; }

		private string _actionHash;
		
		public TargetGenerateSettings(ITargetGeneratingAction action, IEnumerable<string> inputFiles)
		{
			Action = action;
			_actionHash = action.GetActionHash();
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

		public string ActionHash
		{
			get
			{
				if (Action != null)
					return Action.GetActionHash();
				return _actionHash;
			}
		}

		public override int GetHashCode()
		{
			return (InputFiles != null ? InputFiles.GetHashCode() : 0);
		}
	}
}