using Microsoft.EntityFrameworkCore;
using ServiceAutomation.DataAccess.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAutomation.DataAccess.DbContexts
{
    public abstract class ServiceDbContext : DbContext
    {
        public virtual DbSet<UserEntity> Users { get; set; }
        public virtual DbSet<RefreshTokenEntity> RefresTokens { get; set; }
        public virtual DbSet<ThumbnailTemplateEntity> Thumbnails { get; set; }
        public virtual DbSet<CredentialEntity> Credentials { get; set; }
        public virtual DbSet<WithdrawTransactionEntity> WithdrawTransactions { get; set; }
        public virtual DbSet<UserContactEntity> UserContacts { get; set; }
        public virtual DbSet<TenantGroupEntity> TenantGroups { get; set; }
        public virtual DbSet<VideoLessonTemplateEntity> VideoLessons { get; set; }
        public virtual DbSet<ProfilePhotoEntity> ProfilePhotos { get; set; }

        public ServiceDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
