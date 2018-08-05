namespace TeamR.DataSeed.Seeders
{
	using Teamr.Core.Domain;

	public class ActivityTypeQuery : Query<ActivityType>
	{
		public ActivityTypeQuery(string entityName, DataSeedDiContainer container, DatabaseEntityTracker tracker) : base(entityName, container, tracker)
		{
		}
	}
}