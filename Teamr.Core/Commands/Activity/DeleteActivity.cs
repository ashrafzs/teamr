namespace Teamr.Core.Commands.Activity
{
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using MediatR;
	using Teamr.Core.Security.Activity;
	using TeamR.Core.DataAccess;
	using TeamR.Infrastructure;
	using TeamR.Infrastructure.Forms;
	using TeamR.Infrastructure.Security;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "delete-activity", PostOnLoad = true)]
	[Secure(typeof(ActivityAction), nameof(ActivityAction.Delete))]
	public class DeleteActivity : MyAsyncForm<DeleteActivity.Request, DeleteActivity.Response>
	{
		private readonly CoreDbContext dbContext;

		public DeleteActivity(CoreDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public override async Task<Response> Handle(Request request, CancellationToken cancellationToken)
		{
			var activity = await this.dbContext.Activities.SingleOrExceptionAsync(t => t.Id == request.Id);

			this.dbContext.Activities.Remove(activity);
			await this.dbContext.SaveChangesAsync(cancellationToken);

			return new Response();
		}

		public static FormLink Button(int id)
		{
			return new FormLink
			{
				Form = typeof(DeleteActivity).GetFormId(),
				Label = UiFormConstants.DeleteLabel,
				InputFieldValues = new Dictionary<string, object>
				{
					{ nameof(Request.Id), id }
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