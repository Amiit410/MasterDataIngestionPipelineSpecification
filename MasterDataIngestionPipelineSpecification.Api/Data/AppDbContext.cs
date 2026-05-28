using MasterDataIngestionPipelineSpecification.Api.Entitties;
using Microsoft.EntityFrameworkCore;

namespace MasterDataIngestionPipelineSpecification.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Staging
        public DbSet<LogCustomerIngestion> LogCustomerIngestions => Set<LogCustomerIngestion>();
        public DbSet<LogItemIngestion> LogItemIngestions => Set<LogItemIngestion>();

        // Master
        public DbSet<MRegion> MRegions => Set<MRegion>();
        public DbSet<MCity> MCities => Set<MCity>();
        public DbSet<MPaymentTerm> MPaymentTerms => Set<MPaymentTerm>();
        public DbSet<MChannel> MChannels => Set<MChannel>();
        public DbSet<MBrand> MBrands => Set<MBrand>();
        public DbSet<MCategory> MCategories => Set<MCategory>();
        public DbSet<MUom> MUoms => Set<MUom>();

        // Transaction
        public DbSet<TCustomer> TCustomers => Set<TCustomer>();
        public DbSet<TItem> TItems => Set<TItem>();
        public DbSet<TItemUomConversion> TItemUomConversions => Set<TItemUomConversion>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── LogCustomerIngestion ──────────────────────────────────────────────
            modelBuilder.Entity<LogCustomerIngestion>(e =>
            {
                e.ToTable("log_customer_ingestion");
                e.HasKey(x => x.LogId);
                e.Property(x => x.LogId).HasDefaultValueSql("NEWID()");
                e.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
                e.Property(x => x.ProcessStatus).HasConversion<string>().HasMaxLength(20);
                e.Property(x => x.RawPayload).HasColumnType("nvarchar(max)");
                e.Property(x => x.ValidationDetails).HasColumnType("nvarchar(max)");
            });

            // ── LogItemIngestion ──────────────────────────────────────────────────
            modelBuilder.Entity<LogItemIngestion>(e =>
            {
                e.ToTable("log_item_ingestion");
                e.HasKey(x => x.LogId);
                e.Property(x => x.LogId).HasDefaultValueSql("NEWID()");
                e.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
                e.Property(x => x.ProcessStatus).HasConversion<string>().HasMaxLength(20);
                e.Property(x => x.RawPayload).HasColumnType("nvarchar(max)");
                e.Property(x => x.ValidationDetails).HasColumnType("nvarchar(max)");
            });

            // ── MRegion ───────────────────────────────────────────────────────────
            modelBuilder.Entity<MRegion>(e =>
            {
                e.ToTable("M_REGION");
                e.HasKey(x => x.RegionId);
                e.HasIndex(x => x.RegionCode).IsUnique();
                e.Property(x => x.RegionCode).HasMaxLength(20).IsRequired();
                e.Property(x => x.RegionName).HasMaxLength(100).IsRequired();
            });

            // ── MCity ─────────────────────────────────────────────────────────────
            modelBuilder.Entity<MCity>(e =>
            {
                e.ToTable("M_CITY");
                e.HasKey(x => x.CityId);
                e.HasIndex(x => x.CityCode).IsUnique();
                e.Property(x => x.CityCode).HasMaxLength(20).IsRequired();
                e.Property(x => x.CityName).HasMaxLength(100).IsRequired();
                e.HasOne(x => x.Region).WithMany(r => r.Cities).HasForeignKey(x => x.RegionId);
            });

            // ── MPaymentTerm ──────────────────────────────────────────────────────
            modelBuilder.Entity<MPaymentTerm>(e =>
            {
                e.ToTable("M_PAYMENT_TERM");
                e.HasKey(x => x.PaymentTermId);
                e.HasIndex(x => x.TermCode).IsUnique();
                e.Property(x => x.TermCode).HasMaxLength(20).IsRequired();
                e.Property(x => x.TermName).HasMaxLength(100).IsRequired();
                e.Property(x => x.CreditLimit).HasColumnType("decimal(18,2)");
            });

            // ── MChannel ──────────────────────────────────────────────────────────
            modelBuilder.Entity<MChannel>(e =>
            {
                e.ToTable("M_CHANNEL");
                e.HasKey(x => x.ChannelId);
                e.HasIndex(x => x.ChannelCode).IsUnique();
                e.Property(x => x.ChannelCode).HasMaxLength(20).IsRequired();
                e.Property(x => x.ChannelName).HasMaxLength(100).IsRequired();
            });

            // ── MBrand ────────────────────────────────────────────────────────────
            modelBuilder.Entity<MBrand>(e =>
            {
                e.ToTable("M_BRAND");
                e.HasKey(x => x.BrandId);
                e.HasIndex(x => x.BrandCode).IsUnique();
                e.Property(x => x.BrandCode).HasMaxLength(20).IsRequired();
                e.Property(x => x.BrandName).HasMaxLength(100).IsRequired();
            });

            // ── MCategory ─────────────────────────────────────────────────────────
            modelBuilder.Entity<MCategory>(e =>
            {
                e.ToTable("M_CATEGORY");
                e.HasKey(x => x.CategoryId);
                e.HasIndex(x => x.CategoryCode).IsUnique();
                e.Property(x => x.CategoryCode).HasMaxLength(20).IsRequired();
                e.Property(x => x.CategoryName).HasMaxLength(100).IsRequired();
            });

            // ── MUom ──────────────────────────────────────────────────────────────
            modelBuilder.Entity<MUom>(e =>
            {
                e.ToTable("M_UOM");
                e.HasKey(x => x.UomId);
                e.HasIndex(x => x.UomCode).IsUnique();
                e.Property(x => x.UomCode).HasMaxLength(20).IsRequired();
            });

            // ── TCustomer ─────────────────────────────────────────────────────────
            modelBuilder.Entity<TCustomer>(e =>
            {
                e.ToTable("T_CUSTOMER");
                e.HasKey(x => x.CustomerId);
                e.HasIndex(x => x.CustomerCode).IsUnique();
                e.Property(x => x.CustomerCode).HasMaxLength(20).IsRequired();
                e.Property(x => x.CustomerName).HasMaxLength(200).IsRequired();
                e.Property(x => x.ContactNo).HasMaxLength(20);
                e.Property(x => x.Email).HasMaxLength(100);
                e.Property(x => x.CustomerType).HasMaxLength(20);
                e.Property(x => x.CreditLimit).HasColumnType("decimal(18,2)");
                e.HasOne(x => x.City).WithMany().HasForeignKey(x => x.CityId);
                e.HasOne(x => x.Region).WithMany().HasForeignKey(x => x.RegionId);
            });

            // ── TItem ─────────────────────────────────────────────────────────────
            modelBuilder.Entity<TItem>(e =>
            {
                e.ToTable("T_ITEM");
                e.HasKey(x => x.ItemId);
                e.HasIndex(x => x.ItemCode).IsUnique();
                e.Property(x => x.ItemCode).HasMaxLength(20).IsRequired();
                e.Property(x => x.ItemName).HasMaxLength(200).IsRequired();
                e.Property(x => x.SalesOrgCode).HasMaxLength(20);
                e.Property(x => x.BaseUOM).HasMaxLength(20);
                e.Property(x => x.IsActive).HasMaxLength(1);
                e.HasOne(x => x.Brand).WithMany(b => b.Items).HasForeignKey(x => x.BrandId);
                e.HasOne(x => x.Category).WithMany(c => c.Items).HasForeignKey(x => x.CategoryId);
            });

            // ── TItemUomConversion ────────────────────────────────────────────────
            modelBuilder.Entity<TItemUomConversion>(e =>
            {
                e.ToTable("T_ITEM_UOM_CONVERSION");
                e.HasKey(x => x.ConversionId);
                e.HasIndex(x => new { x.ItemId, x.UomId }).IsUnique();
                e.Property(x => x.ConversionFactor).HasColumnType("decimal(18,6)");
                e.HasOne(x => x.Item).WithMany(i => i.UomConversions).HasForeignKey(x => x.ItemId);
                e.HasOne(x => x.Uom).WithMany(u => u.Conversions).HasForeignKey(x => x.UomId);
            });
        }
    }
}
