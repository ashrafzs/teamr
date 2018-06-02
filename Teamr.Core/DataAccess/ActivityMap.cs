namespace Teamr.Core.DataAccess
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	using Teamr.Core.Domain;

	internal class ActivityMap : IEntityTypeConfiguration<Activity>
	{
		public void Configure(EntityTypeBuilder<Activity> entity)
		{
			entity.ToTable("Activity");
			entity.HasKey(t => t.Id);
			entity.Property(t => t.Notes).HasColumnName("Notes").IsUnicode(false);
			entity.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
			entity.Property(t => t.ActivityTypeId).HasColumnName("ActivityTypeId");
			entity.Property(t => t.Id).HasColumnName("Id");
			entity.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
			entity.Property(t => t.ScheduledOn).HasColumnName("ScheduledOn");
			entity.Property(t => t.PerformedOn).HasColumnName("PerformedOn");
			entity.Property(t => t.Quantity).HasColumnName("Quantity");

			entity.HasOne(t => t.CreatedByUser)
				.WithMany()
				.HasForeignKey(t => t.CreatedByUserId);

			entity.HasOne(t => t.ActivityType)
				.WithMany()
				.HasForeignKey(t => t.ActivityTypeId);
		}
	}
}
