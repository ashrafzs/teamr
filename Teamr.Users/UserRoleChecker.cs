namespace Teamr.Users
{
	using System.Collections.Generic;
	using Teamr.Infrastructure.Security;
	using Teamr.Infrastructure.User;
	using Teamr.Users.Security;

	public class UserRoleChecker : IUserRoleChecker
	{
		public IEnumerable<SystemRole> GetDynamicRoles(UserContextData userData)
		{
			yield return userData != null
				? UserManagementRoles.AuthenticatedUser
				: UserManagementRoles.UnauthenticatedUser;
		}
	}
}
