namespace Teamr.Core.Commands.Activity
{
	using System;
	using System.Collections.Generic;
	using UiMetadataFramework.Core.Binding;

	public class UserCalendar
	{
		[OutputField(OrderIndex = 1)]
		public string User { get; set; }

		[OutputField(OrderIndex = 2)]
		public IEnumerable<CalendarEntry> Log { get; set; }
	}

	public class CalendarEntry
	{
		public bool IsLeave { get; set; }
		public string Tag { get; set; }
		public DateTime Date { get; set; }
		public string Activity { get; set; }
	}
}