// ReSharper disable UnusedMember.Local

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
namespace TeamR.Core.Domain.Jira
{
	using TeamR.Infrastructure.Domain;

	public class TeamMember : DomainEntityWithKeyInt32
	{
		private TeamMember(int userId)
		{
			this.UserId = userId;
		}

		internal TeamMember(string username, int userId)
		{
			this.Username = username;
			this.UserId = userId;
		}

		/// <summary>
		/// Gets username of the team member at JIRA.
		/// </summary>
		public string Username { get; private set; }

		public int UserId { get; private set; }
	}
}