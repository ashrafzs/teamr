namespace Teamr.Core.Commands.Activity
{
	using System.Linq;
	using CPermissions;
	using MediatR;
	using Teamr.Core.DataAccess;
	using Teamr.Core.Menus;
	using Teamr.Core.Security;
	using Teamr.Infrastructure.Forms;
	using Teamr.Infrastructure.Security;
	using UiMetadataFramework.Basic.Input;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.Core.Binding;
	using UiMetadataFramework.MediatR;

	[MyForm(Id = "calendar", PostOnLoad = true, Label = "Calendar", Menu = CoreMenus.Leave, MenuOrderIndex = 1)]
	public class UsersLeavesCalendar : IForm<UsersLeavesCalendar.Request, UsersLeavesCalendar.Response>, ISecureHandler
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

		public UsersLeavesCalendar(
			CoreDbContext dbContext,
			MetadataBinder metadataBinder)
		{
			this.dbContext = dbContext;
			this.metadataBinder = metadataBinder;
		}

		public Response Handle(Request message)
		{
			if (message.SelectMonth != null && message.SelectYear != null)
			{
				var users = this.dbContext.Users;

				var leaves = this.dbContext.Leaves.Where(u =>
					u.ScheduledOn.ToString("MMMM").Equals(message.SelectMonth.Value.ToString()) &&
					u.ScheduledOn.Year.ToString().Equals(message.SelectYear));
				var activities = this.dbContext.Activities.Where(u =>
					u.ScheduledOn.ToString("MMMM").Equals(message.SelectMonth.Value.ToString()) &&
					u.ScheduledOn.Year.ToString().Equals(message.SelectYear));

				return new Response
				{
					TeamSchedule = new TeamCalendar<UserSchedule>(
						users.Select(t => new UserSchedule
						{
							Name = t.Name,
							Month = message.SelectMonth.Value.ToString(),
							Year = message.SelectYear,
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

			return new Response();
		}

		public UserAction GetPermission()
		{
			return CoreActions.ViewActivities;
		}

		public class Request : IRequest<Response>
		{
			[InputField(OrderIndex = 1, Label = "Month")]
			public DropdownValue<MonthEnum> SelectMonth { get; set; }

			[InputField(OrderIndex = 1, Label = "Year")]
			public string SelectYear { get; set; }
		}

		public class Response : FormResponse
		{
			[OutputField(OrderIndex = 0, Label = "")]
			public TeamCalendar<UserSchedule> TeamSchedule { get; set; }
		}
	}
}