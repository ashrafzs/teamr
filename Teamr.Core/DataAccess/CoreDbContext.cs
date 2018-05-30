namespace Teamr.Core.DataAccess
{
	using Microsoft.EntityFrameworkCore;
	using Teamr.Core.Domain;

	public class CoreDbContext : DbContext
	{
		public CoreDbContext(DbContextOptions options) : base(options)
		{
		}

		public virtual DbSet<SomeThing> SomeThings { get; set; }
		public virtual DbSet<ActivityType> ActivityTypes { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.ApplyConfiguration(new SomeThingMap());
			builder.ApplyConfiguration(new ActivityTypeMap());
		}
	}
}
