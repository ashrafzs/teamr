namespace TeamR.Infrastructure
{
	using CPermissions;
	using TeamR.Infrastructure.User;

	public class PermissionException : PermissionException<UserContext>
	{
		public PermissionException(string action, UserContext userContext) : base(new UserAction(action), userContext)
		{
		}
	}
}
