namespace Teamr.Core.Commands
{
	using System.Collections.Generic;
	using Teamr.Core.Commands.LeaveType;
	using Teamr.Core.Commands.ActivityType;
	using TeamR.Infrastructure;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core.Binding;

	public class TabstripUtility
	{
		public static Tabstrip GetConfigurationTabs<T>()
		{
			return new Tabstrip
			{
				CurrentTab = typeof(T).GetFormId(),
				Tabs = new List<Tab>
				{
					ActivityTypes.Button("Activity types").AsTab(),
					LeaveTypes.Button("Leave types").AsTab()
				}
			};
		}
	}
}