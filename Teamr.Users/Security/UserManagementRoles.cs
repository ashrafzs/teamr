namespace TeamR.Users.Security
{
	using TeamR.Infrastructure.Security;

	public class UserManagementRoles : RoleContainer
	{
		public static readonly SystemRole AuthenticatedUser = new SystemRole(nameof(AuthenticatedUser), true);
		public static readonly SystemRole UnauthenticatedUser = new SystemRole(nameof(UnauthenticatedUser), true);
		public static readonly SystemRole UserAdmin = new SystemRole(nameof(UserAdmin));
		public static readonly SystemRole Impersonator = new SystemRole(nameof(Impersonator), true);
	}
}
