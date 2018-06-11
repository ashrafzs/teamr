namespace Teamr.Core.Security.Leave
{
	using Teamr.Core.DataAccess;
	using Teamr.Core.Domain;
	using Teamr.Infrastructure;
	using Teamr.Infrastructure.Security;

	[EntityRepository(EntityType = typeof(Leave))]
	public class LeaveRepository : IEntityRepository
	{
		private readonly CoreDbContext context;

		public LeaveRepository(CoreDbContext context)
		{
			this.context = context;
		}

		public object Find(int entityId)
		{
			return this.context.Leaves.SingleOrException(t => t.Id == entityId);
		}
	}
}