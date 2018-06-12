// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedMember.Local

namespace Teamr.Core.Domain
{
	using System;
	using Teamr.Core.Commands.Activity;
	using UiMetadataFramework.Basic.Output;

	public class Activity
	{
		private Activity()
		{
		}

		internal Activity(int userId, ActivityType type, decimal quantity, string notes, DateTime scheduledOn, DateTime? performedOn = null)
		{
			this.CreatedOn = DateTime.UtcNow;
			this.Notes = notes;
			this.CreatedByUserId = userId;
			this.Quantity = quantity;
			this.ScheduledOn = scheduledOn;
			this.PerformedOn = performedOn;
			this.ActivityType = type;
			this.Points = type.Points * quantity;
		}

		public virtual ActivityType ActivityType { get; private set; }

		public int ActivityTypeId { get; private set; }
		public virtual RegisteredUser CreatedByUser { get; private set; }

		public int CreatedByUserId { get; private set; }

		public DateTime CreatedOn { get; private set; }

		public int Id { get; set; }
		public string Notes { get; private set; }
		public DateTime? PerformedOn { get; private set; }
		public decimal Points { get; private set; }
		public decimal Quantity { get; private set; }
		public DateTime ScheduledOn { get; private set; }

		public void EditActivityType(int activityTypeId)
		{
			this.ActivityTypeId = activityTypeId;
		}

		public void EditNotes(string notes)
		{
			this.Notes = notes;
		}

		public void EditPerformedDate(DateTime performedOn)
		{
			this.PerformedOn = performedOn;
		}

		public void EditPoints(decimal points)
		{
			this.Points = points * this.Quantity;
		}

		public ActionList GetActions(bool canManage)
		{
			if (canManage)
			{
				var result = new ActionList();

				if(this.CreatedOn.AddDays(5) > DateTime.UtcNow || this.PerformedOn == null  || this.PerformedOn?.AddDays(5) > DateTime.UtcNow)
				{
					result.Actions.Add(EditActivity.Button(this.Id));
					result.Actions.Add(DeleteActivity.Button(this.Id));
				}

				if (this.PerformedOn == null)
				{
					result.Actions.Add(PerformActivity.Button(this.Id));
				}
				return result;
			}

			return null;
		}
	}
}