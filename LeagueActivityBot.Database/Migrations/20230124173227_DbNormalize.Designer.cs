﻿// <auto-generated />
using System;
using LeagueActivityBot.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace LeagueActivityBot.Database.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230124173227_DbNormalize")]
    partial class DbNormalize
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.24")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("LeagueActivityBot.Entities.GameInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long?>("GameDurationInSeconds")
                        .HasColumnType("bigint");

                    b.Property<bool>("GameEnded")
                        .HasColumnType("boolean");

                    b.Property<long>("GameId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("GameStartTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsProcessed")
                        .HasColumnType("boolean");

                    b.Property<long>("QueueId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("GameInfos");
                });

            modelBuilder.Entity("LeagueActivityBot.Entities.GameParticipant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("Assists")
                        .HasColumnType("integer");

                    b.Property<int?>("ChampionId")
                        .HasColumnType("integer");

                    b.Property<string>("ChampionName")
                        .HasColumnType("text");

                    b.Property<int?>("CreepScore")
                        .HasColumnType("integer");

                    b.Property<int?>("Deaths")
                        .HasColumnType("integer");

                    b.Property<int?>("DetectorWardsPlaced")
                        .HasColumnType("integer");

                    b.Property<bool?>("FirstBloodKill")
                        .HasColumnType("boolean");

                    b.Property<int>("GameInfoId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<int?>("Kills")
                        .HasColumnType("integer");

                    b.Property<int?>("PentaKills")
                        .HasColumnType("integer");

                    b.Property<int>("SummonerId")
                        .HasColumnType("integer");

                    b.Property<int?>("VisionScore")
                        .HasColumnType("integer");

                    b.Property<bool?>("Win")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("GameInfoId");

                    b.HasIndex("SummonerId");

                    b.ToTable("GameParticipants");
                });

            modelBuilder.Entity("LeagueActivityBot.Entities.LeagueInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<int>("LeaguePoints")
                        .HasColumnType("integer");

                    b.Property<int>("LeagueType")
                        .HasColumnType("integer");

                    b.Property<int>("Rank")
                        .HasColumnType("integer");

                    b.Property<int>("SummonerId")
                        .HasColumnType("integer");

                    b.Property<int>("Tier")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SummonerId");

                    b.ToTable("LeagueInfos");
                });

            modelBuilder.Entity("LeagueActivityBot.Entities.Summoner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AccountId")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Puuid")
                        .HasColumnType("text");

                    b.Property<string>("SummonerId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Summoners");
                });

            modelBuilder.Entity("LeagueActivityBot.Entities.GameParticipant", b =>
                {
                    b.HasOne("LeagueActivityBot.Entities.GameInfo", "GameInfo")
                        .WithMany("GameParticipants")
                        .HasForeignKey("GameInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LeagueActivityBot.Entities.Summoner", "Summoner")
                        .WithMany("GameParticipants")
                        .HasForeignKey("SummonerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LeagueActivityBot.Entities.LeagueInfo", b =>
                {
                    b.HasOne("LeagueActivityBot.Entities.Summoner", "Summoner")
                        .WithMany("LeagueInfos")
                        .HasForeignKey("SummonerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}