using LibraryProjectForInterView.Models;

namespace LibraryProjectForInterView.Helper
{
    public class CommonFunction
    {
        private readonly LibraryDbContext _context;
        public CommonFunction(LibraryDbContext context)
        {
            _context = context;
        }
        #region RACK
        public bool RackExist(string code)
        {
            if (_context.Racks.Where(s => s.Code.Trim().ToLower() == code).Any())
                return true;
            else
                return false;
        }
        public bool Rack_EditExist(int RackId, string code)
        {
            if (_context.Racks.Where(s => s.Code.Trim().ToLower() == code && s.RackId != RackId).Any())
                return true;
            else
                return false;
        }
        #endregion
        #region Shelves
        public bool ShelfExist(int RackId, string code)
        {
            if (_context.Shelves.Where(s => s.Code.Trim().ToLower() == code && s.RackId == RackId).Any())
                return true;
            else
                return false;
        }
        public bool Shelf_EditExist(int shelfId, int RackId, string code)
        {
            if (_context.Shelves.Where(s => s.Code.Trim().ToLower() == code && s.RackId == RackId && s.ShelfId != shelfId).Any())
                return true;
            else
                return false;
        }
        #endregion
        #region Book
        public bool BookExist(string bookName = "", string AuthorName = "")
        {
            if (_context.Books.Where(s => s.BookName.Trim().ToLower() == bookName.Trim().ToLower() && s.Author == AuthorName.ToLower().Trim()).Any())
                return true;
            else
                return false;
        }
        public bool Book_EditExist(int BookId, string bookName = "", string AuthorName = "")
        {
            if (_context.Books.Where(s => s.BookName.Trim().ToLower() == bookName.Trim().ToLower() && s.Author == AuthorName.ToLower().Trim() && s.BookId != BookId).Any())
                return true;
            else
                return false;
        }
        #endregion
    }
}
