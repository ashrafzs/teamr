namespace Teamr.Core.Commands.ActivityType
{
	using System.Collections.Generic;
	using CPermissions;
	using MediatR;
	using Teamr.Core.DataAccess;
	using Teamr.Core.Domain;
	using Teamr.Core.Security;
	using Teamr.Infrastructure.Forms;
	using Teamr.Infrastructure.Security;
	using Teamr.Infrastructure.User;
	using UiMetadataFramework.Basic.Input;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "add-activity-type", Label = "Add activity type",
		SubmitButtonLabel = "Save changes")]
	public class AddActivityType : IMyForm<AddActivityType.Request, AddActivityType.Response>, ISecureHandler
	{
		private readonly CoreDbContext context;
		private readonly UserContext userContext;

		public AddActivityType(CoreDbContext context, UserContext userContext)
		{
			this.context = context;
			this.userContext = userContext;
		}

		public Response Handle(Request message)
		{
			if (message.Points != null)
			{
				var activityType = new ActivityType(message.Name, this.userContext.User.UserId, message.Unit, message.Points.Value, message.Remarks.Value);
				this.context.ActivityTypes.Add(activityType);
				this.context.SaveChanges();
			}

			return new Response();
		}

		public UserAction GetPermission()
		{
			return CoreActions.ManageActivityTypes;
		}

		public static FormLink Button()
		{
			return new FormLink
			{
				Label = "Add",
				Form = typeof(AddActivityType).GetFormId(),
				InputFieldValues = new Dictionary<string, object>()
			};
		}

		public class Request : IRequest<Response>
		{
			[InputField(Required = true, OrderIndex = 2)]
			public string Name { get; set; }

			[InputField(Hidden = false, Required = true, OrderIndex = 5)]
			public decimal? Points { get; set; }

			[InputField(Required = false, OrderIndex = 50)]
			public TextareaValue Remarks { get; set; }

			[InputField(Required = true, OrderIndex = 3)]
			public string Unit { get; set; }
		}

		public class Response : FormResponse<MyFormResponseMetadata>
		{
		}
	}
}