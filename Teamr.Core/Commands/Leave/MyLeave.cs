namespace Teamr.Core.Commands.Leave
{
	using System;
	using System.Linq;
	using MediatR;
	using Microsoft.EntityFrameworkCore;
	using Teamr.Core.Domain;
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

	[MyForm(Id = "my-leave", PostOnLoad = true, Label = "My leave", Menu = CoreMenus.Main, MenuOrderIndex = 1)]
	[Secure(typeof(CoreActions), nameof(CoreActions.ViewActivities))]
	public class MyLeave : MyForm<MyLeave.Request, MyLeave.Response>
	{
		private readonly CoreDbContext dbContext;
		private readonly SystemPermissionManager permissionManager;
		private readonly UserContext userContext;

		public MyLeave(
			SystemPermissionManager permissionManager,
			UserContext userContext,
			CoreDbContext dbContext)
		{
			this.permissionManager = permissionManager;
			this.userContext = userContext;
			this.dbContext = dbContext;
		}

		protected override Response Handle(Request message)
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

		public class Response : MyFormResponse
		{
			[OutputField(OrderIndex = -10)]
			public ActionList Actions { get; set; }

			[PaginatedData(nameof(Request.Paginator), Label = "")]
			public PaginatedData<Item> Users { get; set; }
		}

		public class Item
		{
			public Item(Leave t, MyLeave cmd)
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