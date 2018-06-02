namespace Teamr.Core.Security.Activity
{
	using Teamr.Infrastructure.Security;

	public class ActivityRole : Role
	{
		public static ActivityRole Owner = new ActivityRole(nameof(Owner));
		public static ActivityRole Viewer = new ActivityRole(nameof(Viewer));

		private ActivityRole(string name) : base(name)
		{
		}
	}
}
