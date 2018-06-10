namespace Teamr.Users.Commands
{
	using CPermissions;
	using MediatR;
	using Microsoft.AspNetCore.Identity;
	using Teamr.Infrastructure;
	using Teamr.Infrastructure.Forms;
	using Teamr.Infrastructure.Security;
	using Teamr.Users.Security;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.Core.Binding;
	using Teamr.Infrastructure.User;

	[MyForm(Id = "account", Label = "My account", PostOnLoad = true, Menu = UserMenus.Account)]
	public class MyAccount : IMyForm<MyAccount.Request, MyAccount.Response>, ISecureHandler
	{
		private readonly UserContext userContext;
		private readonly UserManager<ApplicationUser> userManager;

		public MyAccount(UserManager<ApplicationUser> userManager, UserContext userContext)
		{
			this.userManager = userManager;
			this.userContext = userContext;
		}

		public Response Handle(Request message)
		{
			var user = this.userManager.Users.SingleOrException(t => t.UserName == this.userContext.User.UserName);

			return new Response
			{
				Username = user.UserName,
				EmailConfirmed = user.EmailConfirmed,
				Email = user.Email,
				Buttons = new ActionList(ChangePassword.Button(), ChangeEmail.Button())
			};
		}

		public UserAction GetPermission()
		{
			return UserActions.ManageMyAccount;
		}

		public static FormLink Button()
		{
			return new FormLink
			{
				Form = typeof(MyAccount).GetFormId(),
				Label = "My account"
			};
		}

		public class Response : FormResponse<MyFormResponseMetadata>
		{
			[OutputField(OrderIndex = 20)]
			public string Email { get; set; }

			[OutputField(OrderIndex = 10)]
			public string Username { get; set; }

			[OutputField(OrderIndex = 30, Label = "Email confirmed")]
			public bool EmailConfirmed { get; set; }

			[OutputField(OrderIndex = 0)]
			public ActionList Buttons { get; set; }
		}

		public class Request : IRequest<Response>
		{
		}
	}
}
