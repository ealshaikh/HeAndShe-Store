using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace He_SheStore.Models;

public partial class Customer
{
    public decimal CustomerId { get; set; }

    public string? Fname { get; set; }

    public string? Lname { get; set; }

    public string? Profilepicture { get; set; }

    public string? Ph { get; set; }

    public DateTime? Birthdate { get; set; }

    public virtual ICollection<Card> Cards { get; set; } = new List<Card>();

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Testmonial> Testmonials { get; set; } = new List<Testmonial>();

    public virtual ICollection<UserLogin> UserLogins { get; set; } = new List<UserLogin>();

    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();

    [NotMapped]
    public IFormFile ImageFile { get; set; }
}
