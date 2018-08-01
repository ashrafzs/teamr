namespace Teamr.Core.Commands.Activity
{
	using System;
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using Microsoft.EntityFrameworkCore;
	using Teamr.Core.Pickers;
	using Teamr.Core.Security.Activity;
	using TeamR.Core.DataAccess;
	using TeamR.Infrastructure;
	using TeamR.Infrastructure.Forms;
	using TeamR.Infrastructure.Forms.CustomProperties;
	using TeamR.Infrastructure.Forms.Record;
	using TeamR.Infrastructure.Security;
	using UiMetadataFramework.Basic.EventHandlers;
	using UiMetadataFramework.Basic.Input.Typeahead;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "edit-activity", PostOnLoad = true,PostOnLoadValidation = false ,Label = "Edit activity", SubmitButtonLabel = UiFormConstants.EditSubmitLabel)]
	[Secure(typeof(ActivityAction), nameof(ActivityAction.Edit))]
	public class EditActivity : MyAsyncForm<EditActivity.Request, EditActivity.Response>
	{
		private readonly CoreDbContext dbContext;

		public EditActivity(CoreDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public override async Task<Response> Handle(Request request, CancellationToken cancellationToken)
		{
			var activity = this.dbContext.Activities
				.Include(s => s.ActivityType)
				.Include(s => s.CreatedByUser)
				.SingleOrException(t => t.Id == request.Id);

			if (request.Operation?.Value == RecordRequestOperation.Post)
			{
				if (request.PerformedOn != null)
				{
					activity.EditPerformedDate(request.PerformedOn.Value);
				}

				if (request.Quantity != null)
				{
					activity.EditQuantity(request.Quantity.Value);
					activity.EditPoints(activity.ActivityType.Points);
				}

				activity.EditActivityType(request.ActivityType.Value);
				activity.EditNotes(request.Notes);

				await this.dbContext.SaveChangesAsync(cancellationToken);
			}

			return new Response
			{
				Notes = activity.Notes,
				PerformedOn = activity.PerformedOn,
				Quantity = activity.Quantity,
				ActivityType = new TypeaheadValue<int>(activity.ActivityTypeId),
				Metadata = new MyFormResponseMetadata
				{
					Title = activity.Notes
				}
			};
		}

		public static FormLink Button(int id)
		{
			return new FormLink
			{
				Form = typeof(EditActivity).GetFormId(),
				Label = UiFormConstants.EditIconLabel,
				InputFieldValues = new Dictionary<string, object>
				{
					{ nameof(Request.Id), id }
				}
			}.WithCssClass("btn-primary btn-icon");
		}

		public class Request : RecordRequest<Response>, ISecureHandlerRequest
		{
			[BindToOutput(nameof(Response.ActivityType))]
			[TypeaheadInputField(typeof(ActivityTypeTypeaheadRemoteSource), Required = true)]
			public TypeaheadValue<int> ActivityType { get; set; }

			[InputField(Hidden = true, Required = true)]
			public int Id { get; set; }

			[BindToOutput(nameof(Response.Notes))]
			[InputField(OrderIndex = 0)]
			public string Notes { get; set; }

			[BindToOutput(nameof(Response.PerformedOn))]
			[InputField( OrderIndex = 0)]
			public DateTime? PerformedOn { get; set; }

			[BindToOutput(nameof(Response.Quantity))]
			[InputField(OrderIndex = 0, Required = true)]
			[NumberConfig(Step = 0.01)]
			public decimal? Quantity { get; set; }

			[NotField]
			public int ContextId => this.Id;
		}

		public class Response : RecordResponse
		{
			[NotField]
			public TypeaheadValue<int> ActivityType { get; set; }

			[NotField]
			public string Notes { get; set; }

			[NotField]
			public DateTime? PerformedOn { get; set; }

			[NotField]
			public decimal Quantity { get; set; }
		}
	}
}