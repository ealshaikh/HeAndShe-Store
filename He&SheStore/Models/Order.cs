using System;
using System.Collections.Generic;

namespace He_SheStore.Models;

public partial class Order
{
    public decimal Orderid { get; set; }

    public decimal? CustomerId { get; set; }

    public string? Status { get; set; }

    public DateTime? Orderdate { get; set; }

    public decimal? Totalamount { get; set; }

    public decimal? OrderAddressId { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Orderaddress? OrderAddress { get; set; }

    public virtual ICollection<Orderitem> Orderitems { get; set; } = new List<Orderitem>();
}
