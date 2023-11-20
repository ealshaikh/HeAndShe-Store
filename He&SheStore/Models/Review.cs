using System;
using System.Collections.Generic;

namespace He_SheStore.Models;

public partial class Review
{
    public decimal Reviewid { get; set; }

    public decimal? ProductId { get; set; }

    public string? Comment { get; set; }

    public decimal? Reating { get; set; }

    public decimal? CustomerId { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Product? Product { get; set; }
}
