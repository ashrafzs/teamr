namespace Teamr.Core.Security.Activity
{
	using Teamr.Core.Domain;
	using TeamR.Core.DataAccess;
	using TeamR.Infrastructure;
	using TeamR.Infrastructure.Security;

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
