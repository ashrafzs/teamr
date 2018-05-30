namespace Teamr.Core.Security
{
	using Teamr.Infrastructure.Security;

	public class CoreRoles : RoleContainer
	{
		public static readonly SystemRole ToolUser = new SystemRole(nameof(ToolUser));
	}
}
