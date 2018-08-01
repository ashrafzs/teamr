namespace TeamR.Filing.Security
{
	using TeamR.Infrastructure.Security;

	public class FilingRole : RoleContainer
	{
		public static readonly SystemRole AuthenticatedUser = new SystemRole(nameof(AuthenticatedUser), true);
	}
}
