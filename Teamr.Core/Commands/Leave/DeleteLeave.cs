namespace Teamr.Core.Commands.Leave
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using CPermissions;
	using MediatR;
	using Teamr.Core.DataAccess;
	using Teamr.Core.Domain;
	using Teamr.Core.Security.Leave;
	using Teamr.Infrastructure;
	using Teamr.Infrastructure.Forms;
	using Teamr.Infrastructure.Security;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "delete-Leave", PostOnLoad = true)]
	public class DeleteLeave : IMyAsyncForm<DeleteLeave.Request, DeleteLeave.Response>,
		IAsyncSecureHandler<Leave, DeleteLeave.Request, DeleteLeave.Response>
	{
		private readonly CoreDbContext dbContext;

		public DeleteLeave(CoreDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public UserAction<Leave> GetPermission()
		{
			return LeaveAction.Delete;
		}

		public async Task<Response> Handle(Request message)
		{
			var leave = await this.dbContext.Leaves.SingleOrExceptionAsync(t => t.Id == message.Id);

			this.dbContext.Leaves.Remove(leave);
			await this.dbContext.SaveChangesAsync();

			return new Response();
		}

		public static FormLink Button(int userId)
		{
			return new FormLink
			{
				Form = typeof(DeleteLeave).GetFormId(),
				Label = UiFormConstants.DeleteLabel,
				InputFieldValues = new Dictionary<string, object>
				{
					{ nameof(Request.Id), userId }
				}
			}.WithAction(FormLinkActions.Run).WithCssClass("btn-danger btn-icon");
		}

		public class Response : FormResponse<MyFormResponseMetadata>
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