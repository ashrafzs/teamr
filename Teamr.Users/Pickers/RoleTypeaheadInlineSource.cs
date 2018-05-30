namespace Teamr.Users.Pickers
{
	using System.Collections.Generic;
	using System.Linq;
	using Teamr.Infrastructure;
	using Teamr.Infrastructure.Security;
	using UiMetadataFramework.Basic.Input.Typeahead;

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
