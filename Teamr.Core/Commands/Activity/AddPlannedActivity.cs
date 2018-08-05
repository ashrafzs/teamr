namespace Teamr.Core.Commands.Activity
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using MediatR;
	using Teamr.Core.Domain;
	using Teamr.Core.Pickers;
	using TeamR.Core.DataAccess;
	using TeamR.Core.Security;
	using TeamR.Infrastructure;
	using TeamR.Infrastructure.Forms;
	using TeamR.Infrastructure.Forms.CustomProperties;
	using TeamR.Infrastructure.Security;
	using TeamR.Infrastructure.User;
	using UiMetadataFramework.Basic.Input.Typeahead;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.Core.Binding;

	[MyForm(PostOnLoad = true, Id = "add-planned-activity", Label = "Add planned activity")]
	[Secure(typeof(CoreActions), nameof(CoreActions.AddActivity))]
	public class AddPlannedActivity : MyAsyncForm<AddPlannedActivity.Request, AddPlannedActivity.Response>
	{
		private readonly CoreDbContext dbContext;
		private readonly UserContext userContext;

		public AddPlannedActivity(CoreDbContext dbContext, UserContext userContext)
		{
			this.dbContext = dbContext;
			this.userContext = userContext;
		}

		public override async Task<Response> Handle(Request request, CancellationToken cancellationToken)
		{
			if (request.ScheduledOn.Date < DateTime.Today.Date)
			{
				throw new BusinessException("It is not allowed to record activities in the past.");
			}

			var activityType = await this.dbContext.ActivityTypes.FindOrExceptionAsync(request.ActivityTypeId.Value);

			var activity = new Activity(
				this.userContext.User.UserId, 
				activityType, 
				request.Quantity, 
				request.Notes, 
				request.ScheduledOn);

			this.dbContext.Activities.Add(activity);
		
			await this.dbContext.SaveChangesAsync(cancellationToken);
			
			return new Response
			{
				Id = activity.Id
			};
		}
		
		public static FormLink Button()
		{
			return new FormLink
			{
				Form = typeof(AddPlannedActivity).GetFormId(),
				Label = "Add plan activity"
			};
		}

		public class Response : FormResponse<MyFormResponseMetadata>
		{
			[NotField]
			public int Id { get; set; }
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
			[NumberConfig(Step = 0.01)]
			public decimal Quantity { get; set; }
		}
	}
}