using System;
using System.Collections.Generic;

namespace LibraryProjectForInterView.Models;

public partial class Rack
{
    public int RackId { get; set; }

    public string Code { get; set; } = null!;

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Shelf> Shelves { get; set; } = new List<Shelf>();
}
