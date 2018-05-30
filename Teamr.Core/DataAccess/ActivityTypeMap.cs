namespace Teamr.Core.DataAccess
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	using Teamr.Core.Domain;

	internal class ActivityTypeMap : IEntityTypeConfiguration<ActivityType>
	{
		public void Configure(EntityTypeBuilder<ActivityType> entity)
		{
			entity.ToTable("ActivityType");
			entity.HasKey(t => t.Id);
			entity.Property(t => t.Remarks).HasColumnName("Remarks").IsUnicode(false);
			entity.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
			entity.Property(t => t.Id).HasColumnName("Id");
			entity.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
			entity.Property(t => t.Points).HasColumnName("Points");
			entity.Property(t => t.Name).HasColumnName("Name").IsUnicode(false).HasMaxLength(100);
			entity.Property(t => t.Unit).HasColumnName("Unit").IsUnicode(false).HasMaxLength(250);
			entity.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
		}
	}
}
