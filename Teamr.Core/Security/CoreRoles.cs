namespace Teamr.Core.Security
{
	using Teamr.Infrastructure.Security;

	public class CoreRoles : RoleContainer
	{
		public static readonly SystemRole SysAdmin = new SystemRole(nameof(SysAdmin));
		public static readonly SystemRole Member = new SystemRole(nameof(Member));
	}
}