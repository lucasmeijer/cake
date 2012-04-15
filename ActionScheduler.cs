using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace cake
{
	public class ActionScheduler
	{
		private readonly Dictionary<TargetGenerateSettings, HashSet<string>> _scheduledActions = new Dictionary<TargetGenerateSettings, HashSet<string>>();

		public bool AnyJobsLeft
		{
			get { return _scheduledActions.Any(); }
		}

		public void Add(SchedulableAction schedulableAction)
		{
			_scheduledActions.Add(schedulableAction.Settings, new HashSet<string>(schedulableAction.InputFilesRequiringGeneration));
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

		public void VerifyAllInputFilesArePresentOrWillBeGenerated()
		{
			foreach(var scheduledAction in _scheduledActions.Keys)
			{
				foreach(var inputFile in scheduledAction.InputFiles)
				{
					if (File.Exists(inputFile))
						continue;
					if (_scheduledActions.Keys.Any(sa=>sa.OutputFiles.Contains(inputFile)))
						continue;
					throw new MissingDependencyException();
				}
			}
		}
	}
}