namespace TeamR.Help.Security
{
    using System.Collections.Generic;
    using TeamR.Infrastructure.Security;
    using TeamR.Infrastructure.User;

    public class HelpRoleChecker : IUserRoleChecker
	{
		public IEnumerable<SystemRole> GetDynamicRoles(UserContextData userData)
		{
			if (userData != null)
			{
				yield return HelpRoles.HelpReader;
			}
		}
	}
}
