namespace Teamr.Core.Commands.Activity
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using CPermissions;
	using Teamr.Core.DataAccess;
	using Teamr.Core.Domain;
	using Teamr.Core.Security.Activity;
	using Teamr.Infrastructure;
	using Teamr.Infrastructure.Forms;
	using Teamr.Infrastructure.Forms.Record;
	using Teamr.Infrastructure.Security;
	using UiMetadataFramework.Basic.EventHandlers;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "perform-activity", PostOnLoad = true, PostOnLoadValidation = false, Label = "Perform-activity")]
	public class PerformActivity : IMyAsyncForm<PerformActivity.Request, PerformActivity.Response>,
		IAsyncSecureHandler<Activity, PerformActivity.Request, PerformActivity.Response>
	{
		private readonly CoreDbContext dbContext;

		public PerformActivity(CoreDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public UserAction<Activity> GetPermission()
		{
			return ActivityAction.Perform;
		}

		public async Task<Response> Handle(Request message)
		{
			var activity = await this.dbContext.Activities.SingleOrExceptionAsync(t => t.Id == message.Id);
			if (message.Operation?.Value == RecordRequestOperation.Post)
			{
				activity.EditPerformedDate(message.PerformedOn);
				await this.dbContext.SaveChangesAsync();
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