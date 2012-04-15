using System.Collections.Generic;
using System.Linq;

namespace cake
{
	public class ActionScheduler
	{
		private Dictionary<TargetGenerateSettings, List<string>> _scheduledActions = new Dictionary<TargetGenerateSettings, List<string>>();

		public void Add(IEnumerable<string> dependenciesRequiringRegeneration, TargetGenerateSettings tgs)
		{
			_scheduledActions.Add(tgs, new List<string>(dependenciesRequiringRegeneration));
		}

		public TargetGenerateSettings FindJobToRun()
		{
			var findJobToRun = _scheduledActions.Where(kvp => kvp.Value.Count == 0).Select(kvp=>kvp.Key).FirstOrDefault();
			if (findJobToRun!=null)
				_scheduledActions.Remove(findJobToRun);
			return findJobToRun;
		}
	}
}