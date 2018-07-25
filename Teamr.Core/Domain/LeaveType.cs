// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedMember.Local

namespace Teamr.Core.Domain
{
	using System;

	public class LeaveType
	{
		private LeaveType()
		{
		}

		internal LeaveType(string name, int userId, decimal quantity, string remarks, string tag)
		{
			this.CreatedOn = DateTime.UtcNow;
			this.Remarks = remarks;
			this.UserId = userId;
			this.Name = name;
			this.Quantity = quantity;
			this.Tag = tag;
		}

		/// <summary>
		/// Gets date when the leave created.
		/// </summary>
		public DateTime CreatedOn { get; private set; }

		public int Id { get; set; }

		/// <summary>
		/// Gets name of the leave.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Get and set quantity for this leave
		/// </summary>
		public decimal Quantity { get; private set; }

		public string Remarks { get; private set; }

		public string Tag { get; private set; }

		public virtual RegisteredUser User { get; private set; }

		public int UserId { get; private set; }

		internal void Edit(string name, decimal quantity, string remarks,string tag)
		{
			this.Quantity = quantity;
			this.Name = name;
			this.Remarks = remarks;
			this.Tag = tag;
		}
	}
}