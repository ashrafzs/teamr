namespace Teamr.Core.Menus
{
	using System.Collections.Generic;
	using Teamr.Infrastructure.Forms.Menu;

	public sealed class CoreMenus : IMenuContainer
	{
		public const string Reports = "Reports";
		public const string Activity = "Activity";
		public const string System = "System";
		public const string Main = "";
		public const string Leave = "Leave";

		public IList<MenuMetadata> GetMenuMetadata()
		{
			return new List<MenuMetadata>
			{
				new MenuMetadata(Main, 2),
				new MenuMetadata(Activity, 14),
				new MenuMetadata(Leave, 16),
				new MenuMetadata(Reports, 19),
				new MenuMetadata(System, 20),
			};
		}
	}
}
