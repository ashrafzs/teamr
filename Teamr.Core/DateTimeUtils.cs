namespace TeamR.Core
{
	using System;

	public static class DateTimeUtils
	{
		/// <summary>
		/// Gets the 11:59:59 instance of a DateTime
		/// </summary>
		public static DateTime AbsoluteEnd(this DateTime dateTime)
		{
			return AbsoluteStart(dateTime).AddDays(1).AddTicks(-1);
		}

		/// <summary>
		/// Gets the 12:00:00 instance of a DateTime
		/// </summary>
		public static DateTime AbsoluteStart(this DateTime dateTime)
		{
			return dateTime.Date;
		}

		public static DateTime EndOfMonth(this DateTime date)
		{
			return EndOfMonth(date.Year, date.Month);
		}

		public static DateTime EndOfMonth(int year, int month)
		{
			return new DateTime(year, month, 1).AddMonths(1).Date.AddTicks(-1);
		}

		public static DateTime StartOfMonth(this DateTime date)
		{
			return StartOfMonth(date.Year, date.Month);
		}

		public static DateTime StartOfMonth(int year, int month)
		{
			return new DateTime(year, month, 1).Date;
		}
	}
}