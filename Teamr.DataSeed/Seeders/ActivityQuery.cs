namespace TeamR.DataSeed.Seeders
{
	using Teamr.Core.Domain;

	public class ActivityQuery : Query<Activity>
	{
		public ActivityQuery(string entityName, DataSeedDiContainer container, DatabaseEntityTracker tracker) : base(entityName, container, tracker)
		{
		}
	}
}