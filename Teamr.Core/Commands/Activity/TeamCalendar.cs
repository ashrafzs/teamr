namespace Teamr.Core.Commands.Activity
{
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
}