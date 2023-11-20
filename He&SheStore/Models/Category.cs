using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace He_SheStore.Models;

public partial class Category
{
    public decimal CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public string? CategoryDescription { get; set; }

    public string? CategoryImage { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

	[NotMapped]
	public IFormFile ImageFile { get; set; }
}
