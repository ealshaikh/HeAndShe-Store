using System;
using System.Collections.Generic;

namespace He_SheStore.Models;

public partial class Orderaddress
{
    public decimal OrderAddressId { get; set; }

    public string Fname { get; set; } = null!;

    public string Lname { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Streetnumber { get; set; } = null!;

    public string Postalcode { get; set; } = null!;

    public string Phonenumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
