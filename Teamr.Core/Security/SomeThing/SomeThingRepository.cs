namespace Teamr.Core.Security.SomeThing
{
	using Teamr.Core.DataAccess;
	using Teamr.Core.Domain;
	using Teamr.Infrastructure;
	using Teamr.Infrastructure.Security;

	[EntityRepository(EntityType = typeof(SomeThing))]
	public class SomeThingRepository : IEntityRepository
	{
		private readonly CoreDbContext context;

		public SomeThingRepository(CoreDbContext context)
		{
			this.context = context;
		}

		public object Find(int entityId)
		{
			return this.context.SomeThings.SingleOrException(t => t.Id == entityId);
		}
	}
}
