namespace Teamr.Core.Security.SomeThing
{
	using Teamr.Infrastructure.Security;

	public class SomeThingRole : Role
	{
		public static SomeThingRole Owner = new SomeThingRole(nameof(Owner));
		public static SomeThingRole Viewer = new SomeThingRole(nameof(Viewer));

		private SomeThingRole(string name) : base(name)
		{
		}
	}
}
