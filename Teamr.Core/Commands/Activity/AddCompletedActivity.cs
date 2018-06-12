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

	[MyForm(PostOnLoad = true, Id = "add-activity", Label = "Add completed activity")]
	public class AddCompletedActivity : IMyAsyncForm<AddCompletedActivity.Request, AddCompletedActivity.Response>, ISecureHandler
	{
		private readonly CoreDbContext dbContext;
		private readonly UserContext userContext;

		public AddCompletedActivity(CoreDbContext dbContext, UserContext userContext)
		{
			this.dbContext = dbContext;
			this.userContext = userContext;
		}

		public async Task<Response> Handle(Request message)
		{
			if (message.PerformedOn.Date > DateTime.Today.Date)
			{
				throw new BusinessException("It is not allowed to record activities for future dates.");
			}

			var activityType = await this.dbContext.ActivityTypes.FindOrExceptionAsync(message.ActivityTypeId.Value);
			var activity = new Activity(this.userContext.User.UserId, activityType, message.Quantity, message.Notes, message.PerformedOn, message.PerformedOn);
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
				Form = typeof(AddCompletedActivity).GetFormId(),
				Label = "Add completed activity"
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

			[InputField(OrderIndex = 50, Required = true)]
			public DateTime PerformedOn { get; set; }

			[InputField(OrderIndex = 60)]
			[DecimalStep(".01")]
			public decimal Quantity { get; set; }
		}
	}
}