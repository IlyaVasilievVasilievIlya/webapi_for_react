﻿// <auto-generated />
using System;
using LearnProject.Data.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LearnProject.DAL.Migrations
{
    [DbContext(typeof(RepositoryAppDbContext))]
    [Migration("20231207065232_addRoles")]
    partial class addRoles
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("LearnProject.Domain.Entities.Car", b =>
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

                    b.HasKey("CarId")
                        .HasName("pk_cars");

                    b.HasIndex("CarModelId")
                        .HasDatabaseName("ix_cars_car_model_id");

                    b.ToTable("cars");

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

            modelBuilder.Entity("LearnProject.Domain.Entities.CarModel", b =>
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

                    b.HasKey("CarModelId")
                        .HasName("pk_car_brand_models");

                    b.ToTable("car_brand_models");

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

            modelBuilder.Entity("LearnProject.Domain.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer")
                        .HasColumnName("access_failed_count");

                    b.Property<DateOnly>("BirthDate")
                        .HasColumnType("date")
                        .HasColumnName("birth_date")
                        .HasComment("user birth date");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text")
                        .HasColumnName("concurrency_stamp");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("email");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean")
                        .HasColumnName("email_confirmed");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("lockout_enabled");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("lockout_end");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name")
                        .HasComment("user name");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("normalized_email");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("normalized_user_name");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text")
                        .HasColumnName("password_hash");

                    b.Property<string>("Patronymic")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("patronymic")
                        .HasComment("user patronymic");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text")
                        .HasColumnName("phone_number");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean")
                        .HasColumnName("phone_number_confirmed");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text")
                        .HasColumnName("security_stamp");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("surname")
                        .HasComment("user surname");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("two_factor_enabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("user_name");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("email_index");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("user_name_index");

                    b.ToTable("users", null, t =>
                        {
                            t.HasComment("users table");
                        });

                    b.HasData(
                        new
                        {
                            Id = "8e445865-a24d-4543-a6c6-9443d048cdb9",
                            AccessFailedCount = 0,
                            BirthDate = new DateOnly(1999, 2, 2),
                            ConcurrencyStamp = "e484e650-d4aa-49b1-8f4b-9549376dd8ea",
                            Email = "ilyavasilev56@gmail.com",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            Name = "Ivan",
                            NormalizedEmail = "ILYAVASILEV56@GMAIL.COM",
                            NormalizedUserName = "ILYAVASILEV56@GMAIL.COM",
                            PasswordHash = "AQAAAAIAAYagAAAAEOoVJcEnnv4YQPTzBfMrd5CM1TFTAYXgz1oJnXjaC+oOZx212NTQ3AkSirQAYhJ8lA==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "cf1725a7-7b20-496a-9ea9-08fd4e10fce5",
                            Surname = "Ivanov",
                            TwoFactorEnabled = false,
                            UserName = "ilyavasilev56@gmail.com"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text")
                        .HasColumnName("concurrency_stamp");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("name");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("normalized_name");

                    b.HasKey("Id")
                        .HasName("pk_asp_net_roles");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("role_name_index");

                    b.ToTable("asp_net_roles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                            Name = "SuperUser",
                            NormalizedName = "SUPERUSER"
                        },
                        new
                        {
                            Id = "2c5e174e-3b0e-446f-86af-483d56fd7211",
                            Name = "Administrator",
                            NormalizedName = "ADMINISTRATOR"
                        },
                        new
                        {
                            Id = "2c5e174e-3b0e-446f-86af-483d56fd7212",
                            Name = "Manager",
                            NormalizedName = "MANAGER"
                        },
                        new
                        {
                            Id = "2c5e174e-3b0e-446f-86af-483d56fd7213",
                            Name = "User",
                            NormalizedName = "USER"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text")
                        .HasColumnName("claim_type");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text")
                        .HasColumnName("claim_value");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("role_id");

                    b.HasKey("Id")
                        .HasName("pk_asp_net_role_claims");

                    b.HasIndex("RoleId")
                        .HasDatabaseName("ix_asp_net_role_claims_role_id");

                    b.ToTable("asp_net_role_claims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text")
                        .HasColumnName("claim_type");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text")
                        .HasColumnName("claim_value");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_asp_net_user_claims");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_asp_net_user_claims_user_id");

                    b.ToTable("asp_net_user_claims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text")
                        .HasColumnName("login_provider");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text")
                        .HasColumnName("provider_key");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text")
                        .HasColumnName("provider_display_name");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.HasKey("LoginProvider", "ProviderKey")
                        .HasName("pk_asp_net_user_logins");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_asp_net_user_logins_user_id");

                    b.ToTable("asp_net_user_logins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.Property<string>("RoleId")
                        .HasColumnType("text")
                        .HasColumnName("role_id");

                    b.HasKey("UserId", "RoleId")
                        .HasName("pk_asp_net_user_roles");

                    b.HasIndex("RoleId")
                        .HasDatabaseName("ix_asp_net_user_roles_role_id");

                    b.ToTable("asp_net_user_roles", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9",
                            RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text")
                        .HasColumnName("login_provider");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Value")
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.HasKey("UserId", "LoginProvider", "Name")
                        .HasName("pk_asp_net_user_tokens");

                    b.ToTable("asp_net_user_tokens", (string)null);
                });

            modelBuilder.Entity("LearnProject.Domain.Entities.Car", b =>
                {
                    b.HasOne("LearnProject.Domain.Entities.CarModel", "CarModel")
                        .WithMany()
                        .HasForeignKey("CarModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_cars_car_brand_models_car_model_id");

                    b.Navigation("CarModel");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_asp_net_role_claims_asp_net_roles_role_id");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("LearnProject.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_asp_net_user_claims_users_user_id");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("LearnProject.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_asp_net_user_logins_users_user_id");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_asp_net_user_roles_asp_net_roles_role_id");

                    b.HasOne("LearnProject.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_asp_net_user_roles_users_user_id");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("LearnProject.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_asp_net_user_tokens_users_user_id");
                });
#pragma warning restore 612, 618
        }
    }
}