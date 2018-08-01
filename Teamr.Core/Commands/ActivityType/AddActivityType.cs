namespace Teamr.Core.Commands.ActivityType
{
	using System.Collections.Generic;
	using MediatR;
	using Teamr.Core.Domain;
	using TeamR.Core.DataAccess;
	using TeamR.Core.Security;
	using TeamR.Infrastructure.Forms;
	using TeamR.Infrastructure.Forms.CustomProperties;
	using TeamR.Infrastructure.Security;
	using TeamR.Infrastructure.User;
	using UiMetadataFramework.Basic.Input;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "add-activity-type", Label = "Add activity type", SubmitButtonLabel = "Save changes")]
	[Secure(typeof(CoreActions), nameof(CoreActions.ManageActivityTypes))]
	public class AddActivityType : MyForm<AddActivityType.Request, AddActivityType.Response>
	{
		private readonly CoreDbContext context;
		private readonly UserContext userContext;

		public AddActivityType(CoreDbContext context, UserContext userContext)
		{
			this.context = context;
			this.userContext = userContext;
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

		protected override Response Handle(Request message)
		{
			var activityType = new ActivityType(
				message.Name,
				this.userContext.User.UserId,
				message.Unit,
				message.Points.Value,
				message.Remarks?.Value,
				message.Tag);

			this.context.ActivityTypes.Add(activityType);
			this.context.SaveChanges();

			return new Response
			{
				Id = activityType.Id
			};
		}

		public class Request : IRequest<Response>
		{
			[InputField(Required = true, OrderIndex = 2)]
			public string Name { get; set; }

			[InputField(Hidden = false, Required = true, OrderIndex = 5)]
			[NumberConfig(Step = 0.01)]
			public decimal? Points { get; set; }

			[InputField(Required = false, OrderIndex = 50)]
			public TextareaValue Remarks { get; set; }

			[InputField(Required = true, OrderIndex = 3)]
			public string Tag { get; set; }

			[InputField(Required = true, OrderIndex = 3)]
			public string Unit { get; set; }
		}

		public class Response : MyFormResponse
		{
			public int Id { get; set; }
		}
	}
}