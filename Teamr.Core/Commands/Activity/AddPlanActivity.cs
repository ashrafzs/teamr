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
	using Teamr.Infrastructure.Forms.CustomProperties;
	using Teamr.Infrastructure.Security;
	using Teamr.Infrastructure.User;
	using UiMetadataFramework.Basic.Input.Typeahead;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.Core.Binding;

	[MyForm(PostOnLoad = true, Id = "add-plan-activity", Label = "Add plan activity")]
	public class AddPlanActivity : IMyAsyncForm<AddPlanActivity.Request, AddPlanActivity.Response>,ISecureHandler
	{
		private readonly CoreDbContext dbContext;
		private readonly UserContext userContext;

		public AddPlanActivity(CoreDbContext dbContext, UserContext userContext)
		{
			this.dbContext = dbContext;
			this.userContext = userContext;
		}

		public async Task<Response> Handle(Request message)
		{
			if (message.ScheduledOn.Date < DateTime.Today.Date)
			{
				throw new BusinessException("It is not allowed to record activities in the past.");
			}

			var activityType = await this.dbContext.ActivityTypes.FindOrExceptionAsync(message.ActivityTypeId.Value);
				var activity = new Activity(this.userContext.User.UserId, activityType, message.Quantity, message.Notes, message.ScheduledOn);
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
				Form = typeof(AddPlanActivity).GetFormId(),
				Label = "Add plan activity"
			};
		}

		public class Response : FormResponse<MyFormResponseMetadata>
		{
		}

		public class Request : IRequest<Response>
		{
			[TypeaheadInputField(typeof(ActivityTypeTypeaheadRemoteSource), Required = true, Label = "Activity type")]
			public TypeaheadValue<int> ActivityTypeId { get; set; }

			[InputField(Label = "Notes", OrderIndex = 10)]
			public string Notes { get; set; }

			[InputField(OrderIndex = 40, Label = "Scheduled on")]
			public DateTime ScheduledOn { get; set; }

			[InputField(OrderIndex = 60)]
			[DecimalStep(".01")]
			public decimal Quantity { get; set; }
		}
	}
}