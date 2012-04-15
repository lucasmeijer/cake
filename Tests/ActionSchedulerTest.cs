using System.Collections.Generic;
using System.Linq;
using System.Text;
using cake;
using cake.Tests;
using NUnit.Framework;

namespace bs.Tests
{
	[TestFixture]
	public class ActionSchedulerTest
	{
		[Test]
		[Ignore]
		public void Test()
		{
			var scheduler = new ActionScheduler();

			var action = new SimpleAction(s => { });
			var tgs = new TargetGenerateSettings(action, new[] {"input"}, "output");
			scheduler.Add("output", tgs, new string[0]);

			var jobToRun = scheduler.FindJobToRun();

		}
	}
}
