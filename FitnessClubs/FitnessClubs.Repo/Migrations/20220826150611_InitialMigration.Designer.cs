﻿// <auto-generated />
using FitnessClubs.Repo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FitnessClubs.Repo.Migrations
{
    [DbContext(typeof(FintessClubsDbContext))]
    [Migration("20220826150611_InitialMigration")]
    partial class InitialMigration
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
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("FitnessClubName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("FitnessClubId");

                    b.HasIndex("FitnessClubName")
                        .IsUnique();

                    b.ToTable("FitnessClubs");
                });

            modelBuilder.Entity("FitnessClubs.Domain.Models.TrainerEmployment", b =>
                {
                    b.Property<string>("UserId")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("FitnessClubId")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.HasKey("UserId", "FitnessClubId");

                    b.HasIndex("FitnessClubId");

                    b.ToTable("TrainerEmployments");
                });

            modelBuilder.Entity("FitnessClubs.Domain.Models.WorkerEmployment", b =>
                {
                    b.Property<string>("UserId")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("FitnessClubId")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.HasKey("UserId", "FitnessClubId");

                    b.HasIndex("FitnessClubId");

                    b.ToTable("WorkerEmployments");
                });

            modelBuilder.Entity("FitnessClubs.Domain.Models.TrainerEmployment", b =>
                {
                    b.HasOne("FitnessClubs.Domain.Models.FitnessClub", "FitnessClub")
                        .WithMany()
                        .HasForeignKey("FitnessClubId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FitnessClub");
                });

            modelBuilder.Entity("FitnessClubs.Domain.Models.WorkerEmployment", b =>
                {
                    b.HasOne("FitnessClubs.Domain.Models.FitnessClub", "FitnessClub")
                        .WithMany()
                        .HasForeignKey("FitnessClubId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FitnessClub");
                });
#pragma warning restore 612, 618
        }
    }
}
