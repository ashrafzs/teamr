namespace Teamr.Core.DataAccess
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Design;

	public class DesignTimeCoreDbContextFactory : IDesignTimeDbContextFactory<CoreDbContext>
	{
		public CoreDbContext CreateDbContext(string[] args)
		{
			const string ConnectionString = "Server=;Database=Teamr;Trusted_Connection=True;MultipleActiveResultSets=true";
			var dbContextOptions = new DbContextOptionsBuilder().UseSqlServer(ConnectionString).Options;
			return new CoreDbContext(dbContextOptions);
		}
	}
}
