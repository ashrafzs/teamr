namespace TeamR.Core.Menus
{
	using System.Collections.Generic;
	using Teamr.Core.Commands.Activity;
	using Teamr.Core.Commands.ActivityType;
	using TeamR.Infrastructure;
	using TeamR.Infrastructure.Forms.Menu;
	using TeamR.Infrastructure.Security;

	[RegisterEntry("core")]
	public sealed class CoreMenus : MenuContainer
	{
		public const string Notifications = "Notifications";
		public const string Reports = "Reports";
		public const string System = "System";
		public const string Main = "";
		private readonly UserSecurityContext userSecurityContext;

		public CoreMenus(UserSecurityContext userSecurityContext)
		{
			this.userSecurityContext = userSecurityContext;
		}

		public override IEnumerable<MenuItem> GetDynamicMenuItems()
		{
			if (this.userSecurityContext.CanAccess<ActivityTypes>())
			{
				yield return ActivityTypes.Button("Configuration").AsMenuItem(System);
			}

			if (this.userSecurityContext.CanAccess<Calendar>())
			{
				yield return Calendar.Button("Calendar").AsMenuItem(System);
			}
		}

		public override IEnumerable<MenuGroup> GetMenuGroups()
		{
			return new List<MenuGroup>
			{
				new MenuGroup(Main, 2),
				new MenuGroup(Reports, 19),
				new MenuGroup(System, 20),
				new MenuGroup(Notifications, -1)
			};
		}
	}
}