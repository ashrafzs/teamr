namespace TeamR.Core
{
	using System.Collections.Generic;
	using TeamR.Core.Security;
	using TeamR.Infrastructure.Security;
	using TeamR.Infrastructure.User;

	public class UserRoleChecker : IUserRoleChecker
	{
		public IEnumerable<SystemRole> GetDynamicRoles(UserContextData userData)
		{
			if (userData != null)
			{
				yield return CoreRoles.AuthenticatedUser;
			}
		}
	}
}
