namespace Teamr.Core.Security.Activity
{
	using Teamr.Core.Domain;
	using TeamR.Infrastructure.Security;

	public class ActivityAction : EntityAction<Activity, ActivityRole>
	{
		public static ActivityAction Edit = new ActivityAction(nameof(Edit), ActivityRole.Owner);
		public static ActivityAction Delete = new ActivityAction(nameof(Delete), ActivityRole.Owner);
		public static ActivityAction View = new ActivityAction(nameof(View), ActivityRole.Owner, ActivityRole.Viewer);
		public static ActivityAction Perform = new ActivityAction(nameof(Perform), ActivityRole.Owner);

		private ActivityAction(string name, params ActivityRole[] roles) : base(name, roles)
		{
		}
	}
}