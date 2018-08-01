namespace TeamR.Core.Security
{
	using TeamR.Infrastructure.Security;

	public class CoreRoles : RoleContainer
	{
		public static readonly SystemRole Admin = new SystemRole(nameof(Admin));
		public static readonly SystemRole Member = new SystemRole(nameof(Member));
		public static readonly SystemRole AuthenticatedUser = new SystemRole(nameof(AuthenticatedUser));
	}
}
