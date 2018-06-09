namespace Teamr.Core.Commands.ActivityType
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

	[MyForm(Id = "delete-activity-type", PostOnLoad = true)]
	public class DeleteActivityType : IMyAsyncForm<DeleteActivityType.Request, DeleteActivityType.Response>, ISecureHandler
	{
		private readonly CoreDbContext context;

		public DeleteActivityType(CoreDbContext context)
		{
			this.context = context;
		}

		public async Task<Response> Handle(Request message)
		{
			var activityType = await this.context.ActivityTypes
				.SingleOrExceptionAsync(s => s.Id == message.Id);

			if (this.context.Activities.Any(a => a.ActivityTypeId == message.Id))
			{
				throw new BusinessException("You can not delete activity type, because it's have linked activities.");
			}


			this.context.ActivityTypes.Remove(activityType);
			this.context.SaveChanges();


			return new Response();
		}

		public UserAction GetPermission()
		{
			return CoreActions.ManageActivityTypes;
		}

		public static FormLink Button(int id)
		{
			return new FormLink
			{
				Label = UiFormConstants.DeleteLabel,
				Form = typeof(DeleteActivityType).GetFormId(),
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