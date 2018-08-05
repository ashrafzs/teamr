namespace TeamR.DataSeed.Seeders
{
	using System;
	using System.Threading.Tasks;
	using Teamr.Core.Commands.Activity;
	using Teamr.Core.Commands.Leave;
	using Teamr.Core.Domain;
	using TeamR.Infrastructure;
	using TeamR.Users;
	using UiMetadataFramework.Basic.Input.Typeahead;

	public class UserQuery : Seeder
	{
		public readonly string EntityName;

		public UserQuery(string entityName, DataSeedDiContainer container, DatabaseEntityTracker tracker) : base(container, tracker)
		{
			this.EntityName = entityName;
		}

		public void Do(Action<UserQuery> action)
		{
			this.Do(this.EntityName, action);
		}

		public async Task Do(Func<UserQuery, Task> action)
		{
			await this.Do(this.EntityName, action);
		}

		public ApplicationUser GetEntity()
		{
			var userId = (int)this.Tracker.GetEntityId<ApplicationUser>(this.EntityName);
			return this.Container.Container.GetInstance<ApplicationDbContext>().Users.SingleOrException(t => t.Id == userId);
		}

		public async Task<ActivityQuery> AddCompletedActivity(string name, ActivityType type, DateTime date, decimal quantity)
		{
			var request = new AddCompletedActivity.Request
			{
				Notes = this.Faker.Lorem.Sentence(),
				Quantity = quantity,
				ActivityTypeId = new TypeaheadValue<int>(type.Id),
				PerformedOn = date
			};

			var response = await this.RunMediatorCommand(request);
			var activity = await this.GetContext().Activities.SingleOrExceptionAsync(t => t.Id == response.Id);

			this.Tracker.RegisterEntity(name, activity);
			return new ActivityQuery(name, this.Container, this.Tracker);
		}

		public async Task<ActivityQuery> AddPlannedActivity(string name, ActivityType type, DateTime date, decimal quantity)
		{
			var request = new AddPlannedActivity.Request
			{
				Notes = this.Faker.Lorem.Sentence(),
				Quantity = quantity,
				ActivityTypeId = new TypeaheadValue<int>(type.Id),
				ScheduledOn = date
			};

			var response = await this.RunMediatorCommand(request);
			var activity = await this.GetContext().Activities.SingleOrExceptionAsync(t => t.Id == response.Id);

			this.Tracker.RegisterEntity(name, activity);
			return new ActivityQuery(name, this.Container, this.Tracker);
		}

		public async Task<LeaveQuery> RequestLeave(string name, LeaveType al, DateTime date)
		{
			var request = new AddLeave.Request
			{
				ScheduledOn = date,
				Notes = this.Faker.Lorem.Sentence(),
				LeaveTypeId = new TypeaheadValue<int>(al.Id)
			};

			var response = await this.RunMediatorCommand(request);
			var leave = await this.GetContext().Leaves.SingleOrExceptionAsync(t => t.Id == response.Id);

			this.Tracker.RegisterEntity(name, leave);
			return new LeaveQuery(name, this.Container, this.Tracker);
		}
	}
}