namespace TeamR.Filing.Security
{
	using System.Collections.Generic;
	using TeamR.Infrastructure.Security;
	using TeamR.Infrastructure.User;

	public class FilingRoleChecker : IUserRoleChecker
	{
		public IEnumerable<SystemRole> GetDynamicRoles(UserContextData userData)
		{
			if (userData != null)
			{
				yield return FilingRole.AuthenticatedUser;
			}
		}
	}
}
