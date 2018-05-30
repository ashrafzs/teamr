namespace Teamr.Infrastructure
{
	using CPermissions;
	using Teamr.Infrastructure.User;

	public class PermissionException : PermissionException<UserContext>
	{
		public PermissionException(string action, UserContext userContext) : base(new UserAction(action), userContext)
		{
		}
	}
}
