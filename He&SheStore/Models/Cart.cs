using System;
using System.Collections.Generic;

namespace He_SheStore.Models;

public partial class Cart
{
    public decimal CartId { get; set; }

    public decimal? ProductQuantity { get; set; }

    public decimal? TotalPrice { get; set; }

    public decimal? ProductId { get; set; }

    public decimal? CustomerId { get; set; }

    public decimal? Quantity { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Product? Product { get; set; }
}
