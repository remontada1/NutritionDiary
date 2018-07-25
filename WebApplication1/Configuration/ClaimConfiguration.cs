using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using WebApplication1.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Configuration
{
    public class ClaimConfiguration : EntityTypeConfiguration<Claim>
    {
        internal ClaimConfiguration()
        {
            ToTable("Claim");

            HasKey(x => x.ClaimId)
                .Property(x => x.ClaimId)
                .HasColumnName("ClaimId")
                .HasColumnType("int")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();

            Property(x => x.UserId)
                .HasColumnName("UserId")
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            Property(x => x.ClaimType)
                .HasColumnName("ClaimType")
                .HasColumnType("nvarchar")
                .IsMaxLength()
                .IsOptional();

            Property(x => x.ClaimValue)
                .HasColumnName("ClaimValue")
                .HasColumnType("nvarchar")
                .IsMaxLength()
                .IsOptional();

            HasRequired(x => x.User)
                .WithMany(x => x.Claims)
                .HasForeignKey(x => x.UserId);
        }
    }
}