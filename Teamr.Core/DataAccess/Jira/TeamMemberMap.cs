namespace TeamR.Core.DataAccess.Jira
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	using TeamR.Core.Domain.Jira;

	internal class TeamMemberMap : IEntityTypeConfiguration<TeamMember>
	{
		public void Configure(EntityTypeBuilder<TeamMember> entity)
		{
			entity.ToTable("Issue");
			entity.HasKey(t => t.Id);
			entity.Property(t => t.Id).HasColumnName("Id");
			entity.Property(t => t.Username).HasColumnName("Username");
			entity.Property(t => t.UserId).HasColumnName("UserId");
		}
	}
}