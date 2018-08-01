namespace TeamR.Core.DataAccess
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Options;
	using Teamr.Core.DataAccess;
	using Teamr.Core.Domain;
	using TeamR.Core.Domain;
	using TeamR.Infrastructure.Configuration;
	using TeamR.Infrastructure.DataAccess;
	using TeamR.Infrastructure.Domain;
	using TeamR.Infrastructure.User;

	public class CoreDbContext : BaseDbContext
	{
		public CoreDbContext(DbContextOptions options, EventManager eventManager, UserSession userSession, IOptions<AppConfig> appConfig)
			: base(options, eventManager, appConfig, userSession)
		{
		}

		public virtual DbSet<Activity> Activities { get; set; }
		public virtual DbSet<ActivityType> ActivityTypes { get; set; }
		public virtual DbSet<Leave> Leaves { get; set; }
		public virtual DbSet<LeaveType> LeaveTypes { get; set; }
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
