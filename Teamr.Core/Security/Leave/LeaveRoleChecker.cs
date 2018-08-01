namespace Teamr.Core.Security.Leave
{
	using System.Collections.Generic;
	using CPermissions;
	using Teamr.Core.Domain;
	using TeamR.Core.Security;
	using TeamR.Infrastructure.User;

	public class LeaveRoleChecker : IRoleChecker<UserContext, LeaveRole, Leave>
	{
		public IEnumerable<LeaveRole> GetRoles(UserContext user, Leave context)
		{
			if (context.CreatedByUserId == user.User.UserId)
			{
				yield return LeaveRole.Owner;
			}

			if (user.HasRole(CoreRoles.Admin))
			{
				yield return LeaveRole.Viewer;
			}
		}
	}
}