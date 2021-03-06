// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedMember.Local

namespace Teamr.Core.Domain
{
	using System;
	using TeamR.Core.Domain;
	using TeamR.Infrastructure.Domain;

	public class ActivityType : DomainEntityWithKeyInt32
	{
		private ActivityType()
		{
		}

		internal ActivityType(string name, int userId, string unit, decimal points, string remarks,string tag)
		{
			this.CreatedOn = DateTime.UtcNow;
			this.Remarks = remarks;
			this.UserId = userId;
			this.Name = name;
			this.Unit = unit;
			this.Points = points;
			this.Tag = tag;
		}

		internal void Edit(string name, string unit, decimal points, string remarks, string tag)
		{
			this.Points = points;
			this.Name = name;
			this.Unit = unit;
			this.Remarks = remarks;
			this.Tag = tag;
		}	

		/// <summary>
		/// Gets date when the activity created.
		/// </summary>
		public DateTime CreatedOn { get; private set; }
		
		/// <summary>
		/// Gets name of the activity.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Get and set points for this activity
		/// </summary>
		public decimal Points { get; private set; }

		public string Remarks { get; private set; }

		public string Tag { get; private set; }

		public string Unit { get; private set; }

		public virtual RegisteredUser User { get; private set; }

		public int UserId { get; private set; }
	}
}