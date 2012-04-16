using System.Collections.Generic;
using System.Linq;

namespace cake
{
	public class SchedulableAction
	{
		public TargetGenerateSettings Settings;
		public IEnumerable<string> InputFilesRequiringGeneration;

		public SchedulableAction(TargetGenerateSettings generateSettings)
			: this(generateSettings, new List<string>())
		{
		}

		public SchedulableAction(TargetGenerateSettings generateSettings, IEnumerable<string> inputFilesRequiringGeneration)
		{
			Settings = generateSettings;
			InputFilesRequiringGeneration = inputFilesRequiringGeneration;
		}

		public override bool Equals(System.Object obj)
		{
			if (obj == null)
				return false;

			var other = obj as SchedulableAction;
			if ((System.Object) other == null) //the cast here is required even if resharper thinks it isn't
				return false;

			bool equals1 = Equals(other.Settings, Settings);
			bool equals2 = other.InputFilesRequiringGeneration.SequenceEqual(InputFilesRequiringGeneration);
			return equals1 && equals2;
		}

		public override int GetHashCode()
		{
			return Settings.GetHashCode() ^ InputFilesRequiringGeneration.GetHashCode();
		}
	}
}