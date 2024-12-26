using Gigashop.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gigashop.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<ContactMessage> Categorys { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define foreign key relationships explicitly
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Product)
                .WithMany() // Assuming a product can have multiple reviews
                .HasForeignKey(r => r.ProductID)
                .OnDelete(DeleteBehavior.Restrict); // Or whatever behavior you prefer

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany() // Assuming a user can write multiple reviews
                .HasForeignKey(r => r.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);


          // Cart Table Configuration
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserID);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Product)
                .WithMany()
                .HasForeignKey(c => c.ProductID);

            // OrderDetail Table Configuration
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Cart)
                .WithMany()
                .HasForeignKey(od => od.CartID);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.User)
                .WithMany()
                .HasForeignKey(od => od.UserID);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany()
                .HasForeignKey(od => od.ProductID);
            // Cấu hình mối quan hệ giữa Product và Review
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Reviews)
                .WithOne(r => r.Product)
                .HasForeignKey(r => r.ProductID)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }

    // Example model class for the "Products" table
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int Stock { get; set; }

        public int? CategoryID { get; set; }

        public string Specifications { get; set; }

        public double Rating { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public List<Review> Reviews { get; set; }

    }

    [Table("Users", Schema = "dbo")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        public string Address { get; set; }

        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = "User";

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public ICollection<ContactMessage> ContactMessages { get; set; }
    }

    [Table("Services")]
    public class Service
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ServiceID { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }

    [Table("Reviews")]
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReviewID { get; set; }

        [Required]
        public int ProductID { get; set; } // Linked to Products

        [Required]
        public int UserID { get; set; } // Linked to Users

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(1000)]
        public string Comment { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        // Optional: Navigation properties
        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; }
    }
    [Table("ContactMessages", Schema = "dbo")]
    public class ContactMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageID { get; set; }     // Khóa chính
        public int UserID { get; set; }        // Khóa ngoại, liên kết với bảng Users
        public string Name { get; set; }       // Tên người gửi
        public string Email { get; set; }      // Email của người gửi
        public string Subject { get; set; }    // Tiêu đề
        public string Message { get; set; }    // Nội dung tin nhắn
        public DateTime CreatedAt { get; set; } // Thời gian tin nhắn được gửi
    }
    [Table("Categorys", Schema = "dbo")]
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentCategoryID { get; set; }  // Cho phép null nếu không có Parent Category
        public DateTime CreatedAt { get; set; }

        // Navigation property để liên kết với Parent Category (nếu có)
        public virtual Category ParentCategory { get; set; }
        public virtual ICollection<Category> SubCategories { get; set; }
    }

    public class Cart
    {
        [Key]
        public int CartID { get; set; }

        [Required]
        public int UserID { get; set; } // Foreign key liên kết với User

        [Required]
        public int ProductID { get; set; } // Foreign key liên kết với Product

        [Required]
        public int Quantity { get; set; } // Số lượng sản phẩm

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public User User { get; set; }
        public Product Product { get; set; }
    }
    public class OrderDetail
    {
        [Key]
        public int OrderDetailID { get; set; }

        [Required]
        public int CartID { get; set; } // Foreign key liên kết với Cart

        [Required]
        public int UserID { get; set; } // Foreign key liên kết với User

        [Required]
        public int ProductID { get; set; } // Foreign key liên kết với Product

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; } // Giá sản phẩm tại thời điểm đặt hàng

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice => Quantity * UnitPrice; // Tổng giá (tính toán tự động)

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public Cart Cart { get; set; }
        public User User { get; set; }
        public Product Product { get; set; }
    }

}