﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Configuration
{
    public class ExternalLoginConfiguration : EntityTypeConfiguration<ExternalLogin>
    {
        public ExternalLoginConfiguration()
        {
            ToTable("ExternalLogin");

            HasKey(x => new { x.LoginProvider, x.ProviderKey, x.UserId });

            Property(x => x.LoginProvider)
                .HasColumnName("LoginProvider")
                .HasColumnType("nvarchar")
                .HasMaxLength(128)
                .IsRequired();

            Property(x => x.ProviderKey)
                .HasColumnName("ProviderKey")
                .HasColumnType("nvarchar")
                .HasMaxLength(128)
                .IsRequired();

            Property(x => x.UserId)
                .HasColumnName("UserId")
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            HasRequired(x => x.User)
                .WithMany(x => x.Logins)
                .HasForeignKey(x => x.UserId);

        }
    }
}