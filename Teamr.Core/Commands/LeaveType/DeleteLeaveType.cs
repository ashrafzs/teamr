namespace Teamr.Core.Commands.LeaveType
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using MediatR;
	using TeamR.Core.DataAccess;
	using TeamR.Core.Security;
	using TeamR.Infrastructure;
	using TeamR.Infrastructure.Forms;
	using TeamR.Infrastructure.Security;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "delete-leave-type", PostOnLoad = true)]
	[Secure(typeof(CoreActions), nameof(CoreActions.ManageLeaveTypes))]
	public class DeleteLeaveType : MyAsyncForm<DeleteLeaveType.Request, DeleteLeaveType.Response>
	{
		private readonly CoreDbContext context;

		public DeleteLeaveType(CoreDbContext context)
		{
			this.context = context;
		}

		public override async Task<Response> Handle(Request request, CancellationToken cancellationToken)
		{
			var leaveType = await this.context.LeaveTypes
				.SingleOrExceptionAsync(s => s.Id == request.Id);

			if (this.context.Leaves.Any(a => a.LeaveTypeId == request.Id))
			{
				throw new BusinessException("You can not delete Leave type, because it's have linked leaves.");
			}


			this.context.LeaveTypes.Remove(leaveType);
			this.context.SaveChanges();


			return new Response();
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