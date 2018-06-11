namespace Teamr.Core.Commands.LeaveType
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using CPermissions;
	using MediatR;
	using Teamr.Core.DataAccess;
	using Teamr.Core.Security;
	using Teamr.Infrastructure;
	using Teamr.Infrastructure.Forms;
	using Teamr.Infrastructure.Security;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "delete-leave-type", PostOnLoad = true)]
	public class DeleteLeaveType : IMyAsyncForm<DeleteLeaveType.Request, DeleteLeaveType.Response>, ISecureHandler
	{
		private readonly CoreDbContext context;

		public DeleteLeaveType(CoreDbContext context)
		{
			this.context = context;
		}

		public async Task<Response> Handle(Request message)
		{
			var leaveType = await this.context.LeaveTypes
				.SingleOrExceptionAsync(s => s.Id == message.Id);

			if (this.context.Leaves.Any(a => a.LeaveTypeId == message.Id))
			{
				throw new BusinessException("You can not delete Leave type, because it's have linked leaves.");
			}


			this.context.LeaveTypes.Remove(leaveType);
			this.context.SaveChanges();


			return new Response();
		}

		public UserAction GetPermission()
		{
			return CoreActions.ManageLeaveTypes;
		}

		public static FormLink Button(int id)
		{
			return new FormLink
			{
				Label = UiFormConstants.DeleteLabel,
				Form = typeof(DeleteLeaveType).GetFormId(),
				InputFieldValues = new Dictionary<string, object>
				{
					{ nameof(Request.Id), id }
				}
			}.WithAction(FormLinkActions.Run).WithCssClass("btn-danger btn-icon");
		}

		public class Request : IRequest<Response>, ISecureHandlerRequest
		{
			[InputField(Hidden = true)]
			public int Id { get; set; }

			[NotField]
			public int ContextId => this.Id;
		}

		public class Response : FormResponse<MyFormResponseMetadata>
		{
		}
	}
}