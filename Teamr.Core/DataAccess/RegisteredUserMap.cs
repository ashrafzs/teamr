﻿namespace TeamR.Core.DataAccess
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	using TeamR.Core.Domain;

	public class RegisteredUserMap : IEntityTypeConfiguration<RegisteredUser>
	{
		public void Configure(EntityTypeBuilder<RegisteredUser> entity)
		{
			entity.ToTable("AspNetUsers");
			entity.HasKey(c => c.Id);

			entity.Property(t => t.Id).HasColumnName("Id");
			entity.Property(t => t.Name).HasColumnName("UserName");
		}
	}
}