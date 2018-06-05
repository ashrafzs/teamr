namespace Teamr.Core.Commands.Activity
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using CPermissions;
	using Microsoft.EntityFrameworkCore;
	using Teamr.Core.DataAccess;
	using Teamr.Core.Pickers;
	using Teamr.Core.Security;
	using Teamr.Core.Security.Activity;
	using Teamr.Infrastructure;
	using Teamr.Infrastructure.Forms;
	using Teamr.Infrastructure.Forms.Record;
	using Teamr.Infrastructure.Security;
	using UiMetadataFramework.Basic.EventHandlers;
	using UiMetadataFramework.Basic.Input.Typeahead;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "edit-activity", PostOnLoad = true,PostOnLoadValidation = false ,Label = "Edit activity", SubmitButtonLabel = UiFormConstants.EditSubmitLabel)]
	public class EditActivity : IMyAsyncForm<EditActivity.Request, EditActivity.Response>, 
		IAsyncSecureHandler<Domain.Activity, EditActivity.Request, EditActivity.Response>
	{
		private readonly CoreDbContext dbContext;

		public EditActivity(CoreDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<Response> Handle(Request message)
		{
			var activity = this.dbContext.Activities
				.Include(s => s.ActivityType)
				.Include(s => s.CreatedByUser)
				.SingleOrException(t => t.Id == message.Id);

			if (message.Operation?.Value == RecordRequestOperation.Post)
			{
				if (message.PerformedOn != null)
				{
					activity.EditPerformedDate(message.PerformedOn.Value);
				}

				activity.EditNotes(message.Notes);
				activity.EditActivityType(message.ActivityType.Value);

				await this.dbContext.SaveChangesAsync();
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

		public UserAction<Domain.Activity> GetPermission()
		{
			return ActivityAction.Edit;
		}

		public static FormLink Button(int id, string label)
		{
			return new FormLink
			{
				Form = typeof(EditActivity).GetFormId(),
				Label =label,
				InputFieldValues = new Dictionary<string, object>
				{
					{ nameof(Request.Id), id }
				}
			};
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