namespace Teamr.Core.Commands.Activity
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using CPermissions;
	using MediatR;
	using Teamr.Core.DataAccess;
	using Teamr.Core.Domain;
	using Teamr.Core.Security.Activity;
	using Teamr.Infrastructure;
	using Teamr.Infrastructure.Forms;
	using Teamr.Infrastructure.Security;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "delete-activity", PostOnLoad = true)]
	public class DeleteActivity : IMyAsyncForm<DeleteActivity.Request, DeleteActivity.Response>,
		IAsyncSecureHandler<Activity, DeleteActivity.Request, DeleteActivity.Response>
	{
		private readonly CoreDbContext dbContext;

		public DeleteActivity(CoreDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public UserAction<Activity> GetPermission()
		{
			return ActivityAction.Delete;
		}

		public async Task<Response> Handle(Request message)
		{
			var activity = this.dbContext.Activities.SingleOrDefault(t => t.Id == message.Id);

			if (activity == null)
			{
				return new Response();
			}

			this.dbContext.Activities.Remove(activity);
			await this.dbContext.SaveChangesAsync();
			return new Response();
		}

		public static FormLink Button(int userId, string label)
		{
			return new FormLink
			{
				Form = typeof(DeleteActivity).GetFormId(),
				Label = label,
				InputFieldValues = new Dictionary<string, object>
				{
					{ nameof(Request.Id), userId }
				}
			}.WithAction(FormLinkActions.Run);
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