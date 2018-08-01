namespace Teamr.Core.Security.Leave
{
	using Teamr.Core.Domain;
	using TeamR.Infrastructure.Security;
	using TeamR.Infrastructure.User;

	public class LeavePermissionManager : EntityPermissionManager<UserContext, LeaveAction, LeaveRole, Leave>
	{
		public LeavePermissionManager() : base(new LeaveRoleChecker())
		{
		}
	}
}