using System;
using System.Collections.Generic;

namespace He_SheStore.Models;

public partial class Orderitem
{
    public decimal OrderitemId { get; set; }

    public decimal? OrderId { get; set; }

    public decimal? ProductId { get; set; }

    public decimal Quantitiy { get; set; }

    public decimal ItemPrice { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}
