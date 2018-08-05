namespace Teamr.Core.Commands.LeaveType
{
	using System;
	using MediatR;
	using Microsoft.EntityFrameworkCore;
	using TeamR.Core.DataAccess;
	using TeamR.Core.Security;
	using TeamR.Help;
	using TeamR.Infrastructure;
	using TeamR.Infrastructure.EntityFramework;
	using TeamR.Infrastructure.Forms;
	using TeamR.Infrastructure.Security;
	using UiMetadataFramework.Basic.Input;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "leave-types", Label = "Leave types", PostOnLoad = true)]
	[Secure(typeof(CoreActions), nameof(CoreActions.ViewLeaveTypes))]
	[Documentation(DocumentationPlacement.Inline, DocumentationSourceType.String, "Show list of all available leave types that users can apply for.")]
	public class LeaveTypes : MyForm<LeaveTypes.Request, LeaveTypes.Response>
	{
		private readonly CoreDbContext context;

		public LeaveTypes(CoreDbContext context)
		{
			this.context = context;
		}

		public static FormLink Button(string label)
		{
			return new FormLink
			{
				Label = label,
				Form = typeof(LeaveTypes).GetFormId()
			};
		}

		protected override Response Handle(Request message)
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
				Actions = new ActionList(AddLeaveType.Button()),
				Tabs = TabstripUtility.GetConfigurationTabs<LeaveTypes>()
			};
		}

		public class Response : MyFormResponse
		{
			[OutputField(OrderIndex = 0)]
			public ActionList Actions { get; set; }

			[PaginatedData(nameof(Request.LeaveTypePaginator), OrderIndex = 10, Label = "")]
			public PaginatedData<LeaveTypeItem> Results { get; set; }

			[OutputField(OrderIndex = -10)]
			public Tabstrip Tabs { get; set; }
		}

		public class Request : IRequest<Response>
		{
			public Paginator LeaveTypePaginator { get; set; }
		}

		public class LeaveTypeItem
		{
			[OutputField(OrderIndex = 90, Label = "")]
			public ActionList Actions { get; set; }

			[OutputField(OrderIndex = 70, Label = "Created by")]
			public string CreatedBy { get; set; }

			[OutputField(OrderIndex = 60, Label = "Created on")]
			public DateTime CreatedOn { get; set; }

			[OutputField(OrderIndex = 10)]
			public string Name { get; set; }

			[OutputField(OrderIndex = 30)]
			[Documentation(
				DocumentationPlacement.Hint,
				DocumentationSourceType.String,
				"Number of days to be deducted from user's leave balance for each time the leave " +
				"type is used. **For full-day leave it should be 1, for half-day leave it should be 0.5**.")]
			public decimal Quantity { get; set; }

			[OutputField(OrderIndex = 50)]
			public string Remarks { get; set; }
		}
	}
}