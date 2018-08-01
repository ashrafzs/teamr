namespace TeamR.Core.Security
{
	using TeamR.Infrastructure.Security;

	public class CoreActions : ActionContainer
	{
		public static readonly SystemAction ViewNotifications = new SystemAction(nameof(ViewNotifications), CoreRoles.Member);
		public static readonly SystemAction ViewActivityTypes = new SystemAction(nameof(ViewActivityTypes), CoreRoles.Admin);
		public static readonly SystemAction ViewLeaveTypes = new SystemAction(nameof(ViewLeaveTypes), CoreRoles.Admin);
		public static readonly SystemAction ViewActivities = new SystemAction(nameof(ViewActivities), CoreRoles.Member);
		public static readonly SystemAction ViewUserProfile = new SystemAction(nameof(ViewUserProfile), CoreRoles.Member);
		public static readonly SystemAction ViewUserPointsByDayReport = new SystemAction(nameof(ViewUserPointsByDayReport), CoreRoles.Member);
		public static readonly SystemAction ManageActivityTypes = new SystemAction(nameof(ManageActivityTypes), CoreRoles.Admin);
		public static readonly SystemAction ManageLeaveTypes = new SystemAction(nameof(ManageLeaveTypes), CoreRoles.Admin);
		public static readonly SystemAction AddActivity = new SystemAction(nameof(AddActivity), CoreRoles.Member);
		public static readonly SystemAction ViewReport = new SystemAction(nameof(ViewReport), CoreRoles.Member);  
		public static readonly SystemAction AddLeave = new SystemAction(nameof(AddLeave), CoreRoles.Member);
	}
}
