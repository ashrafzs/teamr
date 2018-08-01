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

	[MyForm(Id = "add-activity", Label = "Add completed activity")]
	[Secure(typeof(CoreActions), nameof(CoreActions.AddActivity))]
	public class AddCompletedActivity : MyAsyncForm<AddCompletedActivity.Request, AddCompletedActivity.Response>
	{
		private readonly CoreDbContext dbContext;
		private readonly UserContext userContext;

		public AddCompletedActivity(CoreDbContext dbContext, UserContext userContext)
		{
			this.dbContext = dbContext;
			this.userContext = userContext;
		}

		public override async Task<Response> Handle(Request request, CancellationToken cancellationToken)
		{
			if (request.PerformedOn.Date > DateTime.Today.Date)
			{
				throw new BusinessException("It is not allowed to record activities for future dates.");
			}

			var activityType = await this.dbContext.ActivityTypes.FindOrExceptionAsync(request.ActivityTypeId.Value);
			var activity = new Activity(this.userContext.User.UserId, activityType, request.Quantity, request.Notes, request.PerformedOn, request.PerformedOn);
			this.dbContext.Activities.Add(activity);
			await this.dbContext.SaveChangesAsync(cancellationToken);

			return new Response();
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
			[NumberConfig(Step = 0.01)]
			public decimal Quantity { get; set; }
		}
	}
}