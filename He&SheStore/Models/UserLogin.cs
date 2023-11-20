using System;
using System.Collections.Generic;

namespace He_SheStore.Models;

public partial class UserLogin
{
    public decimal UserloginId { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public decimal? RoleId { get; set; }

    public decimal? CustomerId { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Role? Role { get; set; }
}
