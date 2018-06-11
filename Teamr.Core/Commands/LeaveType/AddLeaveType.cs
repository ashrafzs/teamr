namespace Teamr.Core.Commands.LeaveType
{
	using System.Collections.Generic;
	using CPermissions;
	using MediatR;
	using Teamr.Core.DataAccess;
	using Teamr.Core.Domain;
	using Teamr.Core.Security;
	using Teamr.Infrastructure.Forms;
	using Teamr.Infrastructure.Security;
	using Teamr.Infrastructure.User;
	using UiMetadataFramework.Basic.Input;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "add-leave-type", Label = "Add leave type",
		SubmitButtonLabel = "Save changes")]
	public class AddLeaveType : IMyForm<AddLeaveType.Request, AddLeaveType.Response>, ISecureHandler
	{
		private readonly CoreDbContext context;
		private readonly UserContext userContext;

		public AddLeaveType(CoreDbContext context, UserContext userContext)
		{
			this.context = context;
			this.userContext = userContext;
		}

		public Response Handle(Request message)
		{
			if (message.Quantity != null)
			{
				var leaveType = new LeaveType(message.Name, this.userContext.User.UserId, message.Quantity.Value, message.Remarks?.Value);
				this.context.LeaveTypes.Add(leaveType);
				this.context.SaveChanges();
			}

			return new Response();
		}

		public UserAction GetPermission()
		{
			return CoreActions.ManageLeaveTypes;
		}

		public static FormLink Button()
		{
			return new FormLink
			{
				Label = "Add",
				Form = typeof(AddLeaveType).GetFormId(),
				InputFieldValues = new Dictionary<string, object>()
			};
		}

		public class Request : IRequest<Response>
		{
			[InputField(Required = true, OrderIndex = 2)]
			public string Name { get; set; }

			[InputField(Hidden = false, Required = true, OrderIndex = 5)]
			public decimal? Quantity { get; set; }

			[InputField(Required = false, OrderIndex = 50)]
			public TextareaValue Remarks { get; set; }
		}

		public class Response : FormResponse<MyFormResponseMetadata>
		{
		}
	}
}