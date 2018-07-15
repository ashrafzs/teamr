namespace Teamr.Core.Commands.ActivityType
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using CPermissions;
	using Teamr.Core.DataAccess;
	using Teamr.Core.Security;
	using Teamr.Infrastructure;
	using Teamr.Infrastructure.Forms;
	using Teamr.Infrastructure.Forms.CustomProperties;
	using Teamr.Infrastructure.Forms.Record;
	using Teamr.Infrastructure.Security;
	using UiMetadataFramework.Basic.EventHandlers;
	using UiMetadataFramework.Basic.Input;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "edit-activity-type", PostOnLoad = true, PostOnLoadValidation = false, Label = "Edit activity type",
		SubmitButtonLabel = "Save changes")]
	public class EditActivityType : IMyAsyncForm<EditActivityType.Request, EditActivityType.Response>, ISecureHandler
	{
		private readonly CoreDbContext context;

		public EditActivityType(CoreDbContext context)
		{
			this.context = context;
		}

		public async Task<Response> Handle(Request message)
		{
			var activityType = await this.context.ActivityTypes
				.SingleOrExceptionAsync(s => s.Id == message.Id);

			if (message.Operation?.Value == RecordRequestOperation.Post)
			{
				if (message.Points != null)
				{
					if (message.Points != activityType.Points && message.ChangeOldActivityPoints)
					{
						foreach (var activity in this.context.Activities.Where(w => w.ActivityTypeId == message.Id))
						{
							activity.EditPoints(message.Points.Value);
						}
					}

					activityType.Edit(message.Name, message.Unit, message.Points.Value, message.Remarks?.Value);
					this.context.SaveChanges();
				}
			}

			return new Response
			{
				Name = activityType.Name,
				Points = activityType.Points,
				Unit = activityType.Unit,
				Remarks = activityType.Remarks,
				Metadata = new MyFormResponseMetadata
				{
					Title = activityType.Name
				}
			};
		}

		public UserAction GetPermission()
		{
			return CoreActions.ManageActivityTypes;
		}

		public static FormLink Button(int id)
		{
			return new FormLink
			{
				Label = UiFormConstants.EditLabel,
				Form = typeof(EditActivityType).GetFormId(),
				InputFieldValues = new Dictionary<string, object>
				{
					{ nameof(Request.Id), id }
				}
			}.WithCssClass("btn-primary btn-icon");
		}

		public class Request : RecordRequest<Response>
		{
			[InputField(Hidden = false, Required = true, OrderIndex = 5, Label = "Change old activity points")]
			public bool ChangeOldActivityPoints { get; set; }

			[InputField(Hidden = true)]
			public int Id { get; set; }

			[InputField(Required = true, OrderIndex = 2)]
			[BindToOutput(nameof(Response.Name))]
			public string Name { get; set; }

			[InputField(Hidden = false, Required = true, OrderIndex = 5)]
			[BindToOutput(nameof(Response.Points))]
			[NumberConfig(Step = 0.01)]
			public decimal? Points { get; set; }

			[InputField(Required = false, OrderIndex = 50)]
			[BindToOutput(nameof(Response.Remarks))]
			public TextareaValue Remarks { get; set; }

			[InputField(Required = true, OrderIndex = 3)]
			[BindToOutput(nameof(Response.Unit))]
			public string Unit { get; set; }
		}

		public class Response : RecordResponse
		{
			[NotField]
			public string Name { get; set; }

			[NotField]
			public decimal Points { get; set; }

			[NotField]
			public string Remarks { get; set; }

			[NotField]
			public string Unit { get; set; }
		}
	}
}