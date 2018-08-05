namespace TeamR.DataSeed.Seeders
{
	using Teamr.Core.Domain;

	public class LeaveTypeQuery : Query<LeaveType>
	{
		public LeaveTypeQuery(string entityName, DataSeedDiContainer container, DatabaseEntityTracker tracker) : base(entityName, container, tracker)
		{
		}
	}
}