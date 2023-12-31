﻿// <auto-generated />
using System;
using CelebraTix.Promotions.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CelebraTix.Promotions.Migrations
{
    [DbContext(typeof(PromotionDataContext))]
    partial class PromotionDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CelebraTix.Promotions.Venues.Venue", b =>
                {
                    b.Property<int>("VenueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("VenueId"));

                    b.Property<Guid>("VenueGuid")
                        .HasColumnType("uuid");

                    b.HasKey("VenueId");

                    b.HasAlternateKey("VenueGuid");

                    b.ToTable("Venues");
                });

            modelBuilder.Entity("CelebraTix.Promotions.Venues.VenueDescription", b =>
                {
                    b.Property<int>("VenueDescriptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("VenueDescriptionId"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("VenueId")
                        .HasColumnType("integer");

                    b.HasKey("VenueDescriptionId");

                    b.HasAlternateKey("VenueId", "ModifiedDate");

                    b.ToTable("VenueDescription");
                });

            modelBuilder.Entity("CelebraTix.Promotions.Venues.VenueLocation", b =>
                {
                    b.Property<int>("VenueLocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("VenueLocationId"));

                    b.Property<float>("Latitude")
                        .HasColumnType("real");

                    b.Property<float>("Longitude")
                        .HasColumnType("real");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("VenueId")
                        .HasColumnType("integer");

                    b.HasKey("VenueLocationId");

                    b.HasAlternateKey("VenueId", "ModifiedDate");

                    b.ToTable("VenueLocation");
                });

            modelBuilder.Entity("CelebraTix.Promotions.Venues.VenueRemoved", b =>
                {
                    b.Property<int>("VenueRemovedId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("VenueRemovedId"));

                    b.Property<DateTime>("RemovedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("VenueId")
                        .HasColumnType("integer");

                    b.HasKey("VenueRemovedId");

                    b.HasIndex("VenueId");

                    b.ToTable("VenueRemoved");
                });

            modelBuilder.Entity("CelebraTix.Promotions.Venues.VenueTimeZone", b =>
                {
                    b.Property<int>("VenueTimeZoneId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("VenueTimeZoneId"));

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("TimeZone")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("VenueId")
                        .HasColumnType("integer");

                    b.HasKey("VenueTimeZoneId");

                    b.HasAlternateKey("VenueId", "ModifiedDate");

                    b.ToTable("VenueTimeZone");
                });

            modelBuilder.Entity("CelebraTix.Promotions.Venues.VenueDescription", b =>
                {
                    b.HasOne("CelebraTix.Promotions.Venues.Venue", "Venue")
                        .WithMany("Descriptions")
                        .HasForeignKey("VenueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Venue");
                });

            modelBuilder.Entity("CelebraTix.Promotions.Venues.VenueLocation", b =>
                {
                    b.HasOne("CelebraTix.Promotions.Venues.Venue", "Venue")
                        .WithMany("Locations")
                        .HasForeignKey("VenueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Venue");
                });

            modelBuilder.Entity("CelebraTix.Promotions.Venues.VenueRemoved", b =>
                {
                    b.HasOne("CelebraTix.Promotions.Venues.Venue", "Venue")
                        .WithMany("Removed")
                        .HasForeignKey("VenueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Venue");
                });

            modelBuilder.Entity("CelebraTix.Promotions.Venues.VenueTimeZone", b =>
                {
                    b.HasOne("CelebraTix.Promotions.Venues.Venue", "Venue")
                        .WithMany("TimeZones")
                        .HasForeignKey("VenueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Venue");
                });

            modelBuilder.Entity("CelebraTix.Promotions.Venues.Venue", b =>
                {
                    b.Navigation("Descriptions");

                    b.Navigation("Locations");

                    b.Navigation("Removed");

                    b.Navigation("TimeZones");
                });
#pragma warning restore 612, 618
        }
    }
}
