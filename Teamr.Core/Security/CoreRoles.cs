namespace Teamr.Core.Security
{
	using Teamr.Infrastructure.Security;

	public class CoreRoles : RoleContainer
	{
		public static readonly SystemRole SysAdmin = new SystemRole(nameof(SysAdmin));
		public static readonly SystemRole Member = new SystemRole(nameof(Member));
		public static readonly SystemRole Test = new SystemRole(nameof(Test));
		public static readonly SystemRole ToDo = new SystemRole(nameof(ToDo));
	}
}