namespace TeamR.DataSeed.Seeders
{
	using Teamr.Core.Domain;

	public class LeaveQuery : Query<Leave>
	{
		public LeaveQuery(string entityName, DataSeedDiContainer container, DatabaseEntityTracker tracker) : base(entityName, container, tracker)
		{
		}
	}
}