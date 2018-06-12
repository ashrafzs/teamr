namespace Teamr.Core.Commands.Activity
{
	using System;
	using System.Linq;
	using CPermissions;
	using MediatR;
	using Microsoft.EntityFrameworkCore;
	using Teamr.Core.DataAccess;
	using Teamr.Core.Menus;
	using Teamr.Core.Pickers;
	using Teamr.Core.Security;
	using Teamr.Infrastructure.EntityFramework;
	using Teamr.Infrastructure.Forms;
	using Teamr.Infrastructure.Security;
	using UiMetadataFramework.Basic.Input;
	using UiMetadataFramework.Basic.Input.Typeahead;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.Core.Binding;
	using UiMetadataFramework.MediatR;

	[MyForm(Id = "activities-log", PostOnLoad = true, Label = "Activites log", Menu = CoreMenus.Reports, MenuOrderIndex = 1)]
	public class ActivitiesLog : IForm<ActivitiesLog.Request, ActivitiesLog.Response>, ISecureHandler
	{
		private readonly CoreDbContext dbContext;

		public ActivitiesLog(CoreDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public Response Handle(Request message)
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

		public UserAction GetPermission()
		{
			return CoreActions.ViewActivities;
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

		public class Response : FormResponse
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
				this.User = t.CreatedByUser.GetUserProfileLink();
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