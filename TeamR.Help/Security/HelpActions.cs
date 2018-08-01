namespace TeamR.Help.Security
{
    using TeamR.Infrastructure.Security;

    public class HelpActions : ActionContainer
	{
		public static readonly SystemAction ViewHelpFiles = new SystemAction(nameof(ViewHelpFiles), HelpRoles.HelpReader);
	}
}
