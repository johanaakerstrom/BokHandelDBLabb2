using System;
using System.Collections.Generic;

namespace BokHandelDBLabb2.Models;

public partial class Book
{
    public string Isbn { get; set; } = null!;

    public string? Title { get; set; }

    public string? Language { get; set; }

    public decimal? Price { get; set; }

    public DateTime? PublicationDate { get; set; }

    public int? AuthorsId { get; set; }

    public int? GenreId { get; set; }

    public virtual Author? Authors { get; set; }

    public virtual Genre? Genre { get; set; }

    public virtual ICollection<InventoryBalance> InventoryBalances { get; set; } = new List<InventoryBalance>();
}
