namespace TeamR.Users
{
	using System.Collections.Generic;
	using TeamR.Infrastructure.Security;
	using TeamR.Infrastructure.User;
	using TeamR.Users.Security;

	public class UserRoleChecker : IUserRoleChecker
	{
		private readonly UserSession userSession;

		public UserRoleChecker(UserSession userSession)
		{
			this.userSession = userSession;
		}

		public IEnumerable<SystemRole> GetDynamicRoles(UserContextData userData)
		{
			yield return userData != null
				? UserManagementRoles.AuthenticatedUser
				: UserManagementRoles.UnauthenticatedUser;

			if (this.userSession?.ImpersonatorUserId != null &&
				this.userSession.ImpersonatorUserId != this.userSession.CurrentUserId)
			{
				yield return UserManagementRoles.Impersonator;
			}
		}
	}
}
