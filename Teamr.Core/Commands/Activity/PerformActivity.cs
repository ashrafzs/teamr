namespace Teamr.Core.Commands.Activity
{
	using System;
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using Teamr.Core.Security.Activity;
	using TeamR.Core.DataAccess;
	using TeamR.Infrastructure;
	using TeamR.Infrastructure.Forms;
	using TeamR.Infrastructure.Forms.Record;
	using TeamR.Infrastructure.Security;
	using UiMetadataFramework.Basic.EventHandlers;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "perform-activity", PostOnLoad = true, PostOnLoadValidation = false, Label = "Perform-activity")]
	[Secure(typeof(ActivityAction), nameof(ActivityAction.Perform))]
	public class PerformActivity : MyAsyncForm<PerformActivity.Request, PerformActivity.Response>
	{
		private readonly CoreDbContext dbContext;

		public PerformActivity(CoreDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public override async Task<Response> Handle(Request request, CancellationToken cancellationToken)
		{
			var activity = await this.dbContext.Activities.SingleOrExceptionAsync(t => t.Id == request.Id);
			if (request.Operation?.Value == RecordRequestOperation.Post)
			{
				activity.EditPerformedDate(request.PerformedOn);
				await this.dbContext.SaveChangesAsync(cancellationToken);
			}

			return new Response
			{
				PerformedOn = activity.PerformedOn ?? DateTime.UtcNow
			};
		}

		public static FormLink Button(int userId)
		{
			return new FormLink
			{
				Form = typeof(PerformActivity).GetFormId(),
				Label = "Perform",
				InputFieldValues = new Dictionary<string, object>
				{
					{ nameof(Request.Id), userId }
				}
			}.WithAction(FormLinkActions.Run).WithCssClass("btn-success btn btn-sm");
		}

		public class Response : RecordResponse
		{
			[NotField]
			public DateTime PerformedOn { get; set; }
		}

		public class Request : RecordRequest<Response>, ISecureHandlerRequest
		{
			[InputField(Hidden = true)]
			public int Id { get; set; }

			[BindToOutput(nameof(Response.PerformedOn))]
			public DateTime PerformedOn { get; set; }

			[NotField]
			public int ContextId => this.Id;
		}
	}
}