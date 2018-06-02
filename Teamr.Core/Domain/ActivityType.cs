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

		internal void Edit(string name, string unit, decimal points, string remarks)
		{
			this.Points = points;
			this.Name = name;
			this.Unit = unit;
			this.Remarks = remarks;
		}	

		/// <summary>
		/// Gets date when the activity created.
		/// </summary>
		public DateTime CreatedOn { get; private set; }

		public int Id { get; set; }

		/// <summary>
		/// Gets name of the activity.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Get and set points for this activity
		/// </summary>
		public decimal Points { get; private set; }

		public string Remarks { get; private set; }

		public string Unit { get; private set; }

		public virtual RegisteredUser User { get; private set; }

		public int UserId { get; private set; }
	}
}