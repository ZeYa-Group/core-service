﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ServiceAutomation.DataAccess.DbContexts;

namespace ServiceAutomation.DataAccess.Migrations.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20220911210344_AddUsersPurchases")]
    partial class AddUsersPurchases
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("ServiceAutomation.DataAccess.Models.EntityModels.BonusEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Bonuses");
                });

            modelBuilder.Entity("ServiceAutomation.DataAccess.Models.EntityModels.CredentialEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<string>("IBAN")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Credentials");
                });

            modelBuilder.Entity("ServiceAutomation.DataAccess.Models.EntityModels.PackageBonusAssociationEntity", b =>
                {
                    b.Property<Guid>("PackageId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("BonusId")
                        .HasColumnType("uuid");

                    b.Property<int>("FromLevel")
                        .HasColumnType("integer");

                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<int?>("PayablePercent")
                        .HasColumnType("integer");

                    b.HasKey("PackageId", "BonusId");

                    b.HasIndex("BonusId");

                    b.ToTable("Package:Bonuse");
                });

            modelBuilder.Entity("ServiceAutomation.DataAccess.Models.EntityModels.PackageEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Packages");
                });

            modelBuilder.Entity("ServiceAutomation.DataAccess.Models.EntityModels.ProfilePhotoEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<byte[]>("Data")
                        .HasColumnType("bytea");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("ProfilePhotos");
                });

            modelBuilder.Entity("ServiceAutomation.DataAccess.Models.EntityModels.PurchaseEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<Guid>("PackageId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("PurchaseDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PackageId");

                    b.HasIndex("UserId");

                    b.ToTable("Purchases");
                });

            modelBuilder.Entity("ServiceAutomation.DataAccess.Models.EntityModels.RefreshTokenEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<string>("Token")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("ServiceAutomation.DataAccess.Models.EntityModels.TenantGroupEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<Guid>("OwnerUserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ParentId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("OwnerUserId")
                        .IsUnique();

                    b.HasIndex("ParentId");

                    b.ToTable("TenantGroups");
                });

            modelBuilder.Entity("ServiceAutomation.DataAccess.Models.EntityModels.ThumbnailTemplateEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ThumbnailFullPath")
                        .HasColumnType("text");

                    b.Property<string>("ThumbnailName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Thumbnails");
                });

            modelBuilder.Entity("ServiceAutomation.DataAccess.Models.EntityModels.UserContactEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<string>("Adress")
                        .HasColumnType("text");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("IdentityCode")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("PassportNumber")
                        .HasColumnType("text");

                    b.Property<string>("PassportSeries")
                        .HasColumnType("text");

                    b.Property<string>("Patronymic")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UserContacts");
                });

            modelBuilder.Entity("ServiceAutomation.DataAccess.Models.EntityModels.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Country")
                        .HasColumnType("integer");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("InviteReferral")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("bytea");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("bytea");

                    b.Property<string>("PersonalReferral")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ServiceAutomation.DataAccess.Models.EntityModels.VideoLessonTemplateEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("VideoFullPath")
                        .HasColumnType("text");

                    b.Property<string>("VideoName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("VideoLessons");
                });

            modelBuilder.Entity("ServiceAutomation.DataAccess.Models.EntityModels.WithdrawTransactionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<Guid>("CredentialId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("TransactionStatus")
                        .HasColumnType("integer");

                    b.Property<decimal>("Value")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("CredentialId");

                    b.ToTable("WithdrawTransactions");
                });

            modelBuilder.Entity("ServiceAutomation.DataAccess.Models.EntityModels.CredentialEntity", b =>
                {
                    b.HasOne("ServiceAutomation.DataAccess.Models.EntityModels.UserEntity", "User")
                        .WithMany("Credentionals")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ServiceAutomation.DataAccess.Models.EntityModels.PackageBonusAssociationEntity", b =>
                {
                    b.HasOne("ServiceAutomation.DataAccess.Models.EntityModels.BonusEntity", "Bonus")
                        .WithMany("PackageBonuses")
                        .HasForeignKey("BonusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServiceAutomation.DataAccess.Models.EntityModels.PackageEntity", "Package")
                        .WithMany("PackageBonuses")
                        .HasForeignKey("PackageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bonus");

                    b.Navigation("Package");
                });

            modelBuilder.Entity("ServiceAutomation.DataAccess.Models.EntityModels.ProfilePhotoEntity", b =>
                {
                    b.HasOne("ServiceAutomation.DataAccess.Models.EntityModels.UserEntity", "User")
                        .WithOne("ProfilePhoto")
                        .HasForeignKey("ServiceAutomation.DataAccess.Models.EntityModels.ProfilePhotoEntity", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ServiceAutomation.DataAccess.Models.EntityModels.PurchaseEntity", b =>
                {
                    b.HasOne("ServiceAutomation.DataAccess.Models.EntityModels.PackageEntity", "Package")
                        .WithMany("UsersPurchases")
                        .HasForeignKey("PackageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServiceAutomation.DataAccess.Models.EntityModels.UserEntity", "User")
                        .WithMany("UserPurchases")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Package");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ServiceAutomation.DataAccess.Models.EntityModels.TenantGroupEntity", b =>
                {
                    b.HasOne("ServiceAutomation.DataAccess.Models.EntityModels.UserEntity", "OwnerUser")
                        .WithOne("Group")
                        .HasForeignKey("ServiceAutomation.DataAccess.Models.EntityModels.TenantGroupEntity", "OwnerUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServiceAutomation.DataAccess.Models.EntityModels.TenantGroupEntity", "Parent")
                        .WithMany("ChildGroups")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OwnerUser");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("ServiceAutomation.DataAccess.Models.EntityModels.UserContactEntity", b =>
                {
                    b.HasOne("ServiceAutomation.DataAccess.Models.EntityModels.UserEntity", "User")
                        .WithOne("UserContact")
                        .HasForeignKey("ServiceAutomation.DataAccess.Models.EntityModels.UserContactEntity", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ServiceAutomation.DataAccess.Models.EntityModels.WithdrawTransactionEntity", b =>
                {
                    b.HasOne("ServiceAutomation.DataAccess.Models.EntityModels.CredentialEntity", "Credential")
                        .WithMany("WithdrawTransactions")
                        .HasForeignKey("CredentialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Credential");
                });

            modelBuilder.Entity("ServiceAutomation.DataAccess.Models.EntityModels.BonusEntity", b =>
                {
                    b.Navigation("PackageBonuses");
                });

            modelBuilder.Entity("ServiceAutomation.DataAccess.Models.EntityModels.CredentialEntity", b =>
                {
                    b.Navigation("WithdrawTransactions");
                });

            modelBuilder.Entity("ServiceAutomation.DataAccess.Models.EntityModels.PackageEntity", b =>
                {
                    b.Navigation("PackageBonuses");

                    b.Navigation("UsersPurchases");
                });

            modelBuilder.Entity("ServiceAutomation.DataAccess.Models.EntityModels.TenantGroupEntity", b =>
                {
                    b.Navigation("ChildGroups");
                });

            modelBuilder.Entity("ServiceAutomation.DataAccess.Models.EntityModels.UserEntity", b =>
                {
                    b.Navigation("Credentionals");

                    b.Navigation("Group");

                    b.Navigation("ProfilePhoto");

                    b.Navigation("UserContact");

                    b.Navigation("UserPurchases");
                });
#pragma warning restore 612, 618
        }
    }
}
