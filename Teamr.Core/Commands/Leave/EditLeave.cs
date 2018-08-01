namespace Teamr.Core.Commands.Leave
{
	using System;
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using CPermissions;
	using Microsoft.EntityFrameworkCore;
	using Teamr.Core.Pickers;
	using Teamr.Core.Security.Leave;
	using TeamR.Core.DataAccess;
	using TeamR.Infrastructure;
	using TeamR.Infrastructure.Forms;
	using TeamR.Infrastructure.Forms.Record;
	using TeamR.Infrastructure.Security;
	using UiMetadataFramework.Basic.EventHandlers;
	using UiMetadataFramework.Basic.Input.Typeahead;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "edit-leave", PostOnLoad = true, PostOnLoadValidation = false, Label = "Edit leave", SubmitButtonLabel = UiFormConstants.EditSubmitLabel)]
	[Secure(typeof(LeaveAction), nameof(LeaveAction.Edit))]
	public class EditLeave : MyAsyncForm<EditLeave.Request, EditLeave.Response>
	{
		private readonly CoreDbContext dbContext;

		public EditLeave(CoreDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public UserAction<Domain.Leave> GetPermission()
		{
			return LeaveAction.Edit;
		}

		public override async Task<Response> Handle(Request request, CancellationToken cancellationToken)
		{
			var leave = this.dbContext.Leaves
				.Include(s => s.LeaveType)
				.Include(s => s.CreatedByUser)
				.SingleOrException(t => t.Id == request.Id);

			if (request.Operation?.Value == RecordRequestOperation.Post)
			{
				if (request.ScheduledOn != null)
				{
					leave.Edit(request.Notes, request.LeaveType.Value, request.ScheduledOn.Value);
				}

				await this.dbContext.SaveChangesAsync();
			}

			return new Response
			{
				Notes = leave.Notes,
				ScheduledOn = leave.ScheduledOn,
				LeaveType = new TypeaheadValue<int>(leave.LeaveTypeId),
				Metadata = new MyFormResponseMetadata
				{
					Title = leave.Notes
				}
			};
		}

		public static FormLink Button(int id)
		{
			return new FormLink
			{
				Form = typeof(EditLeave).GetFormId(),
				Label = UiFormConstants.EditIconLabel,
				InputFieldValues = new Dictionary<string, object>
				{
					{ nameof(Request.Id), id }
				}
			}.WithCssClass("btn-primary btn-icon");
		}

		public class Request : RecordRequest<Response>, ISecureHandlerRequest
		{
			[InputField(Hidden = true, Required = true)]
			public int Id { get; set; }

			[BindToOutput(nameof(Response.LeaveType))]
			[TypeaheadInputField(typeof(LeaveTypeTypeaheadRemoteSource), Required = true)]
			public TypeaheadValue<int> LeaveType { get; set; }

			[BindToOutput(nameof(Response.Notes))]
			[InputField(OrderIndex = 0)]
			public string Notes { get; set; }

			[BindToOutput(nameof(Response.ScheduledOn))]
			[InputField(OrderIndex = 0)]
			public DateTime? ScheduledOn { get; set; }

			[NotField]
			public int ContextId => this.Id;
		}

		public class Response : RecordResponse
		{
			[NotField]
			public TypeaheadValue<int> LeaveType { get; set; }

			[NotField]
			public string Notes { get; set; }

			[NotField]
			public DateTime? ScheduledOn { get; set; }
		}
	}
}