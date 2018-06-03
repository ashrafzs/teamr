namespace Teamr.Core.Commands.Activity
{
	using System;
	using System.Threading.Tasks;
	using CPermissions;
	using MediatR;
	using Teamr.Core.DataAccess;
	using Teamr.Core.Domain;
	using Teamr.Core.Pickers;
	using Teamr.Core.Security;
	using Teamr.Infrastructure;
	using Teamr.Infrastructure.Forms;
	using Teamr.Infrastructure.Security;
	using Teamr.Infrastructure.User;
	using UiMetadataFramework.Basic.Input.Typeahead;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.Core.Binding;

	[MyForm(PostOnLoad = true, Id = "add-activity", Label = "Add Activity")]
	public class AddActivity : IMyAsyncForm<AddActivity.Request, AddActivity.Response>,ISecureHandler
	{
		private readonly CoreDbContext dbContext;
		private readonly UserContext userContext;

		public AddActivity(CoreDbContext dbContext, UserContext userContext)
		{
			this.dbContext = dbContext;
			this.userContext = userContext;
		}

		public async Task<Response> Handle(Request message)
		{
			if (message.ScheduledOn.Date > DateTime.Today.Date )
			{
				throw new BusinessException("It is not allowed to record activities for future dates.");
			}

			var activityType = await this.dbContext.ActivityTypes.FindOrExceptionAsync(message.ActivityTypeId.Value);
				var activity = new Activity(this.userContext.User.UserId, activityType, message.Quantity, message.Notes, message.ScheduledOn, message.PerformedOn);
				this.dbContext.Activities.Add(activity);
				await this.dbContext.SaveChangesAsync();
			
			return new Response();
		}

		public UserAction GetPermission()
		{
			return CoreActions.AddActivity;
		}

		public static FormLink Button()
		{
			return new FormLink
			{
				Form = typeof(AddActivity).GetFormId(),
				Label = "Add activity"
			};
		}

		public class Response : FormResponse<MyFormResponseMetadata>
		{
		}

		public class Request : IRequest<Response>
		{
			[TypeaheadInputField(typeof(ActivityTypeTypeaheadRemoteSource), Required = true, Label = "Activity Type")]
			public TypeaheadValue<int> ActivityTypeId { get; set; }

			[InputField(Label = "Notes", OrderIndex = 10)]
			public string Notes { get; set; }

			[InputField(OrderIndex = 40)]
			public DateTime ScheduledOn { get; set; }

			[InputField(OrderIndex = 50)]
			public DateTime? PerformedOn { get; set; }

			[InputField(OrderIndex = 60)]
			public decimal Quantity { get; set; }
		}
	}
}