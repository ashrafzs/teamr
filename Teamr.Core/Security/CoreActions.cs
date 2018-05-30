namespace Teamr.Core.Security
{
	using Teamr.Infrastructure.Security;

	public class CoreActions : ActionContainer
	{
		public static readonly SystemAction UseTools = new SystemAction(nameof(UseTools), CoreRoles.SysAdmin);
		public static readonly SystemAction ViewFiles = new SystemAction(nameof(ViewFiles), CoreRoles.SysAdmin);
	}
}
