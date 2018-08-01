namespace Teamr.Core.Security.Leave
{
	using TeamR.Infrastructure.Security;

	public class LeaveRole : Role
	{
		public static LeaveRole Owner = new LeaveRole(nameof(Owner));
		public static LeaveRole Viewer = new LeaveRole(nameof(Viewer));

		private LeaveRole(string name) : base(name)
		{
		}
	}
}