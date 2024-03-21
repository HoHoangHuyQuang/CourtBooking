
using Microsoft.EntityFrameworkCore;
using CourtBooking.Models;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CourtBooking.Data
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public class DatabaseContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Court> Courts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        
        public DbSet<Course> Courses { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("tb_Product");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasDefaultValueSql("UUID()").HasColumnType("nvarchar(50)");
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.Price).HasPrecision(14, 0); ;
                entity.Property(e => e.ProductSize);
                entity.Property(e => e.Model);
                entity.Property(e => e.Origin);
                entity.Property(e => e.ThumbnailURl);
                entity.Property(e => e.HtmlContent);
                entity.Property(e => e.Description);
                entity.Property(e => e.CateId);
                entity.Property(e => e.ContactPhone);

                entity.HasOne(p => p.Category)
                   .WithMany(c => c.Products)
                   .HasForeignKey(b => b.CateId)
                   .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("tb_Category");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.Description);
            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("tb_Users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasDefaultValueSql("UUID()").HasColumnType("nvarchar(50)");
                entity.Property(e => e.FullName).HasMaxLength(50);
                entity.Property(e => e.UserName).HasMaxLength(50).IsUnicode(false);
                entity.HasIndex(e => e.UserName).IsUnique();
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Phone).HasMaxLength(12);
                entity.Property(e => e.Password).HasMaxLength(120).IsUnicode(false);
                entity.Property(e => e.CreatedAt);
                entity.Property(e => e.IsActive);

                entity.HasOne(u => u.Role)
                   .WithMany(r => r.Users)
                   .HasForeignKey(b => b.RoleId)
                   .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("tb_Role");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.Description);
            });
         
            modelBuilder.Entity<Court>(entity =>
            {
                entity.ToTable("tb_Court");
                entity.HasKey(e => e.CourtId);
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.Address).HasMaxLength(250);
                entity.Property(e => e.PricePerHour).HasPrecision(14, 0);
                entity.Property(e => e.Description);


            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("tb_Booking");
                entity.HasKey(e => e.BookingId);
                entity.Property(e => e.BookingId).HasDefaultValueSql("UUID()").HasColumnType("nvarchar(50)"); ; ;
                entity.Property(e => e.StartTime);
                entity.Property(e => e.DurationInHour);
                entity.Property(e => e.EndTime);
                entity.Property(e => e.TotalAmount).HasPrecision(14, 0);
                entity.Property(e => e.ExtraData);
                entity.Property(e => e.Checksum);
                entity.Property(e => e.CreatedAt);
                entity.Property(e => e.Status);


                entity.HasOne(u => u.Users)
                    .WithMany(u => u.Bookings)
                    .HasForeignKey(b => b.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(c => c.Court)
                    .WithMany(c => c.Bookings)
                    .HasForeignKey(b => b.CourtId)
                    .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("tb_Course");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).HasMaxLength(50);
                entity.Property(e => e.Content);


            });

            base.OnModelCreating(modelBuilder);
        }


    }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
