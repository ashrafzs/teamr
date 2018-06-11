namespace Teamr.Core.DataAccess
{
	using Microsoft.EntityFrameworkCore;
	using Teamr.Core.Domain;
	using Unops.Spgs.Core.DataAccess.Mapping;

	public class CoreDbContext : DbContext
	{
		public CoreDbContext(DbContextOptions options) : base(options)
		{
		}

		public virtual DbSet<ActivityType> ActivityTypes { get; set; }
		public virtual DbSet<Activity> Activities { get; set; }
		public virtual DbSet<LeaveType> LeaveTypes { get; set; }
		public virtual DbSet<Leave> Leaves{ get; set; }
		public virtual DbSet<RegisteredUser> Users { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.ApplyConfiguration(new ActivityTypeMap());
			builder.ApplyConfiguration(new ActivityMap());
			builder.ApplyConfiguration(new LeaveTypeMap());
			builder.ApplyConfiguration(new LeaveMap());
			builder.ApplyConfiguration(new RegisteredUserMap());
		}
	}
}
