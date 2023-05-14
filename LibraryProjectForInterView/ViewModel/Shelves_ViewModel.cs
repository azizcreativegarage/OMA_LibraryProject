using LibraryProjectForInterView.Models;
using Microsoft.Build.Framework;

namespace LibraryProjectForInterView.ViewModel
{
    public class Shelves_ViewModel
    {
        public int? ShelfId { get; set; }
        [Required]
        public int RackId { get; set; }
        public string RackCode { get; set; }
        [Required]
        public string Code { get; set; }  

    }
}
