// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace Teamr.Core.Domain
{
	using System.Collections.Generic;
	using Teamr.Core.Commands.Activity;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core.Binding;

	/// <summary>
	/// Represents user registered in the system.
	/// </summary>
	public class RegisteredUser
	{
		private RegisteredUser()
		{
			// This constructor is private, because we are not supposed to create new users
			// from this library. All users are created by *Unops.Spgs.Users*. This assembly
			// can only read existing data.
		}

		public int Id { get; private set; }
		public string Name { get; private set; }

		internal static RegisteredUser Create(int userId, string name)
		{
			return new RegisteredUser
			{
				Id = userId,
				Name = name
			};
		}

		public FormLink GetUserProfileLink()
		{
			return new FormLink
			{
				Label = this.Name,
				Form = typeof(UserProfile).GetFormId(),
				InputFieldValues = new Dictionary<string, object>
				{
					{ nameof(UserProfile.Request.UserId), this.Id }
				}

			};
		}
	}
}