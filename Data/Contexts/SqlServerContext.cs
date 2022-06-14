using Audit.Core;
using Data.Migrations;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Contexts
{
    public sealed class SqlServerContext : DbContext //AuditDbContext
    {
        private static IConfiguration _configuration;
        public SqlServerContext(DbContextOptions<SqlServerContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
            //DbInitializer.Initialize(this);
            //AuditEventType = "{database}_{context}";
            //Mode = AuditOptionMode.OptOut;
            //IncludeEntityObjects = false;
            //Audit.Core.Configuration.DataProvider = new AuditProvider();
        }
        public DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder
          .UseLazyLoadingProxies(true)
          .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.LazyLoadOnDisposedContextWarning))
          .UseSqlServer(($"Data Source=tcp:{_configuration[ConfigurationEnum.Server]}.database.windows.net,1433;Initial Catalog={_configuration[ConfigurationEnum.Bd]};User Id={_configuration[ConfigurationEnum.User]}@{_configuration[ConfigurationEnum.Server]};Password={_configuration[ConfigurationEnum.Password]}"));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Dependency
            modelBuilder.Entity<Dependency>().ToTable("Dependencies").HasKey(x => x.Id);
            modelBuilder.Entity<Dependency>(x =>
            {
                x.Property(y => y.Id).ValueGeneratedOnAdd();
                x.Property(y => y.Description).IsRequired().HasMaxLength(50);
                x.Property(y => y.Enabled).HasDefaultValue(true);
                x.Property(y => y.CreatedDate).IsRequired();
                x.Property(y => y.UpdatedDate).IsRequired();
            });
            modelBuilder.Entity<Dependency>().HasIndex(x => x.Description).IsUnique();
            #endregion

            #region User
            modelBuilder.Entity<User>().ToTable("Users").HasKey(x => x.Id);
            modelBuilder.Entity<User>(x =>
            {
                x.Property(y => y.Id).ValueGeneratedOnAdd();
                x.Property(y => y.Name).HasMaxLength(50);
                x.Property(y => y.LastName).HasMaxLength(50);
                x.Property(y => y.Email).IsRequired().HasMaxLength(50);
                x.Property(y => y.Password).HasMaxLength(150);
                x.Property(y => y.Activated).HasDefaultValue(false);
                x.Property(y => y.Enabled).HasDefaultValue(true);
                x.Property(y => y.IsFirstLogin).HasDefaultValue(true);
                x.Property(y => y.DependencyId).IsRequired();
                x.Property(y => y.CreatedDate).IsRequired();
                x.Property(y => y.UpdatedDate).IsRequired();
            });
            modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
            modelBuilder.Entity<User>()
                    .HasOne(d => d.Dependency)
                    .WithMany()
                    .HasForeignKey(d => d.DependencyId)
                    .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region Profile
            modelBuilder.Entity<Profile>().ToTable("Profiles").HasKey(x => x.Id);
            modelBuilder.Entity<Profile>(x =>
            {
                x.Property(y => y.Id).ValueGeneratedOnAdd();
                x.Property(y => y.Description).IsRequired().HasMaxLength(50);
                x.Property(y => y.Enabled).HasDefaultValue(true);
                x.Property(y => y.CreatedDate).IsRequired();
                x.Property(y => y.UpdatedDate).IsRequired();
            });
            modelBuilder.Entity<Profile>().HasIndex(x => x.Description).IsUnique();
            #endregion

            #region UserProfile
            modelBuilder.Entity<UserProfile>().ToTable("UserProfiles").HasKey(x => x.Id);
            modelBuilder.Entity<UserProfile>(x =>
            {
                x.Property(y => y.Id).ValueGeneratedOnAdd();
                x.Property(y => y.UserId).IsRequired();
                x.Property(y => y.ProfileId).IsRequired();
                x.Property(y => y.Enabled).HasDefaultValue(true);
                x.Property(y => y.CreatedDate).IsRequired();
                x.Property(y => y.UpdatedDate).IsRequired();

            });
            modelBuilder.Entity<UserProfile>()
                    .HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserProfile>()
                    .HasOne(d => d.Profile)
                    .WithMany()
                    .HasForeignKey(d => d.ProfileId)
                    .OnDelete(DeleteBehavior.Restrict);

            #endregion 

            #region UsersOneTimePass
            modelBuilder.Entity<UserOneTimePass>().ToTable("UsersOneTimePass").HasKey(x => x.Id);
            modelBuilder.Entity<UserOneTimePass>(x =>
            {
                x.Property(y => y.Id).ValueGeneratedOnAdd();
                x.Property(y => y.OTPCode).IsRequired().HasMaxLength(4);
                x.Property(y => y.Used).HasDefaultValue(false);
                x.Property(y => y.ReceivedByUser).HasDefaultValue(false);
                x.Property(y => y.Used).IsRequired();
                x.Property(y => y.UserId).IsRequired();
                x.Property(y => y.CreatedDate).IsRequired();

            });
            modelBuilder.Entity<UserOneTimePass>()
                    .HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region ProjectType
            modelBuilder.Entity<ProjectType>().ToTable("ProjectTypes").HasKey(x => x.Id);
            modelBuilder.Entity<ProjectType>(x =>
            {
                x.Property(y => y.Id).ValueGeneratedOnAdd();
                x.Property(y => y.Description).IsRequired().HasMaxLength(50);
                x.Property(y => y.Enabled).HasDefaultValue(true);
                x.Property(y => y.CreatedDate).IsRequired();
                x.Property(y => y.UpdatedDate).IsRequired();
            });
            modelBuilder.Entity<ProjectType>().HasIndex(x => x.Description).IsUnique();
            #endregion

            #region Project
            modelBuilder.Entity<Project>().ToTable("Projects").HasKey(x => x.Id);
            modelBuilder.Entity<Project>(x =>
            {
                x.Property(y => y.Id).ValueGeneratedOnAdd();
                x.Property(y => y.Name).HasMaxLength(500);
                x.Property(y => y.Description).HasColumnType("varchar(MAX)");
                x.Property(y => y.ImageName).IsRequired().HasColumnType("varchar(MAX)");
                x.Property(y => y.VideoName).HasColumnType("varchar(MAX)");
                x.Property(y => y.ImplementDate).IsRequired().HasMaxLength(200);
                x.Property(y => y.ImpactDescription).IsRequired().HasColumnType("varchar(MAX)");
                x.Property(y => y.Enabled).HasDefaultValue(true);
                x.Property(y => y.DependencyId).IsRequired();
                x.Property(y => y.ProjectTypeId).IsRequired();
                x.Property(y => y.CreatedDate).IsRequired();
                x.Property(y => y.UpdatedDate).IsRequired();
            });
            modelBuilder.Entity<Project>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<Project>()
                  .HasOne(d => d.Dependency)
                  .WithMany()
                  .HasForeignKey(d => d.DependencyId)
                  .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Project>()
                .HasOne(d => d.ProjectType)
                .WithMany()
                .HasForeignKey(d => d.ProjectTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region UserWalletAccount
            modelBuilder.Entity<UserWalletAccount>().ToTable("UserWalletAccounts").HasKey(x => x.Id);
            modelBuilder.Entity<UserWalletAccount>(x =>
            {
                x.Property(y => y.Id).IsRequired();
                x.Property(y => y.Coins).IsRequired().HasDefaultValue(0);
                x.Property(y => y.Enabled).HasDefaultValue(false);
                x.Property(y => y.UserId).IsRequired();
                x.Property(y => y.CreatedDate).IsRequired();
                x.Property(y => y.UpdatedDate).IsRequired();
            });
            modelBuilder.Entity<UserWalletAccount>()
                    .HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region WalletAccountWord
            modelBuilder.Entity<WalletAccountWord>().ToTable("WalletAccountWords").HasKey(x => x.Id);
            modelBuilder.Entity<WalletAccountWord>(x =>
            {
                x.Property(y => y.Id).ValueGeneratedOnAdd();
                x.Property(y => y.Words).IsRequired().HasMaxLength(500);
                x.Property(y => y.UserWalletAccountId).IsRequired();
                x.Property(y => y.CreatedDate).IsRequired();
                x.Property(y => y.UpdatedDate).IsRequired();
            });
            modelBuilder.Entity<WalletAccountWord>()
                    .HasOne(d => d.UserWalletAccount)
                    .WithMany()
                    .HasForeignKey(d => d.UserWalletAccountId)
                    .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region TransactionType
            modelBuilder.Entity<TransactionType>().ToTable("TransactionTypes").HasKey(x => x.Id);
            modelBuilder.Entity<TransactionType>(x =>
            {
                x.Property(y => y.Id).ValueGeneratedOnAdd();
                x.Property(y => y.Description).IsRequired().HasMaxLength(50);
                x.Property(y => y.Enabled).HasDefaultValue(true);
                x.Property(y => y.CreatedDate).IsRequired();
                x.Property(y => y.UpdatedDate).IsRequired();
            });
            modelBuilder.Entity<TransactionType>().HasIndex(x => x.Description).IsUnique();
            #endregion

            #region Profitability
            modelBuilder.Entity<Profitability>().ToTable("Profitabilities").HasKey(x => x.Id);
            modelBuilder.Entity<Profitability>(x =>
            {
                x.Property(y => y.Id).ValueGeneratedOnAdd();
                x.Property(y => y.ProfitabilityPercentage).IsRequired();
                x.Property(y => y.StartTime).IsRequired();
                x.Property(y => y.FinalTIme).IsRequired();
                x.Property(y => y.UpdatedDate).IsRequired();
            });
            #endregion

            #region UserWalletAccountTransaction
            modelBuilder.Entity<UserWalletAccountTransaction>().ToTable("UserWalletAccountTransactions").HasKey(x => x.Id);
            modelBuilder.Entity<UserWalletAccountTransaction>(x =>
            {
                x.Property(y => y.Id).IsRequired();
                x.Property(y => y.Coins).IsRequired();
                x.Property(y => y.UserWalletAccountId).IsRequired();
                x.Property(y => y.CoinsEarned).IsRequired().HasDefaultValue(0);
                x.Property(y => y.ProfitabilityPercentaje).IsRequired().HasDefaultValue(0);
                x.Property(y => y.TransactionTypeId).IsRequired();
                x.Property(y => y.CreatedDate).IsRequired();

            });
            modelBuilder.Entity<UserWalletAccountTransaction>()
                    .HasOne(d => d.UserWalletAccount)
                    .WithMany()
                    .HasForeignKey(d => d.UserWalletAccountId)
                    .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserWalletAccountTransaction>()
                    .HasOne(d => d.TransactionType)
                    .WithMany()
                    .HasForeignKey(d => d.TransactionTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region UserInvestmentProject
            modelBuilder.Entity<UserInvestmentProject>().ToTable("UserInvestmentProjects").HasKey(x => x.Id);
            modelBuilder.Entity<UserInvestmentProject>(x =>
            {
                x.Property(y => y.Id).ValueGeneratedOnAdd();
                x.Property(y => y.UserWalletAccountTransactionId).IsRequired();
                x.Property(y => y.ProjectId).IsRequired();
                x.Property(y => y.CreatedDate).IsRequired();

            });
            modelBuilder.Entity<UserInvestmentProject>()
                    .HasOne(d => d.UserWalletAccountTransaction)
                    .WithMany()
                    .HasForeignKey(d => d.UserWalletAccountTransactionId)
                    .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserInvestmentProject>()
                    .HasOne(d => d.Project)
                    .WithMany()
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.Restrict);
            #endregion

            modelBuilder.Seed();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken()) => base.SaveChangesAsync(cancellationToken);
    }
}