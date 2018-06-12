namespace Teamr.Core.Commands.Activity
{
	using System;
	using System.Linq;
	using CPermissions;
	using MediatR;
	using Microsoft.EntityFrameworkCore;
	using Teamr.Core.DataAccess;
	using Teamr.Core.Domain;
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

	[MyForm(Id = "activites", PostOnLoad = true, Label = "Activites", Menu = CoreMenus.Activity, MenuOrderIndex = 1)]
	public class MemberActivities : IForm<MemberActivities.Request, MemberActivities.Response>, ISecureHandler
	{
		private readonly CoreDbContext dbContext;
		private readonly SystemPermissionManager permissionManager;
		private readonly UserContext userContext;

		public MemberActivities(
			SystemPermissionManager permissionManager,
			UserContext userContext,
			CoreDbContext dbContext)
		{
			this.permissionManager = permissionManager;
			this.userContext = userContext;
			this.dbContext = dbContext;
		}

		public Response Handle(Request message)
		{
			var query = this.dbContext.Activities
				.Include(a => a.ActivityType)
				.Where(a => a.CreatedByUserId == this.userContext.User.UserId)
				.AsNoTracking();

			if (message.Id != null)
			{
				query = query.Where(u => u.Id.Equals(message.Id));
			}

			var result = query
				.OrderBy(t => t.Id)
				.Paginate(t => new Item(t, this), message.Paginator);

			return new Response
			{
				Users = result,
				Actions = this.permissionManager.CanDo(CoreActions.ViewActivities, this.userContext)
					? new ActionList(AddCompletedActivity.Button())
					: null
			};
		}

		public UserAction GetPermission()
		{
			return CoreActions.ViewActivities;
		}

		private ActionList GetActions(Activity activity)
		{
			var result = new ActionList();
			result.Actions.Add(EditActivity.Button(activity.Id));
			result.Actions.Add(DeleteActivity.Button(activity.Id));

			return result;
		}

		public class Request : IRequest<Response>
		{
			[InputField(OrderIndex = 0)]
			public int? Id { get; set; }

			[InputField(OrderIndex = 1)]
			public DateTime? On { get; set; }

			public Paginator Paginator { get; set; }
		}

		public class Response : FormResponse
		{
			[OutputField(OrderIndex = -10)]
			public ActionList Actions { get; set; }

			[PaginatedData(nameof(Request.Paginator), Label = "")]
			public PaginatedData<Item> Users { get; set; }
		}

		public class Item
		{
			public Item(Activity t, MemberActivities cmd)
			{
				this.Id = t.Id;
				this.Notes = t.Notes;
				this.PerformedOn = t.PerformedOn;
				this.ScheduledOn = t.ScheduledOn;
				this.ActivityType = t.ActivityType.Name;
				this.Quantity = t.Quantity;
				this.Actions = cmd.GetActions(t);
				this.Points = t.Points;
			}

			[OutputField(OrderIndex = 20)]
			public ActionList Actions { get; set; }

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

			[OutputField(OrderIndex = 3, Label = "Scheduled on")]
			public DateTime? ScheduledOn { get; set; }
		}
	}
}