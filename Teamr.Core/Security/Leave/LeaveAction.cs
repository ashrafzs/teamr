namespace Teamr.Core.Security.Leave
{
	using Teamr.Core.Domain;
	using TeamR.Infrastructure.Security;

	public class LeaveAction : EntityAction<Leave, LeaveRole>
	{
		public static LeaveAction Edit = new LeaveAction(nameof(Edit), LeaveRole.Owner);
		public static LeaveAction Delete = new LeaveAction(nameof(Delete), LeaveRole.Owner);
		public static LeaveAction View = new LeaveAction(nameof(View), LeaveRole.Owner, LeaveRole.Viewer);

		private LeaveAction(string name, params LeaveRole[] roles) : base(name, roles)
		{
		}
	}
}