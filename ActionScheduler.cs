﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace cake
{
	public class ActionScheduler
	{
		private readonly Dictionary<TargetGenerateSettings, HashSet<string>> _scheduledActions = new Dictionary<TargetGenerateSettings, HashSet<string>>();

		public void Add(IEnumerable<string> dependenciesRequiringRegeneration, TargetGenerateSettings tgs)
		{
			_scheduledActions.Add(tgs, new HashSet<string>(dependenciesRequiringRegeneration));
		}

		public TargetGenerateSettings FindJobToRun()
		{
			var findJobToRun = _scheduledActions.Where(kvp => kvp.Value.Count == 0).Select(kvp=>kvp.Key).FirstOrDefault();
			if (findJobToRun!=null)
				_scheduledActions.Remove(findJobToRun);
			return findJobToRun;
		}

		public void JobFinished(TargetGenerateSettings finishedJob)
		{
			foreach(var outputFile in finishedJob.OutputFiles)
			{
				foreach (var deps in _scheduledActions.Values)
					deps.Remove(outputFile);
			}
		}
	}
}