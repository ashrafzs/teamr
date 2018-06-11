namespace Teamr.Core.Commands.Leave
{
	using System;
	using System.Threading.Tasks;
	using CPermissions;
	using MediatR;
	using Teamr.Core.DataAccess;
	using Teamr.Core.Domain;
	using Teamr.Core.Pickers;
	using Teamr.Core.Security;
	using Teamr.Infrastructure.Forms;
	using Teamr.Infrastructure.Security;
	using Teamr.Infrastructure.User;
	using UiMetadataFramework.Basic.Input.Typeahead;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.Core.Binding;

	[MyForm(PostOnLoad = true, Id = "add-leave", Label = "Add leave")]
	public class AddLeave : IMyAsyncForm<AddLeave.Request, AddLeave.Response>, ISecureHandler
	{
		private readonly CoreDbContext dbContext;
		private readonly UserContext userContext;

		public AddLeave(CoreDbContext dbContext, UserContext userContext)
		{
			this.dbContext = dbContext;
			this.userContext = userContext;
		}

		public async Task<Response> Handle(Request message)
		{
			var leaveType = await this.dbContext.LeaveTypes.FindOrExceptionAsync(message.LeaveTypeId.Value);
			var leave = new Leave(this.userContext.User.UserId, leaveType, message.Notes, message.ScheduledOn);
			this.dbContext.Leaves.Add(leave);
			await this.dbContext.SaveChangesAsync();

			return new Response();
		}

		public UserAction GetPermission()
		{
			return CoreActions.AddLeave;
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