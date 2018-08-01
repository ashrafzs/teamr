namespace TeamR.DataSeed.Seeders
{
	using TeamR.Infrastructure.Domain;

	public abstract class Query<T> : Seeder
		where T : DomainEntityWithKeyInt32
	{
		public readonly string EntityName;

		protected Query(string entityName, DataSeedDiContainer container, DatabaseEntityTracker tracker) : base(container, tracker)
		{
			this.EntityName = entityName;
		}

		public T GetEntity()
		{
			return this.GetEntityByName<T>(this.EntityName);
		}
	}
}
