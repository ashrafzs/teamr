namespace Teamr.Core.Commands.Activity
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using MediatR;
	using Microsoft.EntityFrameworkCore;
	using TeamR.Core.DataAccess;
	using TeamR.Core.Menus;
	using TeamR.Core.Security;
	using TeamR.Infrastructure.EntityFramework;
	using TeamR.Infrastructure.Forms;
	using TeamR.Infrastructure.Security;
	using TeamR.Infrastructure.User;
	using UiMetadataFramework.Basic.Input;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "user-points-by-day-report", PostOnLoad = true, Label = "User points by day report", Menu = CoreMenus.Reports, MenuOrderIndex = 1)]
	[Secure(typeof(CoreActions), nameof(CoreActions.ViewUserPointsByDayReport))]
	public class UserPointsByDayReport : MyForm<UserPointsByDayReport.Request, UserPointsByDayReport.Response>
	{
		private readonly CoreDbContext dbContext;
		private readonly UserContext userContext;

		public UserPointsByDayReport(UserContext userContext, CoreDbContext dbContext)
		{
			this.userContext = userContext;
			this.dbContext = dbContext;
		}

		protected override Response Handle(Request message)
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

		public class Request : IRequest<Response>
		{
			[InputField(OrderIndex = 0)]
			public DateTime? Day { get; set; }

			public Paginator Paginator { get; set; }
		}

		public class Response : MyFormResponse
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