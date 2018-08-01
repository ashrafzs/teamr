// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedMember.Local

namespace Teamr.Core.Domain
{
	using System;
	using TeamR.Core.Domain;
	using TeamR.Infrastructure.Domain;

	public class Leave : DomainEntityWithKeyInt32
	{
		private Leave()
		{
		}

		internal Leave(int userId, LeaveType type, string notes, DateTime scheduledOn)
		{
			this.CreatedOn = DateTime.UtcNow;
			this.Notes = notes;
			this.CreatedByUserId = userId;
			this.ScheduledOn = scheduledOn;
			this.LeaveType = type;
		}

		public virtual RegisteredUser CreatedByUser { get; private set; }

		public int CreatedByUserId { get; private set; }

		public DateTime CreatedOn { get; private set; }

		public virtual LeaveType LeaveType { get; private set; }

		public int LeaveTypeId { get; private set; }
		public string Notes { get; private set; }
		public DateTime ScheduledOn { get; private set; }

		public void Edit(string notes, int typeId, DateTime scheduledOn)
		{
			this.LeaveTypeId = typeId;
			this.ScheduledOn = scheduledOn;
			this.Notes = notes;
		}

	}
}