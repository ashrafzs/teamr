namespace TeamR.DataSeed.Seeds
{
	using System;
	using System.Threading.Tasks;
	using Bogus;
	using Microsoft.EntityFrameworkCore;
	using TeamR.Core;
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
			var faker = new Faker();
			this.admin = await this.Seeder.EnsureUser("admin", CoreRoles.Admin);

			await this.admin.Do(async t =>
			{
				for (int i = 0; i < 10; i++)
				{
					await t.AddActivityType($"activity-type-{i}");
				}

				var al = await t.AddLeaveType("AL", 1);
				var sl = await t.AddLeaveType("SL", 1);
				var usl = await t.AddLeaveType("USL", 1);
				var ul = await t.AddLeaveType("UL", 1);

				for (int i = 0; i < 10; i++)
				{
					var user = await t.EnsureUser($"user-{i}", CoreRoles.Member);

					var userIndex = i;

					await user.Do(async a =>
					{
						for (int j = 0; j < 5; j++)
						{
							var leaveType = faker.PickRandom(al, sl, usl, ul).GetEntity();
							var leaveDate = DateTime.Today.StartOfMonth().AddDays(j + userIndex);

							await user.RequestLeave($"leave-{userIndex}:{j}", leaveType, leaveDate);
						}

						for (int j = 0; j < 5; j++)
						{
							var activityType = this.Seeder.ActivityType($"activity-type-{faker.Random.Number(0, 9)}").GetEntity();

							var completedActivityDate = DateTime.Today.AddDays(-faker.Random.Number(0, 30));
							await user.AddCompletedActivity($"completed-activity-{userIndex}:{j}", activityType, completedActivityDate, 1);

							var plannedActivityDate = DateTime.Today.AddDays(faker.Random.Number(0, 30));
							await user.AddPlannedActivity($"planned-activity-{userIndex}:{j}", activityType, plannedActivityDate, 1);
						}
					});
				}
			});
		}
	}
}