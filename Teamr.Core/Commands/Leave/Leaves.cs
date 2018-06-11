namespace Teamr.Core.Commands.Leave
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

	[MyForm(Id = "leaves", PostOnLoad = true, Label = "Leaves", Menu = CoreMenus.Leave, MenuOrderIndex = 1)]
	public class Leaves : IForm<Leaves.Request, Leaves.Response>, ISecureHandler
	{
		private readonly CoreDbContext dbContext;
		private readonly SystemPermissionManager permissionManager;
		private readonly UserContext userContext;

		public Leaves(
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
			var query = this.dbContext.Leaves
				.Include(a => a.LeaveType)
				.Where(a => a.CreatedByUserId == this.userContext.User.UserId)
				.AsNoTracking();

			//if (message.Id != null)
			//{
			//	query = query.Where(u => u.Id.Equals(message.Id));
			//}

			var result = query
				.OrderBy(t => t.Id)
				.Paginate(t => new Item(t, this), message.Paginator);

			return new Response
			{
				Users = result,
				Actions = this.permissionManager.CanDo(CoreActions.ViewActivities, this.userContext)
					? new ActionList(AddLeave.Button())
					: null
			};
		}

		public UserAction GetPermission()
		{
			return CoreActions.ViewActivities;
		}

		private ActionList GetActions(Leave leave)
		{
			var result = new ActionList();
			result.Actions.Add(EditLeave.Button(leave.Id));
			result.Actions.Add(DeleteLeave.Button(leave.Id));

			return result;
		}

		public class Request : IRequest<Response>
		{
			//[InputField(OrderIndex = 0)]
			//public int? Id { get; set; }

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
			public Item(Leave t, Leaves cmd)
			{
				this.Id = t.Id;
				this.Notes = t.Notes;
				this.ScheduledOn = t.ScheduledOn;
				this.LeaveType = t.LeaveType.Name;
				this.Actions = cmd.GetActions(t);
			}

			[OutputField(OrderIndex = 20)]
			public ActionList Actions { get; set; }

			[OutputField(OrderIndex = 1)]
			public int Id { get; set; }

			[OutputField(OrderIndex = 2, Label = "Leave type")]
			public string LeaveType { get; set; }

			[OutputField(OrderIndex = 6)]
			public string Notes { get; set; }

			[OutputField(OrderIndex = 8, Label = "Scheduled on")]
			public DateTime? ScheduledOn { get; set; }
		}
	}
}