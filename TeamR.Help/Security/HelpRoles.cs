namespace TeamR.Help.Security
{
	using TeamR.Infrastructure.Security;

	public class HelpRoles : RoleContainer
	{
		public static readonly SystemRole HelpReader = new SystemRole(nameof(HelpReader), true);
	}
}
