namespace Teamr.Core.Security.Activity
{
	using System.Collections.Generic;
	using CPermissions;
	using Teamr.Core.Domain;
	using TeamR.Core.Security;
	using TeamR.Infrastructure.User;

	public class ActivityRoleChecker : IRoleChecker<UserContext, ActivityRole, Activity>
	{
		public IEnumerable<ActivityRole> GetRoles(UserContext user, Activity context)
		{
			if (context.CreatedByUserId == user.User.UserId)
			{
				yield return ActivityRole.Owner;
			}

			if (user.HasRole(CoreRoles.Admin))
			{
				yield return ActivityRole.Viewer;
			}
		}
	}
}
