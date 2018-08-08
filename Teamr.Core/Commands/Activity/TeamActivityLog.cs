namespace Teamr.Core.Commands.Activity
{
	using System;
	using System.Linq;
	using MediatR;
	using Microsoft.EntityFrameworkCore;
	using Teamr.Core.Pickers;
	using TeamR.Core.DataAccess;
	using TeamR.Core.Menus;
	using TeamR.Core.Security;
	using TeamR.Infrastructure.EntityFramework;
	using TeamR.Infrastructure.Forms;
	using TeamR.Infrastructure.Security;
	using UiMetadataFramework.Basic.Input;
	using UiMetadataFramework.Basic.Input.Typeahead;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "team-activity-log", PostOnLoad = true, Label = "Team activity log", Menu = CoreMenus.Reports, MenuOrderIndex = 1)]
	[Secure(typeof(CoreActions), nameof(CoreActions.ViewActivities))]
	public class TeamActivityLog : MyForm<TeamActivityLog.Request, TeamActivityLog.Response>
	{
		private readonly CoreDbContext dbContext;

		public TeamActivityLog(CoreDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		protected override Response Handle(Request message)
		{
			var query = this.dbContext.Activities
				.Include(a => a.ActivityType)
				.Include(a => a.CreatedByUser)
				.AsNoTracking();

			if (message.Id != null)
			{
				query = query.Where(u => u.Id.Equals(message.Id));
			}

			if (message.ActivityTypeId?.Items?.Count > 0)
			{
				query = query.Where(u => message.ActivityTypeId.Items.Contains(u.ActivityTypeId));
			}

			if (message.UsersId?.Items?.Count > 0)
			{
				query = query.Where(u => message.UsersId.Items.Contains(u.CreatedByUserId));
			}

			var data = query
				.OrderBy(t => t.Id)
				.Paginate(t => new Activity(t), message.Paginator);

			return new Response
			{
				Activities = data,
			};
		}

		public class Request : IRequest<Response>
		{
			[TypeaheadInputField(typeof(ActivityTypeTypeaheadRemoteSource), Label = "Activity Type")]
			public MultiSelect<int> ActivityTypeId { get; set; }

			[InputField(OrderIndex = 0)]
			public int? Id { get; set; }

			public Paginator Paginator { get; set; }

			[TypeaheadInputField(typeof(UserTypeaheadRemoteSource), Label = "User")]
			public MultiSelect<int> UsersId { get; set; }
		}

		public class Response : MyFormResponse
		{
			[PaginatedData(nameof(Request.Paginator), Label = "")]
			public PaginatedData<Activity> Activities { get; set; }
		}

		public class Activity
		{
			public Activity(Domain.Activity t)
			{
				this.Id = t.Id;
				this.Notes = t.Notes;
				this.PerformedOn = t.PerformedOn;
				this.ScheduledOn = t.ScheduledOn;
				this.ActivityType = t.ActivityType.Name;
				this.Quantity = t.Quantity;
				this.Points = t.Points;
				this.User = UserProfile.Button(t.CreatedByUser.Id, t.CreatedByUser.Name);
			}

			[OutputField(OrderIndex = 2, Label = "Activity type")]
			public string ActivityType { get; set; }

			[OutputField(OrderIndex = 1)]
			public int Id { get; set; }

			[OutputField(OrderIndex = 6)]
			public string Notes { get; set; }

			[OutputField(OrderIndex = 7, Label = "Performed on")]
			public DateTime? PerformedOn { get; set; }

			[OutputField(OrderIndex = 5)]
			public decimal Points { get; set; }

			[OutputField(OrderIndex = 4)]
			public decimal Quantity { get; set; }

			[OutputField(OrderIndex = 8, Label = "Scheduled on")]
			public DateTime? ScheduledOn { get; set; }

			[OutputField(OrderIndex = 2)]
			public FormLink User { get; set; }
		}
	}
}