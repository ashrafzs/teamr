namespace Teamr.Core.Security
{
	using Teamr.Infrastructure.Security;

	public class CoreActions : ActionContainer
	{
		public static readonly SystemAction UseTools = new SystemAction(nameof(UseTools), CoreRoles.SysAdmin);
		public static readonly SystemAction ViewFiles = new SystemAction(nameof(ViewFiles), CoreRoles.SysAdmin);
		public static readonly SystemAction ViewActivityTypes = new SystemAction(nameof(ViewActivityTypes), CoreRoles.SysAdmin);
		public static readonly SystemAction ManageActivityTypes = new SystemAction(nameof(ManageActivityTypes), CoreRoles.SysAdmin);
		public static readonly SystemAction AddActivity = new SystemAction(nameof(ViewFiles), CoreRoles.SysAdmin);
	}
}
