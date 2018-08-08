namespace Teamr.Core.Forms.Outputs
{
	using System;
	using System.Collections.Generic;
	using UiMetadataFramework.Core.Binding;

	[OutputFieldType("calendar")]
	public class TeamCalendar
	{
		public TeamCalendar(int year, int month, List<UserCalendar> schedules)
		{
			this.Year = year;
			this.Month = month;
			this.UserCalendars = schedules;
		}

		public IEnumerable<UserCalendar> UserCalendars { get; set; }
		public int Month { get; }
		public int Year { get; }
	}

	public class UserCalendar
	{
		public string UserName { get; set; }
		public IEnumerable<CalendarEntry> Log { get; set; }
		public int UserId { get; set; }
	}

	public class CalendarEntry
	{
		public bool IsLeave { get; set; }
		public string Tag { get; set; }
		public DateTime Date { get; set; }
		public string Activity { get; set; }
	}
}