﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WithMovies.Business;

#nullable disable

namespace WithMovies.Business.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230629135925_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true);

            modelBuilder.Entity("WithMovies.Domain.Models.CastMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CastId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Character")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("CreditsId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Gender")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Order")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ProfilePath")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CreditsId");

                    b.ToTable("CastMembers");
                });

            modelBuilder.Entity("WithMovies.Domain.Models.Credits", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("MovieId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("MovieId");

                    b.ToTable("Credits");
                });

            modelBuilder.Entity("WithMovies.Domain.Models.CrewMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CreditsId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Department")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Gender")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Job")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ProfilePath")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CreditsId");

                    b.ToTable("CrewMembers");
                });

            modelBuilder.Entity("WithMovies.Domain.Models.Keyword", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Keywords");
                });

            modelBuilder.Entity("WithMovies.Domain.Models.Movie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Adult")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BelongsToCollectionId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Budget")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CastMemberId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CrewMemberId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Genres")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("HomePage")
                        .HasColumnType("TEXT");

                    b.Property<string>("ImdbId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("KeywordId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("OriginalLanguage")
                        .HasColumnType("TEXT");

                    b.Property<string>("OriginalTitle")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Overview")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("Popularity")
                        .HasColumnType("REAL");

                    b.Property<string>("PosterPath")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProductionCountries")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ReleaseDate")
                        .HasColumnType("TEXT");

                    b.Property<ulong>("Revenue")
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan?>("Runtime")
                        .HasColumnType("TEXT");

                    b.Property<string>("SpokenLanguages")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Tagline")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("VoteAverage")
                        .HasColumnType("REAL");

                    b.Property<int>("VoteCount")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("BelongsToCollectionId");

                    b.HasIndex("CastMemberId");

                    b.HasIndex("CrewMemberId");

                    b.HasIndex("KeywordId");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("WithMovies.Domain.Models.MovieCollection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BackdropPath")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PosterPath")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("MovieCollections");
                });

            modelBuilder.Entity("WithMovies.Domain.Models.ProductionCompany", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("MovieId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MovieId");

                    b.ToTable("ProductionCompanies");
                });

            modelBuilder.Entity("WithMovies.Domain.Models.RecommendationProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("RecommendationProfiles");
                });

            modelBuilder.Entity("WithMovies.Domain.Models.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Message")
                        .HasColumnType("TEXT");

                    b.Property<int>("MovieId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("PostedTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("Rating")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("WithMovies.Domain.Models.CastMember", b =>
                {
                    b.HasOne("WithMovies.Domain.Models.Credits", "Credits")
                        .WithMany("Cast")
                        .HasForeignKey("CreditsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Credits");
                });

            modelBuilder.Entity("WithMovies.Domain.Models.Credits", b =>
                {
                    b.HasOne("WithMovies.Domain.Models.Movie", "Movie")
                        .WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("WithMovies.Domain.Models.CrewMember", b =>
                {
                    b.HasOne("WithMovies.Domain.Models.Credits", "Credits")
                        .WithMany("Crew")
                        .HasForeignKey("CreditsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Credits");
                });

            modelBuilder.Entity("WithMovies.Domain.Models.Movie", b =>
                {
                    b.HasOne("WithMovies.Domain.Models.MovieCollection", "BelongsToCollection")
                        .WithMany("Movies")
                        .HasForeignKey("BelongsToCollectionId");

                    b.HasOne("WithMovies.Domain.Models.CastMember", null)
                        .WithMany("Movies")
                        .HasForeignKey("CastMemberId");

                    b.HasOne("WithMovies.Domain.Models.CrewMember", null)
                        .WithMany("Movies")
                        .HasForeignKey("CrewMemberId");

                    b.HasOne("WithMovies.Domain.Models.Keyword", null)
                        .WithMany("Movies")
                        .HasForeignKey("KeywordId");

                    b.Navigation("BelongsToCollection");
                });

            modelBuilder.Entity("WithMovies.Domain.Models.ProductionCompany", b =>
                {
                    b.HasOne("WithMovies.Domain.Models.Movie", null)
                        .WithMany("ProductionCompanies")
                        .HasForeignKey("MovieId");
                });

            modelBuilder.Entity("WithMovies.Domain.Models.CastMember", b =>
                {
                    b.Navigation("Movies");
                });

            modelBuilder.Entity("WithMovies.Domain.Models.Credits", b =>
                {
                    b.Navigation("Cast");

                    b.Navigation("Crew");
                });

            modelBuilder.Entity("WithMovies.Domain.Models.CrewMember", b =>
                {
                    b.Navigation("Movies");
                });

            modelBuilder.Entity("WithMovies.Domain.Models.Keyword", b =>
                {
                    b.Navigation("Movies");
                });

            modelBuilder.Entity("WithMovies.Domain.Models.Movie", b =>
                {
                    b.Navigation("ProductionCompanies");
                });

            modelBuilder.Entity("WithMovies.Domain.Models.MovieCollection", b =>
                {
                    b.Navigation("Movies");
                });
#pragma warning restore 612, 618
        }
    }
}
