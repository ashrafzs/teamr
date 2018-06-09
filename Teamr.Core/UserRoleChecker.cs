namespace Teamr.Core
{
	using System.Collections.Generic;
	using Teamr.Core.Security;
	using Teamr.Infrastructure.Security;
	using Teamr.Infrastructure.User;

	public class UserRoleChecker : IUserRoleChecker
	{
		/// <inheritdoc />
		public IEnumerable<SystemRole> GetDynamicRoles(UserContextData userData)
		{
			if (userData != null)
			{
				yield return CoreRoles.ToDo;
			}
		}
	}
}
