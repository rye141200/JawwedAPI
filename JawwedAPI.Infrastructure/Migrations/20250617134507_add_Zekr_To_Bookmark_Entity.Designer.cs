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
    [Migration("20250617134507_add_Zekr_To_Bookmark_Entity")]
    partial class add_Zekr_To_Bookmark_Entity
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("JawwedAPI.Core.Domain.Entities.ApplicationUser", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DeviceToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("EnableNotifications")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<int>("UserRole")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("ApplicationUsers");

                    b.HasData(
                        new
                        {
                            UserId = new Guid("b4f2b556-8789-4db3-b4b1-e0222f49a8e6"),
                            DeviceToken = "",
                            Email = "thecityhunterhd@gmail.com",
                            EnableNotifications = false,
                            UserName = "Ahmad Mahfouz",
                            UserRole = 0
                        },
                        new
                        {
                            UserId = new Guid("18e83293-3400-447c-b38b-e7e9c62bf220"),
                            DeviceToken = "",
                            Email = "ahmad.mhfz1412@gmail.com",
                            EnableNotifications = false,
                            UserName = "Ahmad Mahfouz",
                            UserRole = 1
                        });
                });

            modelBuilder.Entity("JawwedAPI.Core.Domain.Entities.Bookmark", b =>
                {
                    b.Property<int>("BookmarkId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookmarkId"));

                    b.Property<int>("BookmarkType")
                        .HasColumnType("int");

                    b.Property<string>("Page")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Verse")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VerseKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ZekrID")
                        .HasColumnType("int");

                    b.HasKey("BookmarkId");

                    b.HasIndex("UserId");

                    b.HasIndex("ZekrID");

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

            modelBuilder.Entity("JawwedAPI.Core.Domain.Entities.Goal", b =>
                {
                    b.Property<Guid>("GoalId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ActualPagesRead")
                        .HasColumnType("int");

                    b.Property<int>("DurationDays")
                        .HasColumnType("int");

                    b.Property<string>("LastVerseKeyRead")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("ReminderTime")
                        .HasColumnType("time");

                    b.Property<DateTimeOffset>("StartDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("StartPage")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalPages")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("GoalId");

                    b.HasIndex("UserId");

                    b.ToTable("Goals");
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

                    b.Property<string>("AuthorNameArabic")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AuthorNameEnglish")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BiographyArabic")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BiographyEnglish")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BirthYear")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BookNameArabic")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BookNameEnglish")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeathYear")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("SupportsArabic")
                        .HasColumnType("bit");

                    b.Property<bool>("SupportsEnglish")
                        .HasColumnType("bit");

                    b.HasKey("MofasirID");

                    b.ToTable("Mofasirs");
                });

            modelBuilder.Entity("JawwedAPI.Core.Domain.Entities.Question", b =>
                {
                    b.Property<int>("QuestionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("QuestionID"));

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Difficulty")
                        .HasColumnType("int");

                    b.Property<string>("OptionA")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OptionB")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QuestionHeader")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("QuestionID");

                    b.ToTable("Questions");
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

                    b.Property<bool>("Sajdah")
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

            modelBuilder.Entity("JawwedAPI.Core.Domain.Entities.Zekr", b =>
                {
                    b.Property<int>("ZekrID")
                        .HasColumnType("int");

                    b.Property<string>("Audio")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CategoryAudio")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.HasKey("ZekrID");

                    b.ToTable("Azkar");
                });

            modelBuilder.Entity("JawwedAPI.Core.Domain.Entities.Bookmark", b =>
                {
                    b.HasOne("JawwedAPI.Core.Domain.Entities.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JawwedAPI.Core.Domain.Entities.Zekr", "Zekr")
                        .WithMany()
                        .HasForeignKey("ZekrID");

                    b.Navigation("ApplicationUser");

                    b.Navigation("Zekr");
                });

            modelBuilder.Entity("JawwedAPI.Core.Domain.Entities.Goal", b =>
                {
                    b.HasOne("JawwedAPI.Core.Domain.Entities.ApplicationUser", "User")
                        .WithMany("Goals")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
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

            modelBuilder.Entity("JawwedAPI.Core.Domain.Entities.ApplicationUser", b =>
                {
                    b.Navigation("Goals");
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
