namespace Teamr.Core.Security
{
	using Teamr.Infrastructure.Security;

	public class CoreActions : ActionContainer
	{
		public static readonly SystemAction ViewActivityTypes = new SystemAction(nameof(ViewActivityTypes), CoreRoles.SysAdmin);
		public static readonly SystemAction ViewActivities = new SystemAction(nameof(ViewActivities), CoreRoles.Member);
		public static readonly SystemAction ViewUserProfile = new SystemAction(nameof(ViewUserProfile), CoreRoles.Member);
		public static readonly SystemAction ViewUserPointsByDayReport = new SystemAction(nameof(ViewUserPointsByDayReport), CoreRoles.Member);
		public static readonly SystemAction ManageActivityTypes = new SystemAction(nameof(ManageActivityTypes), CoreRoles.SysAdmin);
		public static readonly SystemAction AddActivity = new SystemAction(nameof(AddActivity), CoreRoles.Member);
		public static readonly SystemAction ViewReport = new SystemAction(nameof(ViewReport), CoreRoles.Test);
	}
}
