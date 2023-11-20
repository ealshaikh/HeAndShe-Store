using System;
using System.Collections.Generic;

namespace He_SheStore.Models;

public partial class Card
{
    public decimal Cardid { get; set; }

    public long? CardNumber { get; set; }

    public byte? CardCvc { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public string? CardholderName { get; set; }

    public decimal? Balance { get; set; }

    public decimal? CustomerId { get; set; }

    public virtual Customer? Customer { get; set; }
}
