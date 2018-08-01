namespace TeamR.Users.Forms
{
	using TeamR.Infrastructure.Forms.CustomProperties;

	public class TeamRPasswordInputConfig : PasswordInputConfigAttribute
	{
		public TeamRPasswordInputConfig(bool requireConfirmation = false)
		{
			this.RegexDescription =
				"Password must contain 8 or more characters with at " +
				"least one number, one uppercase and one lowercase letter.";

			this.Regex = "(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{8,}";

			this.RequireConfirmation = requireConfirmation;
		}
	}
}
