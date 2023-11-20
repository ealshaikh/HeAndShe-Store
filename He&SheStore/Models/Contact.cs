using System;
using System.Collections.Generic;

namespace He_SheStore.Models;

public partial class Contact
{
    public decimal ContactId { get; set; }

    public string? Messeage { get; set; }

    public string? Email { get; set; }

    public string? Fullname { get; set; }

    public string? Title { get; set; }
}
