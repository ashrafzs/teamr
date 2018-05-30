// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedMember.Local

namespace Teamr.Core.Domain
{
	using System;

	public class ActivityType
	{
		private ActivityType()
		{
		}

		internal ActivityType(string name, int userId, string unit, decimal points, string remarks)
		{
			this.CreatedOn = DateTime.UtcNow;
			this.Remarks = remarks;
			this.UserId = userId;
			this.Name = name;
			this.Unit = unit;
			this.Points = points;
		}

		public int UserId { get; private set; }

		/// <summary>
		/// Gets date when the leave was requested.
		/// </summary>
		public DateTime CreatedOn { get; private set; }

		public int Id { get; set; }

		public string Name { get; private set; }
		public decimal Points { get; private set; }
		public string Remarks { get; private set; }
		public string Unit { get; private set; }

		public virtual RegisteredUser User { get; private set; }
	}
}