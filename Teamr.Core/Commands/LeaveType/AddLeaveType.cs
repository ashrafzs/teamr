namespace Teamr.Core.Commands.LeaveType
{
	using System.Collections.Generic;
	using MediatR;
	using Teamr.Core.Domain;
	using TeamR.Core.DataAccess;
	using TeamR.Core.Security;
	using TeamR.Infrastructure.Forms;
	using TeamR.Infrastructure.Security;
	using TeamR.Infrastructure.User;
	using UiMetadataFramework.Basic.Input;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "add-leave-type", Label = "Add leave type", SubmitButtonLabel = "Save changes")]
	[Secure(typeof(CoreActions), nameof(CoreActions.ManageLeaveTypes))]
	public class AddLeaveType : MyForm<AddLeaveType.Request, AddLeaveType.Response>
	{
		private readonly CoreDbContext context;
		private readonly UserContext userContext;

		public AddLeaveType(CoreDbContext context, UserContext userContext)
		{
			this.context = context;
			this.userContext = userContext;
		}

		protected override Response Handle(Request message)
		{
			if (message.Quantity != null)
			{
				var leaveType = new LeaveType(message.Name, this.userContext.User.UserId, message.Quantity.Value, message.Remarks?.Value,message.Tag);
				this.context.LeaveTypes.Add(leaveType);
				this.context.SaveChanges();
			}

			return new Response();
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

			[InputField(Required = true, OrderIndex = 3)]
			public string Tag { get; set; }

			[InputField(Hidden = false, Required = true, OrderIndex = 5)]
			public decimal? Quantity { get; set; }

			[InputField(Required = false, OrderIndex = 50)]
			public TextareaValue Remarks { get; set; }
		}

		public class Response : MyFormResponse
		{
		}
	}
}