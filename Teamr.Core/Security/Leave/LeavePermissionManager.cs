namespace Teamr.Core.Security.Leave
{
	using Teamr.Core.Domain;
	using Teamr.Infrastructure.Security;
	using Teamr.Infrastructure.User;

	public class LeavePermissionManager : EntityPermissionManager<UserContext, LeaveAction, LeaveRole, Leave>
	{
		public LeavePermissionManager() : base(new LeaveRoleChecker())
		{
		}
	}
}