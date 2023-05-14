using LibraryProjectForInterView.Models;

namespace LibraryProjectForInterView.ViewModel
{
    public class Book_ViewModel
    {
        public int? BookId { get; set; }
        public string Code { get; set; }
        public string BookName { get; set; } = null!;
        public string? Author { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsDeleted { get; set; }
        public double? Price { get; set; }
        public int? ShelfId { get; set; }
        public string ShelfCode { get; set; }
        public int? YearOfPublishing { get; set; }
    }
}
 