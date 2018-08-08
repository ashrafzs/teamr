namespace Teamr.Core.Commands.Leave
{
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using MediatR;
	using Teamr.Core.Security.Leave;
	using TeamR.Core.DataAccess;
	using TeamR.Infrastructure;
	using TeamR.Infrastructure.Forms;
	using TeamR.Infrastructure.Security;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "delete-Leave", PostOnLoad = true)]
	[Secure(typeof(LeaveAction), nameof(LeaveAction.Delete))]
	public class DeleteLeave : MyAsyncForm<DeleteLeave.Request, DeleteLeave.Response>
	{
		private readonly CoreDbContext dbContext;

		public DeleteLeave(CoreDbContext dbContext)
		{
			this.dbContext = dbContext;
		}
		
		public override async Task<Response> Handle(Request request, CancellationToken cancellationToken)
		{
			var leave = await this.dbContext.Leaves.SingleOrExceptionAsync(t => t.Id == request.Id);

			this.dbContext.Leaves.Remove(leave);
			await this.dbContext.SaveChangesAsync(cancellationToken);

			return new Response();
		}

		public static FormLink Button(int leaveId)
		{
			return new FormLink
			{
				Form = typeof(DeleteLeave).GetFormId(),
				Label = UiFormConstants.DeleteLabel,
				InputFieldValues = new Dictionary<string, object>
				{
					{ nameof(Request.Id), leaveId }
				}
			}.WithAction(FormLinkActions.Run).WithCssClass("btn-danger btn-icon");
		}

		public class Response : MyFormResponse
		{
		}

		public class Request : IRequest<Response>, ISecureHandlerRequest
		{
			[InputField(Hidden = true)]
			public int Id { get; set; }

			[NotField]
			public int ContextId => this.Id;
		}
	}
}