namespace Teamr.Core.Commands.Activity
{
	using System;
	using System.Linq;
	using CPermissions;
	using MediatR;
	using Microsoft.EntityFrameworkCore;
	using Teamr.Core.Pickers;
	using TeamR.Core;
	using TeamR.Core.DataAccess;
	using TeamR.Core.Menus;
	using TeamR.Core.Security;
	using TeamR.Infrastructure.Forms;
	using TeamR.Infrastructure.Security;
	using UiMetadataFramework.Basic.Input.Dropdown;
	using UiMetadataFramework.Basic.Input.Typeahead;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "calendar", PostOnLoad = true, Label = "Calendar", Menu = CoreMenus.Main, MenuOrderIndex = 1)]
	[Secure(typeof(CoreActions), nameof(CoreActions.ViewActivities))]
	public class Calendar : MyForm<Calendar.Request, Calendar.Response>
	{
		public enum MonthEnum
		{
			January = 1,
			February = 2,
			March = 3,
			April = 4,
			May = 5,
			June = 6,
			July = 7,
			August = 8,
			September = 9,
			October = 10,
			November = 11,
			December = 12
		}

		private readonly CoreDbContext dbContext;
		private readonly MetadataBinder metadataBinder;

		public Calendar(
			CoreDbContext dbContext,
			MetadataBinder metadataBinder)
		{
			this.dbContext = dbContext;
			this.metadataBinder = metadataBinder;
		}

		public static FormLink Button(string label)
		{
			return new FormLink
			{
				Label = label,
				Form = typeof(Calendar).GetFormId()
			};
		}

		public UserAction GetPermission()
		{
			return CoreActions.ViewActivities;
		}

		protected override Response Handle(Request message)
		{
			var year = message.Year ?? DateTime.Today.Year;
			var month = message.Month?.Value != null ? (int)message.Month.Value.Value : DateTime.Today.Month;

			var users = this.dbContext.Users.AsNoTracking();

			if (message.UserIds?.Items?.Count > 0)
			{
				users = users.Where(u => message.UserIds.Items.Contains(u.Id));
			}

			var minDate = DateTimeUtils.StartOfMonth(year, month);
			var maxDate = DateTimeUtils.EndOfMonth(year, month);

			var leaves = this.dbContext.Leaves
				.Where(u => u.ScheduledOn >= minDate && u.ScheduledOn <= maxDate);

			var activities = this.dbContext.Activities
				.Where(u => u.ScheduledOn >= minDate && u.ScheduledOn <= maxDate);

			return new Response
			{
				TeamSchedule = new TeamCalendar<UserSchedule>(
					users.Select(t => new UserSchedule
					{
						Name = t.Name,
						Month = message.Month.Value.ToString(),
						Year = year,
						Schedules = leaves.Where(u => u.CreatedByUserId == t.Id).Select(u => new Schedule
						{
							Day = u.ScheduledOn.Day,
							Event = u.LeaveType.Name,
							Tag = u.LeaveType.Tag
						}).Union(activities.Where(u => u.CreatedByUserId == t.Id).Select(u => new Schedule
						{
							Day = u.ScheduledOn.Day,
							Event = u.ActivityType.Name,
							Tag = u.ActivityType.Tag
						})).ToList()
					}), this.metadataBinder)
			};
		}

		public class Request : IRequest<Response>
		{
			[InputField(OrderIndex = 1, Label = "Month")]
			public DropdownValue<MonthEnum?> Month { get; set; }

			[TypeaheadInputField(typeof(UserTypeaheadRemoteSource), Label = "Users")]
			public MultiSelect<int> UserIds { get; set; }

			[InputField(OrderIndex = 1, Label = "Year")]
			public int? Year { get; set; }
		}

		public class Response : MyFormResponse
		{
			[OutputField(OrderIndex = 0, Label = "")]
			public TeamCalendar<UserSchedule> TeamSchedule { get; set; }
		}
	}
}