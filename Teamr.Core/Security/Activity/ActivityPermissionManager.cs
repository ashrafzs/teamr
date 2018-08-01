namespace Teamr.Core.Security.Activity
{
	using Teamr.Core.Domain;
	using TeamR.Infrastructure.Security;
	using TeamR.Infrastructure.User;

	public class ActivityPermissionManager : EntityPermissionManager<UserContext, ActivityAction, ActivityRole, Activity>
	{
		public ActivityPermissionManager() : base(new ActivityRoleChecker())
		{
		}
	}
}
