namespace Teamr.Core.Commands.Activity
{
	using System;
	using System.Linq;
	using CPermissions;
	using MediatR;
	using Microsoft.EntityFrameworkCore;
	using Teamr.Core.Domain;
	using Teamr.Core.Security.Activity;
	using TeamR.Core.DataAccess;
	using TeamR.Core.Security;
	using TeamR.Infrastructure.EntityFramework;
	using TeamR.Infrastructure.Forms;
	using TeamR.Infrastructure.Security;
	using TeamR.Infrastructure.User;
	using UiMetadataFramework.Basic.Input;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "activites", PostOnLoad = true, Label = "Activites", MenuOrderIndex = 1)]
	[Secure(typeof(CoreActions), nameof(CoreActions.ViewActivities))]
	public class Activities : MyForm<Activities.Request, Activities.Response>
	{
		private readonly ActivityPermissionManager activityPermissionManager;
		private readonly CoreDbContext dbContext;
		private readonly SystemPermissionManager permissionManager;
		private readonly UserContext userContext;

		public Activities(
			SystemPermissionManager permissionManager,
			UserContext userContext,
			CoreDbContext dbContext,
			ActivityPermissionManager activityPermissionManager)
		{
			this.permissionManager = permissionManager;
			this.userContext = userContext;
			this.dbContext = dbContext;
			this.activityPermissionManager = activityPermissionManager;
		}

		protected override Response Handle(Request message)
		{
			var query = this.dbContext.Activities
				.Include(a => a.ActivityType)
				.Where(a => a.CreatedByUserId == this.userContext.User.UserId)
				.AsNoTracking();

			var result = query
				.OrderBy(t => t.Id)
				.Paginate(t => new Item(t, this.activityPermissionManager.CanDo(ActivityAction.Edit, this.userContext, t)), message.Paginator);

			return new Response
			{
				Users = result,
				Actions = this.permissionManager.CanDo(CoreActions.ViewActivities, this.userContext)
					? new ActionList(AddCompletedActivity.Button(), AddPlannedActivity.Button())
					: null
			};
		}

		public UserAction GetPermission()
		{
			return CoreActions.ViewActivities;
		}

		public class Request : IRequest<Response>
		{
			[InputField(OrderIndex = 1)]
			public DateTime? On { get; set; }

			public Paginator Paginator { get; set; }
		}

		public class Response : MyFormResponse
		{
			[OutputField(OrderIndex = -10)]
			public ActionList Actions { get; set; }

			[PaginatedData(nameof(Request.Paginator), Label = "")]
			public PaginatedData<Item> Users { get; set; }
		}

		public class Item
		{
			public Item(Activity t, bool canManage)
			{
				this.Id = t.Id;
				this.Notes = t.Notes;
				this.PerformedOn = t.PerformedOn;
				this.ScheduledOn = t.ScheduledOn;
				this.ActivityType = t.ActivityType.Name;
				this.Quantity = t.Quantity;
				this.Actions = t.GetActions(canManage);
				this.Points = t.Points;
			}

			[OutputField(OrderIndex = 20)]
			public ActionList Actions { get; set; }

			[OutputField(OrderIndex = 2, Label = "Activity type")]
			public string ActivityType { get; set; }

			[OutputField(OrderIndex = 1, Hidden = true)]
			public int Id { get; set; }

			[OutputField(OrderIndex = 6)]
			public string Notes { get; set; }

			[OutputField(OrderIndex = 7, Label = "Performed on")]
			public DateTime? PerformedOn { get; set; }

			[OutputField(OrderIndex = 5)]
			public decimal Points { get; set; }

			[OutputField(OrderIndex = 4)]
			public decimal Quantity { get; set; }

			[OutputField(OrderIndex = 3, Label = "Scheduled on")]
			public DateTime? ScheduledOn { get; set; }
		}
	}
}