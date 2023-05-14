using System;
using System.Collections.Generic;

namespace LibraryProjectForInterView.Models;

public partial class Shelf
{
    public int ShelfId { get; set; }

    public int RackId { get; set; }

    public string Code { get; set; } = null!;

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();

    public virtual Rack Rack { get; set; } = null!;
}
