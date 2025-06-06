﻿// <auto-generated />
using System;
using JawwedAPI.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace JawwedAPI.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250228090057_Mofasir_TafsirModels")]
    partial class Mofasir_TafsirModels
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("JawwedAPI.Core.Domain.Entities.Bookmark", b =>
                {
                    b.Property<int>("BookmarkId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookmarkId"));

                    b.Property<string>("Page")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Verse")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VerseKey")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BookmarkId");

                    b.ToTable("Bookmarks");
                });

            modelBuilder.Entity("JawwedAPI.Core.Domain.Entities.Chapter", b =>
                {
                    b.Property<int>("ChapterID")
                        .HasColumnType("int");

                    b.Property<bool>("BismallahPre")
                        .HasColumnType("bit");

                    b.Property<string>("NameArabic")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameComplex")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameEnglish")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Pages")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RevelationPlace")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VersesCount")
                        .HasColumnType("int");

                    b.HasKey("ChapterID");

                    b.ToTable("Chapters");
                });

            modelBuilder.Entity("JawwedAPI.Core.Domain.Entities.Line", b =>
                {
                    b.Property<int>("LineID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LineID"));

                    b.Property<int?>("HizbNumber")
                        .HasColumnType("int");

                    b.Property<bool>("IsCentered")
                        .HasColumnType("bit");

                    b.Property<int?>("JuzNumber")
                        .HasColumnType("int");

                    b.Property<int>("LineNumber")
                        .HasColumnType("int");

                    b.Property<string>("LineType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PageNumber")
                        .HasColumnType("int");

                    b.Property<int?>("RubHizbNumber")
                        .HasColumnType("int");

                    b.Property<int>("SurahNumber")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VersesKeys")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LineID");

                    b.ToTable("Lines");
                });

            modelBuilder.Entity("JawwedAPI.Core.Domain.Entities.Mofasir", b =>
                {
                    b.Property<int>("MofasirID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MofasirID"));

                    b.Property<string>("AuthorName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BookName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Languages")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MofasirID");

                    b.ToTable("Mofasirs");
                });

            modelBuilder.Entity("JawwedAPI.Core.Domain.Entities.Tafsir", b =>
                {
                    b.Property<int>("TafsirID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TafsirID"));

                    b.Property<string>("ChapterVerseID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MofasirID")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TafsirID");

                    b.HasIndex("MofasirID");

                    b.ToTable("Tafsirs");
                });

            modelBuilder.Entity("JawwedAPI.Core.Domain.Entities.Verse", b =>
                {
                    b.Property<int>("VerseID")
                        .HasColumnType("int");

                    b.Property<int>("ChapterID")
                        .HasColumnType("int");

                    b.Property<int>("HizbNumber")
                        .HasColumnType("int");

                    b.Property<int>("JuzNumber")
                        .HasColumnType("int");

                    b.Property<int>("Page")
                        .HasColumnType("int");

                    b.Property<bool?>("Sajdah")
                        .IsRequired()
                        .HasColumnType("bit");

                    b.Property<string>("TextUthmani")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VerseNumber")
                        .HasColumnType("int");

                    b.HasKey("VerseID");

                    b.HasIndex("ChapterID");

                    b.ToTable("Verses");
                });

            modelBuilder.Entity("JawwedAPI.Core.Domain.Entities.Tafsir", b =>
                {
                    b.HasOne("JawwedAPI.Core.Domain.Entities.Mofasir", "Mofasir")
                        .WithMany("Tafsirs")
                        .HasForeignKey("MofasirID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Mofasir");
                });

            modelBuilder.Entity("JawwedAPI.Core.Domain.Entities.Verse", b =>
                {
                    b.HasOne("JawwedAPI.Core.Domain.Entities.Chapter", "Chapter")
                        .WithMany("Verses")
                        .HasForeignKey("ChapterID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chapter");
                });

            modelBuilder.Entity("JawwedAPI.Core.Domain.Entities.Chapter", b =>
                {
                    b.Navigation("Verses");
                });

            modelBuilder.Entity("JawwedAPI.Core.Domain.Entities.Mofasir", b =>
                {
                    b.Navigation("Tafsirs");
                });
#pragma warning restore 612, 618
        }
    }
}
