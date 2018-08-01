namespace TeamR.Core.Menus
{
	using System;
	using System.Collections.Generic;
	using TeamR.Core.Notification;
	using TeamR.Infrastructure;
	using TeamR.Infrastructure.Forms;
	using TeamR.Infrastructure.Forms.Menu;
	using TeamR.Infrastructure.Security;
	using TeamR.Notifications;

	[RegisterEntry("core")]
	public sealed class CoreMenus : MenuContainer
	{
		public const string Notifications = "Notifications";
		public const string Reports = "Reports";
		public const string Activity = "Activity";
		public const string System = "System";
		public const string Main = "";
		public const string Leave = "Leave";
		private readonly NotificationsDbContext context;
		private readonly UserSecurityContext userSecurityContext;

		public CoreMenus(NotificationsDbContext context, UserSecurityContext userSecurityContext)
		{
			this.context = context;
			this.userSecurityContext = userSecurityContext;
		}

		public override IEnumerable<MenuItem> GetDynamicMenuItems()
		{
			if (this.userSecurityContext.CanAccess(typeof(MyNotifications)))
			{
				//var notificationCount = this.context.Notifications
				//	.Count(t => t.ReadOn == null && t.ArchivedOn == null);

				var count = new Random().Next(1, 100);

				yield return MyNotifications
					.Button($"<i class='far fa-bell' title='Notifications'></i>{UiFormConstants.CounterForTopMenu(count)}")
					.AsMenuItem(Notifications);
			}
		}

		public override IEnumerable<MenuGroup> GetMenuGroups()
		{
			return new List<MenuGroup>
			{
				new MenuGroup(Main, 2),
				new MenuGroup(Activity, 14),
				new MenuGroup(Leave, 16),
				new MenuGroup(Reports, 19),
				new MenuGroup(System, 20),
				new MenuGroup(Notifications, -1)
			};
		}
	}
}