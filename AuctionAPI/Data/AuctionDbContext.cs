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

            modelBuilder.Entity<Auction>()
                .HasOne(a => a.Product)
                .WithMany(p => p.Auctions)
                .HasForeignKey(a => a.ProductId)
                .OnDelete(DeleteBehavior.Restrict); 

            
            modelBuilder.Entity<Auction>()
                .HasOne(a => a.Winner)
                .WithMany(u => u.WonAuctions)
                .HasForeignKey(a => a.WinnerUserId)
                .OnDelete(DeleteBehavior.Restrict); 

            
            modelBuilder.Entity<Bid>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bids)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict); 
            modelBuilder.Entity<Bid>()
                .HasOne(b => b.Auction)
                .WithMany(a => a.Bids)
                .HasForeignKey(b => b.AuctionId)
                .OnDelete(DeleteBehavior.Restrict); 
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Provider)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.ProviderId)
                .OnDelete(DeleteBehavior.Restrict); 
            modelBuilder.Entity<Organization>()
                .HasOne(o => o.AdminUser)
                .WithMany(u => u.Organizations)
                .HasForeignKey(o => o.AdminUserId)
                .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
