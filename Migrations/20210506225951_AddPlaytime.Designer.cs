﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TamagotchiAPI.Models;

namespace TamagotchiAPI.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20210506225951_AddPlaytime")]
    partial class AddPlaytime
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("TamagotchiAPI.Models.Pet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("HappinessLevel")
                        .HasColumnType("integer");

                    b.Property<int>("HungerLevel")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Pets");
                });

            modelBuilder.Entity("TamagotchiAPI.Models.Playtime", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<int>("PetId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("When")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("PetId");

                    b.ToTable("Playtimes");
                });

            modelBuilder.Entity("TamagotchiAPI.Models.Playtime", b =>
                {
                    b.HasOne("TamagotchiAPI.Models.Pet", "Pet")
                        .WithMany()
                        .HasForeignKey("PetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pet");
                });
#pragma warning restore 612, 618
        }
    }
}
