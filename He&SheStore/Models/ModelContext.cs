using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace He_SheStore.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Card> Cards { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Orderaddress> Orderaddresses { get; set; }

    public virtual DbSet<Orderitem> Orderitems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Testmonial> Testmonials { get; set; }

    public virtual DbSet<UserLogin> UserLogins { get; set; }

    public virtual DbSet<Wishlist> Wishlists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521))(CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME =xepdb1)));User Id=FirstProject;password=1234;Persist Security Info=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("FIRSTPROJECT")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Card>(entity =>
        {
            entity.HasKey(e => e.Cardid).HasName("SYS_C009129");

            entity.ToTable("CARD");

            entity.Property(e => e.Cardid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CARDID");
            entity.Property(e => e.Balance)
                .HasColumnType("NUMBER")
                .HasColumnName("BALANCE");
            entity.Property(e => e.CardCvc)
                .HasPrecision(3)
                .HasColumnName("CARD_CVC");
            entity.Property(e => e.CardNumber)
                .HasPrecision(16)
                .HasColumnName("CARD_NUMBER");
            entity.Property(e => e.CardholderName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CARDHOLDER_NAME");
            entity.Property(e => e.CustomerId)
                .HasColumnType("NUMBER")
                .HasColumnName("CUSTOMER_ID");
            entity.Property(e => e.ExpiryDate)
                .HasColumnType("DATE")
                .HasColumnName("EXPIRY_DATE");

            entity.HasOne(d => d.Customer).WithMany(p => p.Cards)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("SYS_C009130");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("SYS_C009082");

            entity.ToTable("CART");

            entity.Property(e => e.CartId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CART_ID");
            entity.Property(e => e.CustomerId)
                .HasColumnType("NUMBER")
                .HasColumnName("CUSTOMER_ID");
            entity.Property(e => e.ProductId)
                .HasColumnType("NUMBER")
                .HasColumnName("PRODUCT_ID");
            entity.Property(e => e.ProductQuantity)
                .HasColumnType("NUMBER")
                .HasColumnName("PRODUCT_QUANTITY");
            entity.Property(e => e.Quantity)
                .HasColumnType("NUMBER")
                .HasColumnName("QUANTITY");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("TOTAL_PRICE");

            entity.HasOne(d => d.Customer).WithMany(p => p.Carts)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("SYS_C009084");

            entity.HasOne(d => d.Product).WithMany(p => p.Carts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("SYS_C009083");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("SYS_C009055");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CATEGORY_ID");
            entity.Property(e => e.CategoryDescription)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("CATEGORY_DESCRIPTION");
            entity.Property(e => e.CategoryImage)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("CATEGORY_IMAGE");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CATEGORY_NAME");
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.ContactId).HasName("SYS_C009086");

            entity.ToTable("CONTACT");

            entity.Property(e => e.ContactId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CONTACT_ID");
            entity.Property(e => e.Email)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Fullname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("FULLNAME");
            entity.Property(e => e.Messeage)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("MESSEAGE");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TITLE");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("SYS_C009048");

            entity.ToTable("CUSTOMER");

            entity.Property(e => e.CustomerId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CUSTOMER_ID");
            entity.Property(e => e.Birthdate)
                .HasColumnType("DATE")
                .HasColumnName("BIRTHDATE");
            entity.Property(e => e.Fname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FNAME");
            entity.Property(e => e.Lname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LNAME");
            entity.Property(e => e.Ph)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("PH");
            entity.Property(e => e.Profilepicture)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("PROFILEPICTURE");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Orderid).HasName("SYS_C009098");

            entity.ToTable("ORDER");

            entity.Property(e => e.Orderid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ORDERID");
            entity.Property(e => e.CustomerId)
                .HasColumnType("NUMBER")
                .HasColumnName("CUSTOMER_ID");
            entity.Property(e => e.OrderAddressId)
                .HasColumnType("NUMBER")
                .HasColumnName("ORDER_ADDRESS_ID");
            entity.Property(e => e.Orderdate)
                .HasColumnType("DATE")
                .HasColumnName("ORDERDATE");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("STATUS");
            entity.Property(e => e.Totalamount)
                .HasColumnType("NUMBER")
                .HasColumnName("TOTALAMOUNT");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("SYS_C009099");

            entity.HasOne(d => d.OrderAddress).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderAddressId)
                .HasConstraintName("SYS_C009100");
        });

        modelBuilder.Entity<Orderaddress>(entity =>
        {
            entity.HasKey(e => e.OrderAddressId).HasName("SYS_C009096");

            entity.ToTable("ORDERADDRESS");

            entity.Property(e => e.OrderAddressId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ORDER_ADDRESS_ID");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CITY");
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("COUNTRY");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Fname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("FNAME");
            entity.Property(e => e.Lname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("LNAME");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PHONENUMBER");
            entity.Property(e => e.Postalcode)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("POSTALCODE");
            entity.Property(e => e.Streetnumber)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("STREETNUMBER");
        });

        modelBuilder.Entity<Orderitem>(entity =>
        {
            entity.HasKey(e => e.OrderitemId).HasName("SYS_C009104");

            entity.ToTable("ORDERITEM");

            entity.Property(e => e.OrderitemId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ORDERITEM_ID");
            entity.Property(e => e.ItemPrice)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ITEM_PRICE");
            entity.Property(e => e.OrderId)
                .HasColumnType("NUMBER")
                .HasColumnName("ORDER_ID");
            entity.Property(e => e.ProductId)
                .HasColumnType("NUMBER")
                .HasColumnName("PRODUCT_ID");
            entity.Property(e => e.Quantitiy)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("QUANTITIY");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderitems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("SYS_C009105");

            entity.HasOne(d => d.Product).WithMany(p => p.Orderitems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("SYS_C009106");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("SYS_C009061");

            entity.ToTable("PRODUCT");

            entity.HasIndex(e => e.ProductName, "SYS_C009062").IsUnique();

            entity.Property(e => e.ProductId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("PRODUCT_ID");
            entity.Property(e => e.CategoryId)
                .HasColumnType("NUMBER")
                .HasColumnName("CATEGORY_ID");
            entity.Property(e => e.ProductDescription)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("PRODUCT_DESCRIPTION");
            entity.Property(e => e.ProductImage)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("PRODUCT_IMAGE");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PRODUCT_NAME");
            entity.Property(e => e.ProductPrice)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("PRODUCT_PRICE");
            entity.Property(e => e.ProductStatus)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValueSql("'In Stock'")
                .HasColumnName("PRODUCT_STATUS");
            entity.Property(e => e.StockQuantity)
                .HasColumnType("NUMBER")
                .HasColumnName("STOCK_QUANTITY");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("SYS_C009063");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Reviewid).HasName("SYS_C009072");

            entity.ToTable("REVIEW");

            entity.Property(e => e.Reviewid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("REVIEWID");
            entity.Property(e => e.Comment)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("COMMENT");
            entity.Property(e => e.CustomerId)
                .HasColumnType("NUMBER")
                .HasColumnName("CUSTOMER_ID");
            entity.Property(e => e.ProductId)
                .HasColumnType("NUMBER")
                .HasColumnName("PRODUCT_ID");
            entity.Property(e => e.Reating)
                .HasColumnType("NUMBER")
                .HasColumnName("REATING");

            entity.HasOne(d => d.Customer).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("SYS_C009074");

            entity.HasOne(d => d.Product).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("SYS_C009073");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("SYS_C009045");

            entity.ToTable("ROLE");

            entity.HasIndex(e => e.RoleName, "SYS_C009046").IsUnique();

            entity.Property(e => e.RoleId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ROLE_ID");
            entity.Property(e => e.RoleName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("ROLE_NAME");
        });

        modelBuilder.Entity<Testmonial>(entity =>
        {
            entity.HasKey(e => e.Testmonialid).HasName("SYS_C009065");

            entity.ToTable("TESTMONIAL");

            entity.Property(e => e.Testmonialid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("TESTMONIALID");
            entity.Property(e => e.Comment)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("COMMENT");
            entity.Property(e => e.CustomerId)
                .HasColumnType("NUMBER")
                .HasColumnName("CUSTOMER_ID");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("STATUS");

            entity.HasOne(d => d.Customer).WithMany(p => p.Testmonials)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("SYS_C009066");
        });

        modelBuilder.Entity<UserLogin>(entity =>
        {
            entity.HasKey(e => e.UserloginId).HasName("SYS_C009132");

            entity.ToTable("USER_LOGIN");

            entity.Property(e => e.UserloginId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("USERLOGIN_ID");
            entity.Property(e => e.CustomerId)
                .HasColumnType("NUMBER")
                .HasColumnName("CUSTOMER_ID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Password)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.RoleId)
                .HasColumnType("NUMBER")
                .HasColumnName("ROLE_ID");

            entity.HasOne(d => d.Customer).WithMany(p => p.UserLogins)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("SYS_C009134");

            entity.HasOne(d => d.Role).WithMany(p => p.UserLogins)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("SYS_C009133");
        });

        modelBuilder.Entity<Wishlist>(entity =>
        {
            entity.HasKey(e => e.Wishlistid).HasName("SYS_C009078");

            entity.ToTable("WISHLIST");

            entity.Property(e => e.Wishlistid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("WISHLISTID");
            entity.Property(e => e.CustomerId)
                .HasColumnType("NUMBER")
                .HasColumnName("CUSTOMER_ID");
            entity.Property(e => e.ProductId)
                .HasColumnType("NUMBER")
                .HasColumnName("PRODUCT_ID");

            entity.HasOne(d => d.Customer).WithMany(p => p.Wishlists)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("SYS_C009079");

            entity.HasOne(d => d.Product).WithMany(p => p.Wishlists)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("SYS_C009080");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
