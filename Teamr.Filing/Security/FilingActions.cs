namespace TeamR.Filing.Security
{
	using TeamR.Infrastructure.Security;

	public class FilingActions : ActionContainer
	{
		public static readonly SystemAction ViewFiles = new SystemAction(nameof(ViewFiles), FilingRole.AuthenticatedUser);
	}
}
