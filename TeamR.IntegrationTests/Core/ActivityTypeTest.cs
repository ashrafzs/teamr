namespace TeamR.IntegrationTests.Core
{
	using System.Threading.Tasks;
	using FluentAssertions;
	using TeamR.DataSeed.Seeders;
	using TeamR.IntegrationTests.Framework;
	using Xunit;

	public class ActivityTypeTest : IntegrationTest
	{
		public ActivityTypeTest(IntegrationTestFixture fixture) : base(fixture)
		{
		}

		[Fact]
		public async Task ActivityTypeCanBeCreatedByAnAuthenticatedUser()
		{
			await this.Seeder.EnsureUser("user");
			var task = await this.Seeder.LoginAs("user").AddActivityType("task");
			task.GetEntity().Should().NotBeNull();
		}
	}
}
