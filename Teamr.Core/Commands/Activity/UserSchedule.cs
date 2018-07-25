namespace Teamr.Core.Commands.Activity
{
	using System.Collections.Generic;
	using UiMetadataFramework.Core.Binding;

	public class UserSchedule
	{
		[OutputField(OrderIndex = 1)]
		public string Month { get; set; }

		[OutputField(OrderIndex = 1)]
		public string Name { get; set; }

		[OutputField(OrderIndex = 2)]
		public IEnumerable<Schedule> Schedules { get; set; }

		[OutputField(OrderIndex = 1)]
		public string Year { get; set; }
	}

	public class Schedule
	{
		public string Name { get; set; }
		public string Tag { get; set; }
		public int Day { get; set; }
		public string Event { get; set; }
	}
}