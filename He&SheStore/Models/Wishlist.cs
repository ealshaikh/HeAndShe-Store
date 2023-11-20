using System;
using System.Collections.Generic;

namespace He_SheStore.Models;

public partial class Wishlist
{
    public decimal Wishlistid { get; set; }

    public decimal? CustomerId { get; set; }

    public decimal? ProductId { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Product? Product { get; set; }
}
