namespace Teamr.Core.Commands.LeaveType
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using CPermissions;
	using Teamr.Core.DataAccess;
	using Teamr.Core.Security;
	using Teamr.Infrastructure;
	using Teamr.Infrastructure.Forms;
	using Teamr.Infrastructure.Forms.Record;
	using Teamr.Infrastructure.Security;
	using UiMetadataFramework.Basic.EventHandlers;
	using UiMetadataFramework.Basic.Input;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "edit-leave-type", PostOnLoad = true, PostOnLoadValidation = false, Label = "Edit leave type",
		SubmitButtonLabel = "Save changes")]
	public class EditLeaveType : IMyAsyncForm<EditLeaveType.Request, EditLeaveType.Response>, ISecureHandler
	{
		private readonly CoreDbContext context;

		public EditLeaveType(CoreDbContext context)
		{
			this.context = context;
		}

		public async Task<Response> Handle(Request message)
		{
			var leaveType = await this.context.LeaveTypes
				.SingleOrExceptionAsync(s => s.Id == message.Id);

			if (message.Operation?.Value == RecordRequestOperation.Post)
			{
				if (message.Quantity != null)
				{
					leaveType.Edit(message.Name, message.Quantity.Value, message.Remarks?.Value, message.Tag);
					this.context.SaveChanges();
				}
			}

			return new Response
			{
				Name = leaveType.Name,
				Tag = leaveType.Tag,
				Points = leaveType.Quantity,
				Remarks = leaveType.Remarks,
				Metadata = new MyFormResponseMetadata
				{
					Title = leaveType.Name
				}
			};
		}

		public UserAction GetPermission()
		{
			return CoreActions.ManageLeaveTypes;
		}

		public static FormLink Button(int id)
		{
			return new FormLink
			{
				Label = UiFormConstants.EditLabel,
				Form = typeof(EditLeaveType).GetFormId(),
				InputFieldValues = new Dictionary<string, object>
				{
					{ nameof(Request.Id), id }
				}
			}.WithCssClass("btn-primary btn-icon");
		}

		public class Request : RecordRequest<Response>
		{
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
			public decimal? Quantity { get; set; }

			[InputField(Required = false, OrderIndex = 50)]
			[BindToOutput(nameof(Response.Remarks))]
			public TextareaValue Remarks { get; set; }
		}

		public class Response : RecordResponse
		{
			[NotField]
			public string Name { get; set; }

			[NotField]
			public string Tag { get; set; }

			[NotField]
			public decimal Points { get; set; }

			[NotField]
			public string Remarks { get; set; }
		}
	}
}