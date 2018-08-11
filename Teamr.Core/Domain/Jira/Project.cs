// ReSharper disable UnusedMember.Local

namespace TeamR.Core.Domain.Jira
{
	using TeamR.Infrastructure.Domain;

	public class Project : DomainEntityWithKeyInt32
	{
		private Project()
		{
		}

		internal Project(string name)
		{
			this.Name = name;
		}

		/// <summary>
		/// Gets name of the project.
		/// </summary>
		public string Name { get; private set; }

		internal void Edit(string name)
		{
			this.Name = name;
		}
	}
}