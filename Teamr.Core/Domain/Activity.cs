// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedMember.Local

namespace Teamr.Core.Domain
{
	using System;
	using UiMetadataFramework.Basic.Input.Typeahead;

	public class Activity
	{
		private Activity()
		{
		}

		internal Activity(int userId, ActivityType type, decimal quantity, string notes, DateTime scheduledOn, DateTime? performedOn)
		{
			this.CreatedOn = DateTime.UtcNow;
			this.Notes = notes;
			this.CreatedByUserId = userId;
			this.Quantity = quantity;
			this.ScheduledOn = scheduledOn;
			this.PerformedOn = performedOn;
			this.ActivityType = type;
		}

		public int CreatedByUserId { get; private set; }
		public virtual RegisteredUser CreatedByUser { get; private set; }

		public int ActivityTypeId { get; private set; }
		public virtual ActivityType ActivityType { get; private set; }

		public DateTime CreatedOn { get; private set; }
		public DateTime ScheduledOn { get; private set; }
		public DateTime? PerformedOn { get; private set; }

		public int Id { get; set; }
		public decimal Quantity { get; private set; }
		public string Notes { get; private set; }

		public void EditPerformedDate(DateTime performedOn)
		{
			this.PerformedOn = performedOn;
		}

		public void EditNotes(string notes)
		{
			this.Notes = notes;
		}

		public void EditActivityType(int activityTypeId)
		{
			this.ActivityTypeId = activityTypeId;
		}
	}
}