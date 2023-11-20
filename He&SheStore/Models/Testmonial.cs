using System;
using System.Collections.Generic;

namespace He_SheStore.Models;

public partial class Testmonial
{
    public decimal Testmonialid { get; set; }

    public string? Status { get; set; }

    public string? Comment { get; set; }

    public decimal? CustomerId { get; set; }

    public virtual Customer? Customer { get; set; }
}
