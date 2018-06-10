namespace Teamr.Core.Commands.Activity
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CPermissions;
	using MediatR;
	using Microsoft.EntityFrameworkCore;
	using Teamr.Core.DataAccess;
	using Teamr.Core.Menus;
	using Teamr.Core.Security;
	using Teamr.Infrastructure.EntityFramework;
	using Teamr.Infrastructure.Forms;
	using Teamr.Infrastructure.Security;
	using Teamr.Infrastructure.User;
	using UiMetadataFramework.Basic.Input;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.Core.Binding;
	using UiMetadataFramework.MediatR;
	

	[MyForm(Id = "user-points-by-day-report", PostOnLoad = true, Label = "User points by day report", Menu = CoreMenus.Reports, MenuOrderIndex = 1)]
	public class UserPointsByDayReport : IForm<UserPointsByDayReport.Request, UserPointsByDayReport.Response>, ISecureHandler
	{
		private readonly CoreDbContext dbContext;
		private readonly UserContext userContext;

		public UserPointsByDayReport(UserContext userContext,CoreDbContext dbContext)
		{
			this.userContext = userContext;
			this.dbContext = dbContext;
		}

		public Response Handle(Request message)
		{
			var allActivities = this.dbContext.Activities
				.Include(a => a.ActivityType)
				.Where(a => a.CreatedByUserId == this.userContext.User.UserId && a.PerformedOn != null)
				.OrderBy(t => t.Id)
				.GroupBy(t => t.ScheduledOn.Date);

			var activitiesList = new List<ActivityModel>();

			foreach (var activitiesByDate in allActivities)
			{
				var activityModel = new ActivityModel
				{
					Activities = activitiesByDate.Select(a => a.ActivityType.Name).ToList(),
					Date = activitiesByDate.Key,
					User = this.userContext,
					Points = activitiesByDate.Sum(a => a.Points)
				};

				activitiesList.Add(activityModel);
			}

			var result = activitiesList.AsQueryable().Paginate(t => new Item(t), message.Paginator);

			return new Response
			{
				Activities = result
			};
		}

		public UserAction GetPermission()
		{
			return CoreActions.ViewUserPointsByDayReport;
		}

		public class Request : IRequest<Response>
		{
			[InputField(OrderIndex = 0)]
			public DateTime? Day { get; set; }

			public Paginator Paginator { get; set; }
		}

		public class Response : FormResponse
		{
			[PaginatedData(nameof(Request.Paginator), Label = "", OrderIndex = 20)]
			public PaginatedData<Item> Activities { get; set; }
		}

		public class Item
		{
			public Item(ActivityModel t)
			{
				this.Activities = t.Activities;
				this.Date = UserDayReport.Button(t.Date, t.Date.ToString("MM/dd/yyyy"));
				this.User = UserProfile.Button(t.User.User.UserId, t.User.User.UserName);
				this.Points = t.Points;
			}

			[OutputField(OrderIndex = 3, Label = "Activities")]
			public IEnumerable<string> Activities { get; set; }

			[OutputField(OrderIndex = 1)]
			public FormLink Date { get; set; }

			[OutputField(OrderIndex = 5)]
			public decimal Points { get; set; }

			[OutputField(OrderIndex = 2)]
			public FormLink User { get; set; }
		}
	}

	public class ActivityModel
	{
		public IEnumerable<string> Activities { get; set; }
		public DateTime Date { get; set; }
		public decimal Points { get; set; }
		public UserContext User { get; set; }
	}
}