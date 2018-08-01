namespace TeamR.Users.Pickers
{
	using System;
	using System.Linq;
	using TeamR.Infrastructure;
	using TeamR.Infrastructure.Forms;
	using TeamR.Infrastructure.Forms.Typeahead;
	using TeamR.Infrastructure.Security;
	using TeamR.Users.Security;

	[Secure(typeof(UserActions), nameof(UserActions.ManageUsers))]
	public class RoleTypeaheadRemoteSource : TypeaheadRemoteSource<RoleTypeaheadRemoteSource.Request, string>
	{
		private readonly ActionRegister actionRegister;

		public RoleTypeaheadRemoteSource(ActionRegister actionRegister)
		{
			this.actionRegister = actionRegister;
		}

		protected override TypeaheadResponse<string> Handle(Request message)
		{
			var manuallyAssignableRoles = this.actionRegister.GetSystemRoles()
				.Where(t => t.IsDynamicallyAssigned == false);

			if (message.GetByIds)
			{
				return manuallyAssignableRoles
					.Where(t => message.Ids.Items.Any(i => i.Equals(t.Name, StringComparison.OrdinalIgnoreCase)))
					.ToList()
					.AsTypeaheadResponse(t => t.Name, t => t.Name);
			}

			return manuallyAssignableRoles
				.ToList()
				.AsTypeaheadResponse(t => t.Name, t => t.Name);
		}

		public class Request : TypeaheadRequest<string>
		{
		}
	}
}
