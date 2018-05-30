namespace Teamr.Infrastructure.Security
{
	using CPermissions;

	public interface ISecureHandler
	{
		UserAction GetPermission();
	}
}
