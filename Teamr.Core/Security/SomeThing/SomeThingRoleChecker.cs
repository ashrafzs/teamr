namespace Teamr.Core.Security.SomeThing
{
	using System.Collections.Generic;
	using CPermissions;
	using Teamr.Core.Domain;
	using Teamr.Infrastructure.User;

	public class SomeThingRoleChecker : IRoleChecker<UserContext, SomeThingRole, SomeThing>
	{
		public IEnumerable<SomeThingRole> GetRoles(UserContext user, SomeThing context)
		{
			if (context.OwnerUserId == user.User.UserId)
			{
				yield return SomeThingRole.Owner;
			}

			if (user.HasRole(CoreRoles.SysAdmin))
			{
				yield return SomeThingRole.Viewer;
			}
		}
	}
}
