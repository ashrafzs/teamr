namespace TeamR.Core.DataAccess.Jira
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	using TeamR.Core.Domain.Jira;

	internal class ProjectMap : IEntityTypeConfiguration<Project>
	{
		public void Configure(EntityTypeBuilder<Project> entity)
		{
			entity.ToTable("Issue");
			entity.HasKey(t => t.Id);
			entity.Property(t => t.Id).HasColumnName("Id");
			entity.Property(t => t.Name).HasColumnName("Name");
		}
	}
}