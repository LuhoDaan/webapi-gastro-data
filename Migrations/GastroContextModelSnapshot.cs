﻿// <auto-generated />
using System;
using GastroApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GastroApi.Migrations
{
    [DbContext(typeof(GastroContext))]
    partial class GastroContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GastroApi.GastroItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("DescriptionName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Ingredients")
                        .HasColumnType("text");

                    b.Property<string>("Recipe")
                        .HasColumnType("text");

                    b.Property<int?>("TimeToPrepare")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("GastroItems");
                });
#pragma warning restore 612, 618
        }
    }
}
