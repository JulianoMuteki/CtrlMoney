﻿// <auto-generated />
using System;
using CtrlMoney.Infra.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CtrlMoney.Infra.Context.Migrations
{
    [DbContext(typeof(CtrlMoneyContext))]
    [Migration("20221015150349_Change-Earnings-To-StatusInvest")]
    partial class ChangeEarningsToStatusInvest
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CtrlMoney.Domain.Entities.Bank", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("BankID");

                    b.Property<decimal>("BankBalance")
                        .HasColumnType("numeric(10,2)");

                    b.Property<int>("BankCode")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("InitialBalance")
                        .HasColumnType("numeric(10,2)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDisable")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.HasKey("Id")
                        .HasName("BankID");

                    b.ToTable("Banks", (string)null);
                });

            modelBuilder.Entity("CtrlMoney.Domain.Entities.BrokerageHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("BrokerageHistoryID");

                    b.Property<decimal>("Brokerage")
                        .HasColumnType("numeric");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ExpireDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("Fees")
                        .HasColumnType("numeric");

                    b.Property<decimal>("IRRF")
                        .HasColumnType("numeric");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDisable")
                        .HasColumnType("boolean");

                    b.Property<string>("Market")
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric(10,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<string>("StockBroker")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<decimal>("Taxes")
                        .HasColumnType("numeric");

                    b.Property<string>("TicketCode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("numeric(10,2)");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("TransactionType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.HasKey("Id")
                        .HasName("BrokerageHistoryID");

                    b.ToTable("BrokeragesHistories", (string)null);
                });

            modelBuilder.Entity("CtrlMoney.Domain.Entities.Earning", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("EarningID");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("EventType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDisable")
                        .HasColumnType("boolean");

                    b.Property<string>("PaymentDate")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric(10,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<string>("StockBroker")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<string>("TicketCode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<decimal>("TotalNetAmount")
                        .HasColumnType("numeric(10,2)");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("numeric(10,2)");

                    b.Property<DateTime>("WithDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id")
                        .HasName("EarningID");

                    b.ToTable("Earnings", (string)null);
                });

            modelBuilder.Entity("CtrlMoney.Domain.Entities.FinancialClassification.ChildTree", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("ChildTreeID");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDisable")
                        .HasColumnType("boolean");

                    b.Property<int>("LevelTree")
                        .HasColumnType("integer");

                    b.Property<Guid?>("ParentNodeID")
                        .HasColumnType("uuid");

                    b.Property<string>("Tag")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.HasKey("Id")
                        .HasName("ChildTreeID");

                    b.HasIndex("ParentNodeID");

                    b.ToTable("ChildrenTrees", (string)null);
                });

            modelBuilder.Entity("CtrlMoney.Domain.Entities.FinancialClassification.Composite", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("CompositeID");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDisable")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("LeafParentID")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.HasKey("Id")
                        .HasName("CompositeID");

                    b.HasIndex("LeafParentID");

                    b.ToTable("Composites", (string)null);
                });

            modelBuilder.Entity("CtrlMoney.Domain.Entities.FinancialClassification.GrandChildTree", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("GrandChildTreeID");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDisable")
                        .HasColumnType("boolean");

                    b.Property<int>("LevelTree")
                        .HasColumnType("integer");

                    b.Property<Guid?>("ParentNodeID")
                        .HasColumnType("uuid");

                    b.Property<string>("Tag")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.HasKey("Id")
                        .HasName("GrandChildTreeID");

                    b.HasIndex("ParentNodeID");

                    b.ToTable("GrandChildrenTrees", (string)null);
                });

            modelBuilder.Entity("CtrlMoney.Domain.Entities.FinancialClassification.Leaf", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("LeafID");

                    b.Property<Guid>("CompositeID")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDisable")
                        .HasColumnType("boolean");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.HasKey("Id")
                        .HasName("LeafID");

                    b.HasIndex("CompositeID");

                    b.ToTable("Leaves", (string)null);
                });

            modelBuilder.Entity("CtrlMoney.Domain.Entities.FinancialClassification.ParentTree", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("ParentTreeID");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDisable")
                        .HasColumnType("boolean");

                    b.Property<int>("LevelTree")
                        .HasColumnType("integer");

                    b.Property<Guid?>("ParentNodeID")
                        .HasColumnType("uuid");

                    b.Property<string>("Tag")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.HasKey("Id")
                        .HasName("ParentTreeID");

                    b.HasIndex("ParentNodeID");

                    b.ToTable("ParentsTrees", (string)null);
                });

            modelBuilder.Entity("CtrlMoney.Domain.Entities.FinancialTransaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("FinancialTransactionID");

                    b.Property<Guid>("BankID")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ChildTreeID")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<int>("EPaymentMethod")
                        .HasColumnType("integer");

                    b.Property<int>("ETransactionType")
                        .HasColumnType("integer");

                    b.Property<Guid>("GrandChildTreeID")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDisable")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsOperationDone")
                        .HasColumnType("boolean");

                    b.Property<Guid>("ParentTreeID")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("PaymentDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("Value")
                        .HasColumnType("numeric(10,2)");

                    b.HasKey("Id")
                        .HasName("FinancialTransactionID");

                    b.HasIndex("BankID");

                    b.HasIndex("ChildTreeID");

                    b.HasIndex("GrandChildTreeID");

                    b.HasIndex("ParentTreeID");

                    b.ToTable("FinancialTransactions", (string)null);
                });

            modelBuilder.Entity("CtrlMoney.Domain.Entities.Moviment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("MovimentsID");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("InputOutput")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDisable")
                        .HasColumnType("boolean");

                    b.Property<string>("MovimentType")
                        .HasColumnType("text");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<string>("StockBroker")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<string>("TicketCode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<decimal>("TransactionValue")
                        .HasColumnType("numeric(10,2)");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("numeric(10,2)");

                    b.HasKey("Id")
                        .HasName("MovimentsID");

                    b.ToTable("Moviments", (string)null);
                });

            modelBuilder.Entity("CtrlMoney.Domain.Entities.Position", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("PositionID");

                    b.Property<string>("Bookkeeping")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<decimal>("ClosingPrice")
                        .HasColumnType("numeric(10,2)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("EInvestmentType")
                        .HasColumnType("integer");

                    b.Property<string>("ISINCode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDisable")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("PositionDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<int>("QuantityAvailable")
                        .HasColumnType("integer");

                    b.Property<int>("QuantityUnavailable")
                        .HasColumnType("integer");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<string>("StockBroker")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<string>("TicketCode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<decimal>("ValueUpdated")
                        .HasColumnType("numeric(10,2)");

                    b.HasKey("Id")
                        .HasName("PositionID");

                    b.ToTable("Positions", (string)null);
                });

            modelBuilder.Entity("CtrlMoney.Domain.Entities.TicketConversion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("TicketConversionID");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDisable")
                        .HasColumnType("boolean");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<string>("StockBroker")
                        .IsRequired()
                        .HasColumnType("varchar");

                    b.Property<string>("TicketInput")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("varchar");

                    b.Property<string>("TicketOutput")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("varchar");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("numeric(10,2)");

                    b.HasKey("Id")
                        .HasName("TicketConversionID");

                    b.ToTable("TicketsConversions", (string)null);
                });

            modelBuilder.Entity("CtrlMoney.Domain.Identity.ApplicationRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("CtrlMoney.Domain.Identity.ApplicationRoleClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("CtrlMoney.Domain.Identity.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("CtrlMoney.Domain.Identity.ApplicationUserClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("CtrlMoney.Domain.Identity.ApplicationUserLogin", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("CtrlMoney.Domain.Identity.ApplicationUserRole", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("CtrlMoney.Domain.Identity.ApplicationUserToken", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("CtrlMoney.Domain.Entities.FinancialClassification.ChildTree", b =>
                {
                    b.HasOne("CtrlMoney.Domain.Entities.FinancialClassification.ParentTree", "ParentNode")
                        .WithMany("Children")
                        .HasForeignKey("ParentNodeID");

                    b.Navigation("ParentNode");
                });

            modelBuilder.Entity("CtrlMoney.Domain.Entities.FinancialClassification.Composite", b =>
                {
                    b.HasOne("CtrlMoney.Domain.Entities.FinancialClassification.Leaf", "LeafParent")
                        .WithMany("CompositesChildren")
                        .HasForeignKey("LeafParentID");

                    b.Navigation("LeafParent");
                });

            modelBuilder.Entity("CtrlMoney.Domain.Entities.FinancialClassification.GrandChildTree", b =>
                {
                    b.HasOne("CtrlMoney.Domain.Entities.FinancialClassification.ChildTree", "ParentNode")
                        .WithMany("Children")
                        .HasForeignKey("ParentNodeID");

                    b.Navigation("ParentNode");
                });

            modelBuilder.Entity("CtrlMoney.Domain.Entities.FinancialClassification.Leaf", b =>
                {
                    b.HasOne("CtrlMoney.Domain.Entities.FinancialClassification.Composite", "Composite")
                        .WithMany("Leaves")
                        .HasForeignKey("CompositeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Composite");
                });

            modelBuilder.Entity("CtrlMoney.Domain.Entities.FinancialClassification.ParentTree", b =>
                {
                    b.HasOne("CtrlMoney.Domain.Entities.FinancialClassification.GrandChildTree", "ParentNode")
                        .WithMany("Children")
                        .HasForeignKey("ParentNodeID");

                    b.Navigation("ParentNode");
                });

            modelBuilder.Entity("CtrlMoney.Domain.Entities.FinancialTransaction", b =>
                {
                    b.HasOne("CtrlMoney.Domain.Entities.Bank", "Bank")
                        .WithMany("FinancialTransactions")
                        .HasForeignKey("BankID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CtrlMoney.Domain.Entities.FinancialClassification.ChildTree", "ChildTree")
                        .WithMany("FinancialTransactions")
                        .HasForeignKey("ChildTreeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CtrlMoney.Domain.Entities.FinancialClassification.GrandChildTree", "GrandChildTree")
                        .WithMany("FinancialTransactions")
                        .HasForeignKey("GrandChildTreeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CtrlMoney.Domain.Entities.FinancialClassification.ParentTree", "ParentTree")
                        .WithMany("FinancialTransactions")
                        .HasForeignKey("ParentTreeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bank");

                    b.Navigation("ChildTree");

                    b.Navigation("GrandChildTree");

                    b.Navigation("ParentTree");
                });

            modelBuilder.Entity("CtrlMoney.Domain.Identity.ApplicationRoleClaim", b =>
                {
                    b.HasOne("CtrlMoney.Domain.Identity.ApplicationRole", "Role")
                        .WithMany("RoleClaims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("CtrlMoney.Domain.Identity.ApplicationUserClaim", b =>
                {
                    b.HasOne("CtrlMoney.Domain.Identity.ApplicationUser", "User")
                        .WithMany("UserClaims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("CtrlMoney.Domain.Identity.ApplicationUserLogin", b =>
                {
                    b.HasOne("CtrlMoney.Domain.Identity.ApplicationUser", "User")
                        .WithMany("UserLogins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("CtrlMoney.Domain.Identity.ApplicationUserRole", b =>
                {
                    b.HasOne("CtrlMoney.Domain.Identity.ApplicationRole", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CtrlMoney.Domain.Identity.ApplicationUser", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CtrlMoney.Domain.Identity.ApplicationUserToken", b =>
                {
                    b.HasOne("CtrlMoney.Domain.Identity.ApplicationUser", "User")
                        .WithMany("UserTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("CtrlMoney.Domain.Entities.Bank", b =>
                {
                    b.Navigation("FinancialTransactions");
                });

            modelBuilder.Entity("CtrlMoney.Domain.Entities.FinancialClassification.ChildTree", b =>
                {
                    b.Navigation("Children");

                    b.Navigation("FinancialTransactions");
                });

            modelBuilder.Entity("CtrlMoney.Domain.Entities.FinancialClassification.Composite", b =>
                {
                    b.Navigation("Leaves");
                });

            modelBuilder.Entity("CtrlMoney.Domain.Entities.FinancialClassification.GrandChildTree", b =>
                {
                    b.Navigation("Children");

                    b.Navigation("FinancialTransactions");
                });

            modelBuilder.Entity("CtrlMoney.Domain.Entities.FinancialClassification.Leaf", b =>
                {
                    b.Navigation("CompositesChildren");
                });

            modelBuilder.Entity("CtrlMoney.Domain.Entities.FinancialClassification.ParentTree", b =>
                {
                    b.Navigation("Children");

                    b.Navigation("FinancialTransactions");
                });

            modelBuilder.Entity("CtrlMoney.Domain.Identity.ApplicationRole", b =>
                {
                    b.Navigation("RoleClaims");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("CtrlMoney.Domain.Identity.ApplicationUser", b =>
                {
                    b.Navigation("UserClaims");

                    b.Navigation("UserLogins");

                    b.Navigation("UserRoles");

                    b.Navigation("UserTokens");
                });
#pragma warning restore 612, 618
        }
    }
}
