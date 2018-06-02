namespace Teamr.Core.Commands
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

	[MyForm(Id = "delete-activity", PostOnLoad = true)]
	public class DeleteActivity : IMyAsyncForm<DeleteActivity.Request, DeleteActivity.Response>, ISecureHandler
	{
		private readonly CoreDbContext dbContext;

		public DeleteActivity(CoreDbContext dbContext)
		{
			this.dbContext = dbContext;
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

		public UserAction GetPermission()
		{
			return CoreActions.UseTools;
		}

		public static FormLink Button(int userId)
		{
			return new FormLink
			{
				Form = typeof(DeleteActivity).GetFormId(),
				Label = "Delete",
				InputFieldValues = new Dictionary<string, object>
				{
					{ nameof(Request.Id), userId }
				}
			}.WithAction(FormLinkActions.Run);
		}

		public class Response : FormResponse<MyFormResponseMetadata>
		{
		}

		public class Request : IRequest<Response>
		{
			[InputField(Hidden = true)]
			public int Id { get; set; }
		}
	}
}