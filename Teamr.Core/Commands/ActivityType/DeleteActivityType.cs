namespace Teamr.Core.Commands.ActivityType
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

	[MyForm(Id = "delete-activity-type", PostOnLoad = true)]
	[Secure(typeof(CoreActions), nameof(CoreActions.ManageActivityTypes))]
	public class DeleteActivityType : MyAsyncForm<DeleteActivityType.Request, DeleteActivityType.Response>
	{
		private readonly CoreDbContext context;

		public DeleteActivityType(CoreDbContext context)
		{
			this.context = context;
		}

		public override async Task<Response> Handle(Request request, CancellationToken cancellationToken)
		{
			var activityType = await this.context.ActivityTypes
				.SingleOrExceptionAsync(s => s.Id == request.Id);

			if (this.context.Activities.Any(a => a.ActivityTypeId == request.Id))
			{
				throw new BusinessException("You can not delete activity type, because it's have linked activities.");
			}


			this.context.ActivityTypes.Remove(activityType);
			this.context.SaveChanges();


			return new Response();
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