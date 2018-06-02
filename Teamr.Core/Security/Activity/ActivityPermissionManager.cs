namespace Teamr.Core.Security.Activity
{
	using Teamr.Core.Domain;
	using Teamr.Infrastructure.Security;
	using Teamr.Infrastructure.User;

	public class ActivityPermissionManager : EntityPermissionManager<UserContext, ActivityAction, ActivityRole, Activity>
	{
		public ActivityPermissionManager() : base(new ActivityRoleChecker())
		{
		}
	}
}
