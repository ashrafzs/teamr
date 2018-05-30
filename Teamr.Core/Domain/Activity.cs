// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedMember.Local

namespace Teamr.Core.Domain
{
	using System;

	public class Activity
	{
		private Activity()
		{
		}

		internal Activity(int userId, ActivityType type, decimal quantity, string notes, DateTime? performedOn)
		{
			this.CreatedOn = DateTime.UtcNow;
			this.Notes = notes;
			this.CreatedByUserId = userId;
			this.Quantity = quantity;
			this.PerformedOn = performedOn;
			this.ActivityTypeId = type.Id;
		}

		public int CreatedByUserId { get; private set; }
		public virtual RegisteredUser CreatedByUser { get; private set; }

		public int ActivityTypeId { get; private set; }
		public virtual ActivityType ActivityType { get; private set; }

		public DateTime CreatedOn { get; private set; }
		public DateTime? PerformedOn { get; private set; }

		public int Id { get; set; }
		public decimal Quantity { get; private set; }
		public string Notes { get; private set; }
	}
}