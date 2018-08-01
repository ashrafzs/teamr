namespace TeamR.App.EventNotification.EventHandlers
{
	using System.Linq;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Options;
	using TeamR.App.EventNotification.Emails.Templates;
	using TeamR.Core.DataAccess;
	using TeamR.Core.Events;
	using TeamR.Core.Notification;
	using TeamR.Core.Security;
	using TeamR.Infrastructure;
	using TeamR.Infrastructure.Configuration;
	using TeamR.Infrastructure.Domain;
	using TeamR.Infrastructure.Emails;
	using TeamR.Notifications;
	using TeamR.Users;

	public class OnActivityRecorded : AppEventHandler<ActivityRecorded>
	{
		private readonly IOptions<AppConfig> appConfig;
		private readonly CoreDbContext dbContext;
		private readonly EmailTemplateRegister emailSender;
		private readonly NotificationsDbContext notificationsDbContext;
		private readonly UserManager<ApplicationUser> userManager;

		public OnActivityRecorded(
			EventManager manager,
			EmailTemplateRegister emailSender,
			CoreDbContext dbContext,
			IOptions<AppConfig> appConfig,
			NotificationsDbContext notificationsDbContext,
			UserManager<ApplicationUser> userManager) : base(manager)
		{
			this.emailSender = emailSender;
			this.dbContext = dbContext;
			this.appConfig = appConfig;
			this.notificationsDbContext = notificationsDbContext;
			this.userManager = userManager;
		}

		public override void HandleEvent(ActivityRecorded @event)
		{
			var activity = this.dbContext.Activities
				.Include(t => t.ActivityType)
				.Include(t => t.CreatedByUser)
				.SingleOrException(t => t.Id == @event.Context.Id);

			var admins = this.userManager.Users
				.Where(t => t.Roles.Any(c => c.Role.Name == CoreRoles.Admin.Name))
				.ToList();

			foreach (var admin in admins)
			{
				this.emailSender.SendEmail(
					admin.Email,
					new ActivityRecordedTemplate.Model(activity, this.appConfig.Value)).Wait();

				this.notificationsDbContext.PublishForUser(
					"",
					activity,
					admin.Id,
					$"{activity.CreatedByUser.Name} recorded activity '{activity.ActivityType.Name}'",
					NotificationCategory.Activity).Wait();
			}
		}
	}
}
