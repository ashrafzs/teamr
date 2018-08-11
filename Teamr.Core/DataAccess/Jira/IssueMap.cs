namespace TeamR.Core.DataAccess.Jira
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	using TeamR.Core.Domain.Jira;

	internal class IssueMap : IEntityTypeConfiguration<Issue>
	{
		public void Configure(EntityTypeBuilder<Issue> entity)
		{
			entity.ToTable("Issue");
			entity.HasKey(t => t.Id);
			entity.Property(t => t.IssueKey).HasColumnName("Key").IsUnicode(false);
			entity.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
			entity.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
			entity.Property(t => t.Id).HasColumnName("Id");
			entity.Property(t => t.InProgressOn).HasColumnName("InProgressOn");
			entity.Property(t => t.ReadyForDeployOn).HasColumnName("ReadyForDeployOn");
			entity.Property(t => t.ResolvedOn).HasColumnName("ResolvedOn");
			entity.Property(t => t.StoryPoints).HasColumnName("StoryPoints");

			entity.HasOne(t => t.CreatedByUser)
				.WithMany()
				.HasForeignKey(t => t.CreatedByUserId);

			entity.HasOne(t => t.ReviewedByUser)
				.WithMany()
				.HasForeignKey(t => t.ReviewedByUserId);

			entity.HasOne(t => t.AssignedToUser)
				.WithMany()
				.HasForeignKey(t => t.AssignedToUserId);

			entity.HasOne(t => t.Project)
				.WithMany()
				.HasForeignKey(t => t.ProjectId);
		}
	}
}