namespace Teamr.Core.DataAccess
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	using Teamr.Core.Domain;

	internal class LeaveTypeMap : IEntityTypeConfiguration<LeaveType>
	{
		public void Configure(EntityTypeBuilder<LeaveType> entity)
		{
			entity.ToTable("LeaveType");
			entity.HasKey(t => t.Id);
			entity.Property(t => t.Remarks).HasColumnName("Remarks").IsUnicode(false);
			entity.Property(t => t.UserId).HasColumnName("UserId");
			entity.Property(t => t.Id).HasColumnName("Id");
			entity.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
			entity.Property(t => t.Quantity).HasColumnName("Quantity");
			entity.Property(t => t.Name).HasColumnName("Name").IsUnicode(false).HasMaxLength(100);
			entity.Property(t => t.CreatedOn).HasColumnName("CreatedOn");

			entity.HasOne(t => t.User)
				.WithMany()
				.HasForeignKey(t => t.UserId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
