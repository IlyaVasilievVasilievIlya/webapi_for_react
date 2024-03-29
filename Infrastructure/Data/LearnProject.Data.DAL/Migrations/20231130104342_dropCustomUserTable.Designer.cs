﻿// <auto-generated />
using LearnProject.DAL;
using LearnProject.Data.DAL;
using LearnProject.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LearnProject.DAL.Migrations
{
    [DbContext(typeof(RepositoryAppDbContext))]
    [Migration("20231130104342_dropCustomUserTable")]
    partial class dropCustomUserTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("LearnProject.Context.Entities.Car", b =>
                {
                    b.Property<int>("CarId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("car_id")
                        .HasComment("car id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CarId"));

                    b.Property<int>("CarModelId")
                        .HasColumnType("integer")
                        .HasColumnName("car_model_id")
                        .HasComment("car model id");

                    b.Property<string>("Color")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("color")
                        .HasComment("Car's color");

                    b.HasKey("CarId");

                    b.HasIndex("CarModelId");

                    b.ToTable("cars", null, t =>
                        {
                            t.HasComment("car table");
                        });

                    b.HasData(
                        new
                        {
                            CarId = 1,
                            CarModelId = 1,
                            Color = "Yellow"
                        },
                        new
                        {
                            CarId = 2,
                            CarModelId = 2,
                            Color = "Black"
                        },
                        new
                        {
                            CarId = 3,
                            CarModelId = 2,
                            Color = "White"
                        },
                        new
                        {
                            CarId = 4,
                            CarModelId = 3,
                            Color = "Yellow"
                        },
                        new
                        {
                            CarId = 5,
                            CarModelId = 3,
                            Color = "Yellow"
                        },
                        new
                        {
                            CarId = 6,
                            CarModelId = 1,
                            Color = "Yellow"
                        },
                        new
                        {
                            CarId = 7,
                            CarModelId = 1,
                            Color = "Yellow"
                        },
                        new
                        {
                            CarId = 8,
                            CarModelId = 4,
                            Color = "Yellow"
                        },
                        new
                        {
                            CarId = 9,
                            CarModelId = 4,
                            Color = "Yellow"
                        },
                        new
                        {
                            CarId = 10,
                            CarModelId = 4,
                            Color = "Yellow"
                        },
                        new
                        {
                            CarId = 11,
                            CarModelId = 2,
                            Color = "White"
                        });
                });

            modelBuilder.Entity("LearnProject.Context.Entities.CarModel", b =>
                {
                    b.Property<int>("CarModelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("car_id")
                        .HasComment("car id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CarModelId"));

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("brand")
                        .HasComment("car brand");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("model")
                        .HasComment("car model");

                    b.HasKey("CarModelId");

                    b.ToTable("car_models", null, t =>
                        {
                            t.HasComment("car models");
                        });

                    b.HasData(
                        new
                        {
                            CarModelId = 1,
                            Brand = "Toyota",
                            Name = "Mark 1"
                        },
                        new
                        {
                            CarModelId = 2,
                            Brand = "Mercedes",
                            Name = "Benz"
                        },
                        new
                        {
                            CarModelId = 3,
                            Brand = "Toyota",
                            Name = "Mark 2"
                        },
                        new
                        {
                            CarModelId = 4,
                            Brand = "Renault",
                            Name = "Logan"
                        });
                });

            modelBuilder.Entity("LearnProject.Context.Entities.Car", b =>
                {
                    b.HasOne("LearnProject.Context.Entities.CarModel", "CarModel")
                        .WithMany()
                        .HasForeignKey("CarModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CarModel");
                });
#pragma warning restore 612, 618
        }
    }
}
