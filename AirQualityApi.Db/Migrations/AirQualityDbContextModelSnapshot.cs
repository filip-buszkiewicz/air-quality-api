﻿// <auto-generated />
using AirQualityApi.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AirQualityApi.Db.Migrations
{
    [DbContext(typeof(AirQualityDbContext))]
    partial class AirQualityDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AirQualityApi.Db.Models.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("AirQualityApi.Db.Models.Sensor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ParamIdParam")
                        .HasColumnType("int");

                    b.Property<int>("StationId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ParamIdParam");

                    b.HasIndex("StationId");

                    b.ToTable("Sensors");
                });

            modelBuilder.Entity("AirQualityApi.Db.Models.SensorParams", b =>
                {
                    b.Property<int>("IdParam")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdParam"));

                    b.Property<string>("ParamCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ParamFormula")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ParamName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdParam");

                    b.ToTable("SensorParams");
                });

            modelBuilder.Entity("AirQualityApi.Db.Models.Station", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AddressStreet")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<string>("GegrLat")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GegrLon")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StationName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("Stations");
                });

            modelBuilder.Entity("AirQualityApi.Db.Models.Sensor", b =>
                {
                    b.HasOne("AirQualityApi.Db.Models.SensorParams", "Param")
                        .WithMany()
                        .HasForeignKey("ParamIdParam")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AirQualityApi.Db.Models.Station", "Station")
                        .WithMany()
                        .HasForeignKey("StationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Param");

                    b.Navigation("Station");
                });

            modelBuilder.Entity("AirQualityApi.Db.Models.Station", b =>
                {
                    b.HasOne("AirQualityApi.Db.Models.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });
#pragma warning restore 612, 618
        }
    }
}
