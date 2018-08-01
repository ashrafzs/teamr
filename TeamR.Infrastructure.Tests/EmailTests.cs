namespace TeamR.Infrastructure.Tests
{
	using System;
	using System.Threading.Tasks;
	using FluentAssertions;
	using Teamr.Core.Domain;
	using TeamR.App.EventNotification.Emails.Templates;
	using TeamR.Core.Domain;
	using TeamR.DataSeed;
	using TeamR.Infrastructure.Configuration;
	using TeamR.Infrastructure.Emails;
	using Xunit;

	public class EmailTests
	{
		[Fact]
		public async Task CanCompileEmail()
		{
			var container = new DataSeedDiContainer();
			var emailTemplateRegister = container.Container.GetInstance<EmailTemplateRegister>();

			var user = RegisteredUser.Create(1, "jack");
			var activityType = new ActivityType("do magic", user.Id, "SP", 1, null, null);
			var activity = new Activity(user.Id, activityType, 1, null, DateTime.Today);

			var model = new ActivityRecordedTemplate.Model(activity, new AppConfig());

			var email = await emailTemplateRegister.CompileEmail(
				"test@example.com",
				model);

			email.Should().NotBeNull();
		}
	}
}
