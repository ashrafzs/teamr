namespace TeamR.DataSeed.Seeds
{
	using System.Threading.Tasks;
	using Microsoft.EntityFrameworkCore;
	using TeamR.Core.Security;
	using TeamR.DataSeed.Seeders;
	using TeamR.Users.Security;

	public class Demo : Seed
	{
		private UserQuery admin;

		public Demo(DbContextOptions dbContextOptions) : base(dbContextOptions)
		{
		}

		public override async Task Run()
		{
			var dataSeed = this.Container.Container.GetInstance<DataSeed>();
			dataSeed.SeedRequiredData().Wait();

			await SeedUsers(dataSeed);

			await this.InitialiseData();
		}

		private static async Task SeedUsers(DataSeed dataSeed)
		{
			await dataSeed.EnsureUser("admin@example.com", "Password1", UserManagementRoles.UserAdmin, CoreRoles.Admin);
		}

		private async Task InitialiseData()
		{
			this.admin = await this.Seeder.EnsureUser("admin", CoreRoles.Admin);

			await this.admin.Do(async t =>
			{
				for (int i = 0; i < 10; i++)
				{
					await t.AddActivityType($"t{i}");
				}
			});
		}
	}
}
