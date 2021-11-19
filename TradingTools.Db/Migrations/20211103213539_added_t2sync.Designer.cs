﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TradingTools.Db;

namespace TradingTools.Db.Migrations
{
    [DbContext(typeof(TradingToolsDbContext))]
    [Migration("20211103213539_added_t2sync")]
    partial class added_t2sync
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TradingTools.Db.Entities.T2OrderEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("T2OrderEntity_ID")
                        .HasComment("PK, Identity")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal?>("AverageFillPrice")
                        .HasPrecision(18, 8)
                        .HasColumnType("decimal(18,8)");

                    b.Property<string>("ClientOrderId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset(2)")
                        .HasComment("Represents UTC date time of entity creation");

                    b.Property<bool?>("IsWorking")
                        .HasColumnType("bit");

                    b.Property<long>("OrderId")
                        .HasColumnType("bigint");

                    b.Property<long>("OrderListId")
                        .HasColumnType("bigint");

                    b.Property<string>("OriginalClientOrderId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasPrecision(18, 8)
                        .HasColumnType("decimal(18,8)");

                    b.Property<decimal>("Quantity")
                        .HasPrecision(18, 8)
                        .HasColumnType("decimal(18,8)");

                    b.Property<decimal>("QuantityFilled")
                        .HasPrecision(18, 8)
                        .HasColumnType("decimal(18,8)");

                    b.Property<decimal>("QuantityRemaining")
                        .HasPrecision(18, 8)
                        .HasColumnType("decimal(18,8)");

                    b.Property<decimal>("QuoteQuantity")
                        .HasPrecision(18, 8)
                        .HasColumnType("decimal(18,8)");

                    b.Property<decimal>("QuoteQuantityFilled")
                        .HasPrecision(18, 8)
                        .HasColumnType("decimal(18,8)");

                    b.Property<int>("Side")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<decimal?>("StopPrice")
                        .HasPrecision(18, 8)
                        .HasColumnType("decimal(18,8)");

                    b.Property<string>("Symbol")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("T2SymbolInfoId")
                        .HasColumnType("bigint");

                    b.Property<int>("TimeInForce")
                        .HasColumnType("int");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTimeOffset>("Updated")
                        .HasColumnType("datetimeoffset(2)")
                        .HasComment("Represents UTC date time of latest entity version");

                    b.HasKey("Id");

                    b.HasIndex("T2SymbolInfoId");

                    b.ToTable("T2Orders");
                });

            modelBuilder.Entity("TradingTools.Db.Entities.T2SymbolInfoEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("T2SymbolInfoEntity_ID")
                        .HasComment("PK, Identity")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BaseAsset")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("BaseAssetCommissionPrecision")
                        .HasColumnType("int");

                    b.Property<int>("BaseAssetPrecision")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset(2)")
                        .HasComment("Represents UTC date time of entity creation");

                    b.Property<int>("ExchangeType")
                        .HasColumnType("int");

                    b.Property<string>("QuoteAsset")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QuoteAssetCommissionPrecision")
                        .HasColumnType("int");

                    b.Property<int>("QuoteAssetPrecision")
                        .HasColumnType("int");

                    b.Property<string>("Symbol")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<DateTimeOffset>("Updated")
                        .HasColumnType("datetimeoffset(2)")
                        .HasComment("Represents UTC date time of latest entity version");

                    b.HasKey("Id");

                    b.ToTable("T2SymbolInfos");
                });

            modelBuilder.Entity("TradingTools.Db.Entities.T2SyncEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("T2SyncEntity_ID")
                        .HasComment("PK, Identity")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset(2)")
                        .HasComment("Represents UTC date time of entity creation");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<DateTimeOffset>("Updated")
                        .HasColumnType("datetimeoffset(2)")
                        .HasComment("Represents UTC date time of latest entity version");

                    b.HasKey("Id");

                    b.ToTable("T2Syncs");
                });

            modelBuilder.Entity("TradingTools.Db.Entities.T2TradeEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("T2TradeEntity_ID")
                        .HasComment("PK, Identity")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Commission")
                        .HasPrecision(18, 8)
                        .HasColumnType("decimal(18,8)");

                    b.Property<string>("CommissionAsset")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset(2)")
                        .HasComment("Represents UTC date time of entity creation");

                    b.Property<int>("ExchangeType")
                        .HasColumnType("int");

                    b.Property<bool>("IsBestMatch")
                        .HasColumnType("bit");

                    b.Property<bool>("IsBuyer")
                        .HasColumnType("bit");

                    b.Property<bool>("IsMaker")
                        .HasColumnType("bit");

                    b.Property<long>("OrderId")
                        .HasColumnType("bigint");

                    b.Property<decimal>("Price")
                        .HasPrecision(18, 8)
                        .HasColumnType("decimal(18,8)");

                    b.Property<decimal>("Quantity")
                        .HasPrecision(18, 8)
                        .HasColumnType("decimal(18,8)");

                    b.Property<decimal>("QuoteQuantity")
                        .HasPrecision(18, 8)
                        .HasColumnType("decimal(18,8)");

                    b.Property<decimal>("QuoteUsdValue")
                        .HasPrecision(18, 8)
                        .HasColumnType("decimal(18,8)");

                    b.Property<string>("Symbol")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("T2OrderId")
                        .HasColumnType("bigint");

                    b.Property<long?>("T2SymbolInfoId")
                        .HasColumnType("bigint");

                    b.Property<long?>("T2TradeGroupId")
                        .HasColumnType("bigint");

                    b.Property<long>("T2TradeId")
                        .HasColumnType("bigint");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<long>("TradeId")
                        .HasColumnType("bigint");

                    b.Property<int>("TradeState")
                        .HasColumnType("int");

                    b.Property<DateTime>("TradeTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTimeOffset>("Updated")
                        .HasColumnType("datetimeoffset(2)")
                        .HasComment("Represents UTC date time of latest entity version");

                    b.HasKey("Id");

                    b.HasIndex("T2OrderId");

                    b.HasIndex("T2SymbolInfoId");

                    b.HasIndex("T2TradeGroupId");

                    b.HasIndex("TradeId")
                        .IsUnique();

                    b.ToTable("T2Trades");
                });

            modelBuilder.Entity("TradingTools.Db.Entities.T2TradeGroupEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("T2TradeGroupEntity_ID")
                        .HasComment("PK, Identity")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BaseAsset")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset(2)")
                        .HasComment("Represents UTC date time of entity creation");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("SymbolInfoId")
                        .HasColumnType("bigint");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<int>("TradeGroupState")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("Updated")
                        .HasColumnType("datetimeoffset(2)")
                        .HasComment("Represents UTC date time of latest entity version");

                    b.HasKey("Id");

                    b.HasIndex("SymbolInfoId");

                    b.ToTable("T2TradeGroups");
                });

            modelBuilder.Entity("TradingTools.Db.Entities.T2OrderEntity", b =>
                {
                    b.HasOne("TradingTools.Db.Entities.T2SymbolInfoEntity", "T2SymbolInfo")
                        .WithMany()
                        .HasForeignKey("T2SymbolInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("T2SymbolInfo");
                });

            modelBuilder.Entity("TradingTools.Db.Entities.T2TradeEntity", b =>
                {
                    b.HasOne("TradingTools.Db.Entities.T2OrderEntity", "T2Order")
                        .WithMany()
                        .HasForeignKey("T2OrderId");

                    b.HasOne("TradingTools.Db.Entities.T2SymbolInfoEntity", "T2SymbolInfo")
                        .WithMany()
                        .HasForeignKey("T2SymbolInfoId");

                    b.HasOne("TradingTools.Db.Entities.T2TradeGroupEntity", "T2TradeGroup")
                        .WithMany("Trades")
                        .HasForeignKey("T2TradeGroupId");

                    b.Navigation("T2Order");

                    b.Navigation("T2SymbolInfo");

                    b.Navigation("T2TradeGroup");
                });

            modelBuilder.Entity("TradingTools.Db.Entities.T2TradeGroupEntity", b =>
                {
                    b.HasOne("TradingTools.Db.Entities.T2SymbolInfoEntity", "SymbolInfo")
                        .WithMany()
                        .HasForeignKey("SymbolInfoId");

                    b.Navigation("SymbolInfo");
                });

            modelBuilder.Entity("TradingTools.Db.Entities.T2TradeGroupEntity", b =>
                {
                    b.Navigation("Trades");
                });
#pragma warning restore 612, 618
        }
    }
}
