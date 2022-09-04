﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ServiceAutomation.DataAccess.DbContexts;

namespace ServiceAutomation.DataAccess.Migrations.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

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

                    b.Property<string>("EmailAddress")
                        .HasColumnType("text");

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

            modelBuilder.Entity("ServiceAutomation.DataAccess.Models.EntityModels.ProfilePhotoEntity", b =>
                {
                    b.HasOne("ServiceAutomation.DataAccess.Models.EntityModels.UserEntity", "User")
                        .WithOne("ProfilePhoto")
                        .HasForeignKey("ServiceAutomation.DataAccess.Models.EntityModels.ProfilePhotoEntity", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

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

            modelBuilder.Entity("ServiceAutomation.DataAccess.Models.EntityModels.CredentialEntity", b =>
                {
                    b.Navigation("WithdrawTransactions");
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
                });
#pragma warning restore 612, 618
        }
    }
}
