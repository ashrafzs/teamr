namespace TeamR.Users.Commands
{
	using System.Threading;
	using System.Threading.Tasks;
	using MediatR;
	using Microsoft.AspNetCore.Identity;
	using UiMetadataFramework.Basic.Input;
	using UiMetadataFramework.Basic.Response;
	using UiMetadataFramework.Core.Binding;
	using UiMetadataFramework.MediatR;
	using TeamR.Infrastructure;
	using TeamR.Infrastructure.Forms;
	using TeamR.Users.Forms;

	[MyForm(Id = "reset-password", Label = "Set new password", SubmitButtonLabel = "Confirm new password")]
	public class ResetPassword : AsyncForm<ResetPassword.Request, ReloadResponse>
	{
		private readonly SignInManager<ApplicationUser> signInManager;
		private readonly UserManager<ApplicationUser> userManager;

		public ResetPassword(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
		}

		public override async Task<ReloadResponse> Handle(Request message, CancellationToken cancellationToken)
		{
			var user = this.userManager.Users.SingleOrException(t => t.Id == message.Id);

			var result = await this.userManager.ResetPasswordAsync(user, message.Token, message.Password.Value);
			result.EnforceSuccess("Failed to reset password.");

			await this.signInManager.SignInAsync(user, true);

			return new ReloadResponse
			{
				// TODO: show success message before redirecting.
				Form = typeof(MyAccount).GetFormId()
			};
		}

		public class Request : IRequest<ReloadResponse>
		{
			/// <summary>
			/// Gets or sets id of the user, whose password to reset.
			/// </summary>
			[InputField(Required = true, Hidden = true)]
			public int Id { get; set; }

			/// <summary>
			/// Gets or sets new password for the account.
			/// </summary>
			[InputField(Required = true)]
			[TeamRPasswordInputConfig(true)]
			public Password Password { get; set; }

			/// <summary>
			/// Gets or sets password reset token.
			/// </summary>
			[InputField(Required = true, Hidden = true)]
			public string Token { get; set; }
		}
	}
}
