namespace Teamr.Core.Security.Activity
{
	using Teamr.Core.DataAccess;
	using Teamr.Core.Domain;
	using Teamr.Infrastructure;
	using Teamr.Infrastructure.Security;

	[EntityRepository(EntityType = typeof(Activity))]
	public class ActivityRepository : IEntityRepository
	{
		private readonly CoreDbContext context;

		public ActivityRepository(CoreDbContext context)
		{
			this.context = context;
		}

		public object Find(int entityId)
		{
			return this.context.Activities.SingleOrException(t => t.Id == entityId);
		}
	}
}
