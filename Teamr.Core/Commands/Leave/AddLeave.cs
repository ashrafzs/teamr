namespace Teamr.Core.Commands.Leave
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using CPermissions;
	using MediatR;
	using Teamr.Core.Domain;
	using Teamr.Core.Pickers;
	using TeamR.Core.DataAccess;
	using TeamR.Core.Security;
	using TeamR.Infrastructure;
	using TeamR.Infrastructure.Forms;
	using TeamR.Infrastructure.Security;
	using TeamR.Infrastructure.User;
	using UiMetadataFramework.Basic.Input.Typeahead;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.Core.Binding;

	[MyForm(PostOnLoad = true, Id = "add-leave", Label = "Add leave")]
	[Secure(typeof(CoreActions), nameof(CoreActions.AddLeave))]
	public class AddLeave : MyAsyncForm<AddLeave.Request, AddLeave.Response>
	{
		private readonly CoreDbContext dbContext;
		private readonly UserContext userContext;

		public AddLeave(CoreDbContext dbContext, UserContext userContext)
		{
			this.dbContext = dbContext;
			this.userContext = userContext;
		}

		public override async Task<Response> Handle(Request request, CancellationToken cancellationToken)
		{
			var leaveType = await this.dbContext.LeaveTypes.FindOrExceptionAsync(request.LeaveTypeId.Value);
			var leave = new Leave(this.userContext.User.UserId, leaveType, request.Notes, request.ScheduledOn);
			this.dbContext.Leaves.Add(leave);
			await this.dbContext.SaveChangesAsync(cancellationToken);

			return new Response();
		}
		
		public static FormLink Button()
		{
			return new FormLink
			{
				Form = typeof(AddLeave).GetFormId(),
				Label = "Add Leave"
			};
		}

		public class Response : FormResponse<MyFormResponseMetadata>
		{
		}

		public class Request : IRequest<Response>
		{
			[TypeaheadInputField(typeof(LeaveTypeTypeaheadRemoteSource), Required = true, Label = "Leave Type")]
			public TypeaheadValue<int> LeaveTypeId { get; set; }

			[InputField(Label = "Notes", OrderIndex = 10)]
			public string Notes { get; set; }

			[InputField(OrderIndex = 40)]
			public DateTime ScheduledOn { get; set; }
		}
	}
}