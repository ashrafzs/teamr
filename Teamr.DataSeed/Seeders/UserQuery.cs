namespace TeamR.DataSeed.Seeders
{
	using System;
	using System.Threading.Tasks;
	using TeamR.Infrastructure;
	using TeamR.Users;

	public class UserQuery : Seeder
	{
		public readonly string EntityName;

		public UserQuery(string entityName, DataSeedDiContainer container, DatabaseEntityTracker tracker) : base(container, tracker)
		{
			this.EntityName = entityName;
		}

		public void Do(Action<UserQuery> action)
		{
			this.Do(this.EntityName, action);
		}

		public async Task Do(Func<UserQuery, Task> action)
		{
			await this.Do(this.EntityName, action);
		}

		public ApplicationUser GetEntity()
		{
			var userId = (int)this.Tracker.GetEntityId<ApplicationUser>(this.EntityName);
			return this.Container.Container.GetInstance<ApplicationDbContext>().Users.SingleOrException(t => t.Id == userId);
		}
	}
}
