namespace Teamr.Core.Commands.Activity
{
	using System;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using CPermissions;
	using MediatR;
	using Microsoft.EntityFrameworkCore;
	using Teamr.Core.Commands.Leave;
	using Teamr.Core.Forms.Outputs;
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

	[MyForm(Id = "calendar", PostOnLoad = true, Label = "Team calendar", Menu = CoreMenus.Main, MenuOrderIndex = 1, SubmitButtonLabel = "Search")]
	[Secure(typeof(CoreActions), nameof(CoreActions.ViewActivities))]
	public class Calendar : MyAsyncForm<Calendar.Request, Calendar.Response>
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

		public Calendar(CoreDbContext dbContext)
		{
			this.dbContext = dbContext;
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

		public override async Task<Response> Handle(Request message, CancellationToken cancellationToken)
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

			var leaves = await this.dbContext.Leaves
				.Where(u => u.ScheduledOn >= minDate && u.ScheduledOn <= maxDate)
				.GroupBy(t => t.CreatedByUserId, u => new CalendarEntry
				{
					Date = u.ScheduledOn,
					Activity = u.LeaveType.Name,
					Tag = u.LeaveType.Tag,
					IsLeave = true
				})
				.ToListAsync(cancellationToken: cancellationToken);

			var activities = await this.dbContext.Activities
				.Where(u => u.ScheduledOn >= minDate && u.ScheduledOn <= maxDate)
				.GroupBy(t => t.CreatedByUserId, u => new CalendarEntry
				{
					Date = u.ScheduledOn,
					Activity = u.ActivityType.Name,
					Tag = u.ActivityType.Tag
				})
				.ToListAsync(cancellationToken: cancellationToken);

			var schedules = users
				.Select(t => new UserCalendar
				{
					UserId = t.Id,
					UserName = t.Name,
					Log = leaves
						.Where(x => x.Key == t.Id)
						.Union(activities.Where(u => u.Key == t.Id))
						.SelectMany(x => x.ToList())
				})
				.ToList();

			return new Response
			{
				TeamSchedule = new TeamCalendar(year, month, schedules),
				Actions = new ActionList(
					AddLeave.Button(), 
					AddCompletedActivity.Button(), 
					AddPlannedActivity.Button())
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
			[OutputField(OrderIndex = -10)]
			public ActionList Actions { get; set; }

			[OutputField(OrderIndex = 0, Label = "")]
			public TeamCalendar TeamSchedule { get; set; }
		}
	}
}