namespace Teamr.Core.Security.SomeThing
{
	using Teamr.Core.Domain;
	using Teamr.Infrastructure.Security;
	using Teamr.Infrastructure.User;

	public class SomeThingPermissionManager : EntityPermissionManager<UserContext, SomeThingAction, SomeThingRole, SomeThing>
	{
		public SomeThingPermissionManager() : base(new SomeThingRoleChecker())
		{
		}
	}
}
