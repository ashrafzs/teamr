namespace TeamR.Users.Commands
{
	using System.Threading;
	using System.Threading.Tasks;
	using MediatR;
	using Microsoft.AspNetCore.Identity;
	using TeamR.Infrastructure.Forms;
	using TeamR.Users;
	using TeamR.Users.Security;
	using UiMetadataFramework.Basic.Response;
	using UiMetadataFramework.Core.Binding;
	using UiMetadataFramework.MediatR;
	using TeamR.Infrastructure.Security;

	[MyForm(Id = "logout", PostOnLoad = true, Label = "Logout", Menu = UserMenus.Account, MenuOrderIndex = 10)]
	[Secure(typeof(UserActions), nameof(UserActions.Logout))]
	public class Logout : AsyncForm<Logout.Request, ReloadResponse>
	{
		private readonly SignInManager<ApplicationUser> signInManager;

		public Logout(SignInManager<ApplicationUser> signInManager)
		{
			this.signInManager = signInManager;
		}

		public override async Task<ReloadResponse> Handle(Request message, CancellationToken cancellationToken)
		{
			// TODO: implement removal of cookies in web project.
			this.signInManager.Context.Response.Cookies.Delete("app-data");
			await this.signInManager.SignOutAsync();

			return new ReloadResponse
			{
				Form = typeof(Login).GetFormId()
			};
		}

		public class Request : IRequest<ReloadResponse>
		{
		}
	}
}
