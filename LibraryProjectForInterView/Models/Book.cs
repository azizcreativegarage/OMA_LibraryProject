using System;
using System.Collections.Generic;

namespace LibraryProjectForInterView.Models;

public partial class Book
{
    public int BookId { get; set; }

    public string Code { get; set; } = null!;

    public string BookName { get; set; } = null!;

    public string? Author { get; set; }

    public bool IsAvailable { get; set; }

    public bool? IsDeleted { get; set; }

    public double? Price { get; set; }

    public int? ShelfId { get; set; }

    public int? YearOfPublishing { get; set; }

    public virtual Shelf? Shelf { get; set; }
}
