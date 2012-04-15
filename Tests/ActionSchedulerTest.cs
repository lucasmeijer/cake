using System.Collections.Generic;
using System.Linq;
using System.Text;
using cake;
using cake.Tests;
using NUnit.Framework;

namespace cake.Tests
{
	[TestFixture]
	public class ActionSchedulerTest
	{
		ActionScheduler _scheduler;
		private SimpleAction _action;

		[SetUp]
		public void Setup()
		{
			_scheduler = new ActionScheduler();
			_action = new SimpleAction();
		}

		[Test]
		public void SingleActionWithoutDependenciesRequiringGenerationCanBeRun()
		{
			var tgs = new TargetGenerateSettings(_action, new[] {"input"}, "output");
			_scheduler.Add(new SchedulableAction(tgs));

			Assert.AreEqual(tgs,_scheduler.FindJobToRun());
		}

		[Test]
		public void DoesNotProvideSameJobTwice()
		{
			var tgs = new TargetGenerateSettings(_action, new[] { "input" }, "output");
			_scheduler.Add(new SchedulableAction(tgs));

			Assert.AreEqual(tgs, _scheduler.FindJobToRun());
			Assert.IsNull(_scheduler.FindJobToRun());
		}

		[Test]
		public void SingleActionWithDependenciesRequiringGenerationCannotBeRun()
		{
			var tgs = new TargetGenerateSettings(_action, new[] { "input" }, "output");
			_scheduler.Add(new SchedulableAction(tgs,new[] { "input"}));

			Assert.IsNull(_scheduler.FindJobToRun());
		}

		[Test]
		public void ActionWithDependenciesRequiringGenerationCanBeRanWhenDependencyIsGenerated()
		{
			var tgs = new TargetGenerateSettings(_action, new[] { "input" }, "output");
			_scheduler.Add(new SchedulableAction(tgs, new[] { "input" }));

			Assert.IsNull(_scheduler.FindJobToRun());

			var inputGenerator = new TargetGenerateSettings(_action, new string[0], "input");
			_scheduler.JobFinished(inputGenerator);

			Assert.AreEqual(tgs,_scheduler.FindJobToRun());
		}

	}
}
