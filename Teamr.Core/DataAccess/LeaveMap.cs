namespace Teamr.Core.DataAccess
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	using Teamr.Core.Domain;

	internal class LeaveMap : IEntityTypeConfiguration<Leave>
	{
		public void Configure(EntityTypeBuilder<Leave> entity)
		{
			entity.ToTable("Leave");
			entity.HasKey(t => t.Id);
			entity.Property(t => t.Notes).HasColumnName("Notes").IsUnicode(false);
			entity.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
			entity.Property(t => t.LeaveTypeId).HasColumnName("LeaveTypeId");
			entity.Property(t => t.Id).HasColumnName("Id");
			entity.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
			entity.Property(t => t.ScheduledOn).HasColumnName("ScheduledOn");

			entity.HasOne(t => t.CreatedByUser)
				.WithMany()
				.HasForeignKey(t => t.CreatedByUserId);

			entity.HasOne(t => t.LeaveType)
				.WithMany()
				.HasForeignKey(t => t.LeaveTypeId);
		}
	}
}
