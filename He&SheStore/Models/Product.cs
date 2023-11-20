using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace He_SheStore.Models;

public partial class Product
{
    public decimal ProductId { get; set; }

    public string? ProductName { get; set; }

    public string? ProductDescription { get; set; }

    public string? ProductImage { get; set; }

    public decimal? ProductPrice { get; set; }

    public decimal? StockQuantity { get; set; }

    public string? ProductStatus { get; set; }

    public decimal? CategoryId { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual Category? Category { get; set; }

    public virtual ICollection<Orderitem> Orderitems { get; set; } = new List<Orderitem>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();

	[NotMapped]
	public IFormFile ImageFile { get; set; }

	[NotMapped]
	public decimal? AverageRating { get; set; }
}
