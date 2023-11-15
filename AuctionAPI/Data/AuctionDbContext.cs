using AuctionAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionAPI.Data
{
    public class AuctionDbContext : DbContext
    {
        public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Auction ve Product arasındaki ilişki
            modelBuilder.Entity<Auction>()
                .HasOne(a => a.Product)
                .WithMany(p => p.Auctions)
                .HasForeignKey(a => a.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // Ürün silindiğinde ilişkili açık artırmaları silme

            // Auction ve User (Winner) arasındaki ilişki
            modelBuilder.Entity<Auction>()
                .HasOne(a => a.Winner)
                .WithMany(u => u.WonAuctions)
                .HasForeignKey(a => a.WinnerUserId)
                .OnDelete(DeleteBehavior.Restrict); // Kazanan kullanıcı silindiğinde ilişkili açık artırmaları silme

            // Bid ve User arasındaki ilişki
            modelBuilder.Entity<Bid>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bids)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Kullanıcı silindiğinde ilişkili teklifleri silme

            // Bid ve Auction arasındaki ilişki
            modelBuilder.Entity<Bid>()
                .HasOne(b => b.Auction)
                .WithMany(a => a.Bids)
                .HasForeignKey(b => b.AuctionId)
                .OnDelete(DeleteBehavior.Restrict); // Açık artırma silindiğinde ilişkili teklifleri silme

            // Product ve User (Provider) arasındaki ilişki
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Provider)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.ProviderId)
                .OnDelete(DeleteBehavior.Restrict); // Sağlayıcı kullanıcı silindiğinde ilişkili ürünleri silme

            // Organization ve User (AdminUser) arasındaki ilişki
            modelBuilder.Entity<Organization>()
                .HasOne(o => o.AdminUser)
                .WithMany(u => u.Organizations)
                .HasForeignKey(o => o.AdminUserId)
                .OnDelete(DeleteBehavior.Restrict); // Admin kullanıcı silindiğinde ilişkili organizasyonları silme
        }
    }
}
