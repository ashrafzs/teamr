namespace Teamr.Core.Commands.LeaveType
{
	using System;
	using CPermissions;
	using MediatR;
	using Microsoft.EntityFrameworkCore;
	using Teamr.Core.DataAccess;
	using Teamr.Core.Menus;
	using Teamr.Core.Security;
	using Teamr.Infrastructure;
	using Teamr.Infrastructure.EntityFramework;
	using Teamr.Infrastructure.Forms;
	using Teamr.Infrastructure.Security;
	using UiMetadataFramework.Basic.Input;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.Core.Binding;
	using UiMetadataFramework.MediatR;

	[MyForm(Menu = CoreMenus.Leave, Id = "leave-types", Label = "Leave types", PostOnLoad = true)]
	public class LeaveTypes : IForm<LeaveTypes.Request, LeaveTypes.Response>,
		ISecureHandler
	{
		private readonly CoreDbContext context;

		public LeaveTypes(CoreDbContext context)
		{
			this.context = context;
		}

		public Response Handle(Request message)
		{
			var leaveTypes = this.context.LeaveTypes
				.Include(i => i.User)
				.AsNoTracking()
				.Paginate(t => t, message.LeaveTypePaginator);
			return new Response
			{
				Results = leaveTypes.Transform(s => new LeaveTypeItem
				{
					Quantity = s.Quantity,
					Name = s.Name,
					CreatedBy = s.User?.Name,
					CreatedOn = s.CreatedOn,
					Remarks = s.Remarks,
					Actions = new ActionList(EditLeaveType.Button(s.Id), DeleteLeaveType.Button(s.Id))
				}),
				Actions = new ActionList(AddLeaveType.Button())
			};
		}

		public UserAction GetPermission()
		{
			return CoreActions.ViewLeaveTypes;
		}

		public class Response : FormResponse
		{
			[OutputField(OrderIndex = 0)]
			public ActionList Actions { get; set; }

			[PaginatedData(nameof(Request.LeaveTypePaginator), OrderIndex = 10, Label = "")]
			public PaginatedData<LeaveTypeItem> Results { get; set; }
		}

		public class Request : IRequest<Response>
		{
			public Paginator LeaveTypePaginator { get; set; }
		}

		public class LeaveTypeItem
		{

			[OutputField(OrderIndex = 70, Label = "Created by")]
			public string CreatedBy { get; set; }

			[OutputField(OrderIndex = 60, Label = "Created on")]
			public DateTime CreatedOn { get; set; }

			[OutputField(OrderIndex = 10)]
			public string Name { get; set; }

			[OutputField(OrderIndex = 30)]
			public decimal Quantity { get; set; }

			[OutputField(OrderIndex = 50)]
			public string Remarks { get; set; }

			[OutputField(OrderIndex = 90)]
			public ActionList Actions { get; set; }
		}
	}
}