using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace DataLayer.Model
{
    public partial class SWD392_DBContext : DbContext
    {
        public SWD392_DBContext()
        {
        }

        public SWD392_DBContext(DbContextOptions<SWD392_DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Appeal> Appeals { get; set; } = null!;
        public virtual DbSet<BannedAccount> BannedAccounts { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<Exchanged> Exchangeds { get; set; } = null!;
        public virtual DbSet<ExchangedProduct> ExchangedProducts { get; set; } = null!;
        public virtual DbSet<Group> Groups { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Rating> Ratings { get; set; } = null!;
        public virtual DbSet<Report> Reports { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<SubCategory> SubCategories { get; set; } = null!;
        public virtual DbSet<Token> Tokens { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
         => optionsBuilder.UseSqlServer(GetConnectionString());

        string GetConnectionString()
        {
            IConfiguration builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            return builder["ConnectionStrings:local"];
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appeal>(entity =>
            {
                entity.ToTable("Appeal");

                entity.Property(e => e.BannerAcountId).HasColumnName("BannerAcount_Id");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Modified_Date");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.HasOne(d => d.BannerAcount)
                    .WithMany(p => p.Appeals)
                    .HasForeignKey(d => d.BannerAcountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Appeal__BannerAc__6A30C649");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Appeals)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Appeal__User_Id__693CA210");
            });

            modelBuilder.Entity<BannedAccount>(entity =>
            {
                entity.ToTable("BannedAccount");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Modified_Date");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BannedAccounts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BannedAcc__User___66603565");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.PostId).HasColumnName("Post_Id");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Comment__Post_Id__7B5B524B");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Comment__User_Id__7A672E12");
            });

            modelBuilder.Entity<Exchanged>(entity =>
            {
                entity.ToTable("Exchanged");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("DATE");

                entity.Property(e => e.PostId).HasColumnName("Post_Id");

                entity.Property(e => e.StatusRating).HasDefaultValueSql("((1))");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Exchangeds)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Exchanged__Post___0A9D95DB");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Exchangeds)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Exchanged__User___09A971A2");
            });

            modelBuilder.Entity<ExchangedProduct>(entity =>
            {
                entity.ToTable("ExchangedProduct");

                entity.HasOne(d => d.Exchange)
                    .WithMany(p => p.ExchangedProducts)
                    .HasForeignKey(d => d.ExchangeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Exchanged__Excha__0D7A0286");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ExchangedProducts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Exchanged__Produ__0E6E26BF");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("Group");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Groups)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Group__PostId__14270015");

                entity.HasOne(d => d.UserExchange)
                    .WithMany(p => p.Groups)
                    .HasForeignKey(d => d.UserExchangeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Group__UserExcha__151B244E");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("Message");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Message__GroupId__18EBB532");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.SenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Message__SenderI__17F790F9");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notification");

                entity.Property(e => e.ReceiverId).HasColumnName("Receiver_Id");

                entity.HasOne(d => d.Receiver)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.ReceiverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Notificat__Recei__3C34F16F");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.PaymentId).HasColumnName("Payment_Id");

                entity.Property(e => e.TotalPrice).HasColumnName("Total_Price");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.PaymentId)
                    .HasConstraintName("FK__Order__Payment_I__02FC7413");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Order__User_Id__02084FDA");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.Property(e => e.OrderId).HasColumnName("Order_Id");

                entity.Property(e => e.ProductId).HasColumnName("Product_Id");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderDeta__Order__06CD04F7");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderDeta__Produ__05D8E0BE");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.Method).HasMaxLength(50);
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Post");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.ProductId).HasColumnName("Product_Id");

                entity.Property(e => e.Title).HasMaxLength(100);

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Post__Product_Id__778AC167");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Post__User_Id__76969D2E");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.CategoryId).HasColumnName("Category_Id");

                entity.Property(e => e.Location).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.SubCategoryId).HasColumnName("SubCategory_Id");

                entity.Property(e => e.UrlImg).HasColumnName("Url_IMG");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Product__Categor__73BA3083");

                entity.HasOne(d => d.SubCategory)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.SubCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Product__SubCate__71D1E811");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Product__User_Id__72C60C4A");
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.ToTable("Rating");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("DATE");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Ratings)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK_Rating_Post");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Ratings)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Rating__User_Id__114A936A");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.ToTable("Report");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.PostId).HasColumnName("Post_Id");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Report__Post_Id__7F2BE32F");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Report__User_Id__7E37BEF6");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<SubCategory>(entity =>
            {
                entity.ToTable("SubCategory");

                entity.Property(e => e.CategoryId).HasColumnName("Category_Id");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.SubCategories)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SubCatego__Categ__6EF57B66");
            });

            modelBuilder.Entity<Token>(entity =>
            {
                entity.ToTable("Token");

                entity.Property(e => e.Expiration).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Tokens)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Token__User_Id__619B8048");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("date")
                    .HasColumnName("Created_Date");

                entity.Property(e => e.Dob)
                    .HasColumnType("date")
                    .HasColumnName("DOB");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Gender).HasMaxLength(50);

                entity.Property(e => e.ImgUrl).HasColumnName("ImgURL");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("date")
                    .HasColumnName("Modified_Date");

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(50)
                    .HasColumnName("Phone_Number");

                entity.Property(e => e.RatingCount).HasColumnName("Rating_Count");

                entity.Property(e => e.RoleId).HasColumnName("Role_Id");

                entity.Property(e => e.UserName).HasMaxLength(50);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__User__Role_Id__5EBF139D");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
