namespace TeamR.Users.Pickers
{
	using System.Collections.Generic;
	using System.Linq;
	using TeamR.Infrastructure;
	using TeamR.Infrastructure.Security;
	using UiMetadataFramework.Basic.Input.Typeahead;
	using TeamR.Users.Security;

	[Secure(typeof(UserActions), nameof(UserActions.ManageUsers))]
	public class RoleTypeaheadInlineSource : ITypeaheadInlineSource<string>
	{
		private readonly ActionRegister actionRegister;

		public RoleTypeaheadInlineSource(ActionRegister actionRegister)
		{
			this.actionRegister = actionRegister;
		}

		public IEnumerable<TypeaheadItem<string>> GetItems()
		{
			return this.actionRegister
				.GetSystemRoles()
				.Where(t => t.IsDynamicallyAssigned == false)
				.ToList()
				.AsTypeaheadResponse(t => t.Name, t => t.Name).Items;
		}
	}
}
