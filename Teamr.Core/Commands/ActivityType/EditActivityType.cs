namespace Teamr.Core.Commands.ActivityType
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using TeamR.Core.DataAccess;
	using TeamR.Core.Security;
	using TeamR.Infrastructure;
	using TeamR.Infrastructure.Forms;
	using TeamR.Infrastructure.Forms.CustomProperties;
	using TeamR.Infrastructure.Forms.Record;
	using TeamR.Infrastructure.Security;
	using UiMetadataFramework.Basic.EventHandlers;
	using UiMetadataFramework.Basic.Input;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "edit-activity-type", PostOnLoad = true, PostOnLoadValidation = false, Label = "Edit activity type", SubmitButtonLabel = "Save changes")]
	[Secure(typeof(CoreActions), nameof(CoreActions.ManageActivityTypes))]
	public class EditActivityType : MyAsyncForm<EditActivityType.Request, EditActivityType.Response>
	{
		private readonly CoreDbContext context;

		public EditActivityType(CoreDbContext context)
		{
			this.context = context;
		}

		public override async Task<Response> Handle(Request request, CancellationToken cancellationToken)
		{
			var activityType = await this.context.ActivityTypes
				.SingleOrExceptionAsync(s => s.Id == request.Id);

			if (request.Operation?.Value == RecordRequestOperation.Post)
			{
				if (request.Points != null)
				{
					if (request.Points != activityType.Points && request.ChangeOldActivityPoints)
					{
						foreach (var activity in this.context.Activities.Where(w => w.ActivityTypeId == request.Id))
						{
							activity.EditPoints(request.Points.Value);
						}
					}

					activityType.Edit(request.Name, request.Unit, request.Points.Value, request.Remarks?.Value,request.Tag);
					this.context.SaveChanges();
				}
			}

			return new Response
			{
				Name = activityType.Name,
				Points = activityType.Points,
				Unit = activityType.Unit,
				Remarks = activityType.Remarks,
				Tag = activityType.Tag,
				Metadata = new MyFormResponseMetadata
				{
					Title = activityType.Name
				}
			};
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

			[InputField(Required = true, OrderIndex = 3)]
			[BindToOutput(nameof(Response.Tag))]
			public string Tag { get; set; }

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

			[NotField]
			public string Tag { get; set; }
		}
	}
}