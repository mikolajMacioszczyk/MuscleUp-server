﻿// <auto-generated />
using System;
using Carnets.Repo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Carnets.Repo.Migrations
{
    [DbContext(typeof(CarnetsDbContext))]
    partial class CarnetsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Carnets.Domain.Models.AssignedPermission", b =>
                {
                    b.Property<string>("GympassTypeId")
                        .HasColumnType("character varying(36)");

                    b.Property<string>("PermissionId")
                        .HasColumnType("character varying(36)");

                    b.HasKey("GympassTypeId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.ToTable("AssignedPermissions");
                });

            modelBuilder.Entity("Carnets.Domain.Models.Gympass", b =>
                {
                    b.Property<string>("GympassId")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.Property<DateTime>("ActivationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("GympassTypeId")
                        .IsRequired()
                        .HasColumnType("character varying(36)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.Property<DateTime>("ValidityDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("GympassId");

                    b.HasIndex("GympassTypeId");

                    b.ToTable("Gympasses");
                });

            modelBuilder.Entity("Carnets.Domain.Models.GympassType", b =>
                {
                    b.Property<string>("GympassTypeId")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.Property<string>("FitnessClubId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.Property<string>("GympassTypeName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<int>("ValidityPeriodInSeconds")
                        .HasColumnType("integer");

                    b.Property<int>("Version")
                        .HasColumnType("integer");

                    b.HasKey("GympassTypeId");

                    b.ToTable("GympassTypes");
                });

            modelBuilder.Entity("Carnets.Domain.Models.PermissionBase", b =>
                {
                    b.Property<string>("PermissionId")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("PermissionId");

                    b.ToTable("PermissionBase");

                    b.HasDiscriminator<string>("Discriminator").HasValue("PermissionBase");
                });

            modelBuilder.Entity("Carnets.Domain.Models.Subscription", b =>
                {
                    b.Property<string>("SubscriptionId")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.Property<string>("GympassId")
                        .IsRequired()
                        .HasColumnType("character varying(36)");

                    b.Property<string>("StripeCustomerId")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("StripePaymentmethodId")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.HasKey("SubscriptionId");

                    b.HasIndex("GympassId");

                    b.HasIndex("StripeCustomerId")
                        .IsUnique();

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("Carnets.Domain.Models.AllowedEntriesPermission", b =>
                {
                    b.HasBaseType("Carnets.Domain.Models.PermissionBase");

                    b.Property<int>("AllowedEntries")
                        .HasColumnType("integer");

                    b.Property<int>("AllowedEntriesCooldown")
                        .HasColumnType("integer");

                    b.Property<byte>("CooldownType")
                        .HasColumnType("smallint");

                    b.HasDiscriminator().HasValue("AllowedEntriesPermission");
                });

            modelBuilder.Entity("Carnets.Domain.Models.ClassPermission", b =>
                {
                    b.HasBaseType("Carnets.Domain.Models.PermissionBase");

                    b.Property<string>("PermissionName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.HasIndex("PermissionName")
                        .IsUnique();

                    b.HasDiscriminator().HasValue("ClassPermission");
                });

            modelBuilder.Entity("Carnets.Domain.Models.TimePermissionEntry", b =>
                {
                    b.HasBaseType("Carnets.Domain.Models.PermissionBase");

                    b.Property<byte>("EnableEntryFrom")
                        .HasColumnType("smallint");

                    b.Property<byte>("EnableEntryTo")
                        .HasColumnType("smallint");

                    b.HasDiscriminator().HasValue("TimePermissionEntry");
                });

            modelBuilder.Entity("Carnets.Domain.Models.AssignedPermission", b =>
                {
                    b.HasOne("Carnets.Domain.Models.GympassType", "GympassType")
                        .WithMany()
                        .HasForeignKey("GympassTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Carnets.Domain.Models.PermissionBase", "Permission")
                        .WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GympassType");

                    b.Navigation("Permission");
                });

            modelBuilder.Entity("Carnets.Domain.Models.Gympass", b =>
                {
                    b.HasOne("Carnets.Domain.Models.GympassType", "GympassType")
                        .WithMany()
                        .HasForeignKey("GympassTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GympassType");
                });

            modelBuilder.Entity("Carnets.Domain.Models.Subscription", b =>
                {
                    b.HasOne("Carnets.Domain.Models.Gympass", "Gympass")
                        .WithMany()
                        .HasForeignKey("GympassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Gympass");
                });
#pragma warning restore 612, 618
        }
    }
}
