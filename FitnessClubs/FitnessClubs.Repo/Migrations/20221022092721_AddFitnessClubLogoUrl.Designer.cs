﻿// <auto-generated />
using System;
using FitnessClubs.Repo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FitnessClubs.Repo.Migrations
{
    [DbContext(typeof(FitnessClubsDbContext))]
    [Migration("20221022092721_AddFitnessClubLogoUrl")]
    partial class AddFitnessClubLogoUrl
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FitnessClubs.Domain.Models.FitnessClub", b =>
                {
                    b.Property<string>("FitnessClubId")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("FitnessClubLogoUrl")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("FitnessClubName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("FitnessClubId");

                    b.HasIndex("FitnessClubName")
                        .IsUnique();

                    b.ToTable("FitnessClubs");
                });

            modelBuilder.Entity("FitnessClubs.Domain.Models.Membership", b =>
                {
                    b.Property<string>("MemberId")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.Property<string>("FitnessClubId")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.Property<DateTime>("JoiningDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("MemberId", "FitnessClubId");

                    b.ToTable("Memberships");
                });

            modelBuilder.Entity("FitnessClubs.Domain.Models.TrainerEmployment", b =>
                {
                    b.Property<string>("EmploymentId")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.Property<DateTime>("EmployedFrom")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("EmployedTo")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FitnessClubId")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.Property<string>("UserId")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.HasKey("EmploymentId");

                    b.HasIndex("FitnessClubId");

                    b.ToTable("TrainerEmployments");
                });

            modelBuilder.Entity("FitnessClubs.Domain.Models.WorkerEmployment", b =>
                {
                    b.Property<string>("EmploymentId")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.Property<DateTime>("EmployedFrom")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("EmployedTo")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FitnessClubId")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.Property<string>("UserId")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.HasKey("EmploymentId");

                    b.HasIndex("FitnessClubId");

                    b.ToTable("WorkerEmployments");
                });

            modelBuilder.Entity("FitnessClubs.Domain.Models.TrainerEmployment", b =>
                {
                    b.HasOne("FitnessClubs.Domain.Models.FitnessClub", "FitnessClub")
                        .WithMany()
                        .HasForeignKey("FitnessClubId");

                    b.Navigation("FitnessClub");
                });

            modelBuilder.Entity("FitnessClubs.Domain.Models.WorkerEmployment", b =>
                {
                    b.HasOne("FitnessClubs.Domain.Models.FitnessClub", "FitnessClub")
                        .WithMany()
                        .HasForeignKey("FitnessClubId");

                    b.Navigation("FitnessClub");
                });
#pragma warning restore 612, 618
        }
    }
}
