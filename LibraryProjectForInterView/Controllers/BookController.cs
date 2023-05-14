using LibraryProjectForInterView.Helper;
using LibraryProjectForInterView.Models;
using LibraryProjectForInterView.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;
using System.Net;

namespace LibraryProjectForInterView.Controllers
{
    public class BookController : Controller
    {
        private readonly LibraryDbContext _context;
        private readonly CommonFunction _cf;
        private readonly IConfiguration _configuration;
        public BookController(LibraryDbContext context, CommonFunction cf, IConfiguration configuration)
        {
            _context = context;
            _cf = cf;
            _configuration = configuration;
        }

        // GET: BookController

        public async Task<ActionResult> Index(string BookName = "nil", string AuthorName = "nil",
            int YearOfPublishing = 0, int ShelfId = 0, string IsDeleted = "NULL")
        {
            var Shelfddl = _context.Shelves.FromSqlRaw($"exec shelves_proc_SelectListForDDL").ToList().Select(s => new
            {
                ShelfId = s.ShelfId,
                Code = s.Code
            });
            ViewBag.ShelfDDL = new SelectList(Shelfddl.Select(s => new { value = s.ShelfId, text = s.Code }).ToList(), "value", "text");

            #region STore Procedure
            var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand("Book_proc_SelectAllBook", conn);
            SqlDataAdapter adap = new SqlDataAdapter(cmd);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            conn.Open();
            await cmd.ExecuteNonQueryAsync();
            DataSet set = new DataSet();
            DataRow rows;
            adap.Fill(set);
            DataTable table = new DataTable();
            table = set.Tables[0];
            conn.Close();
            int i = 0;
            int masterId = 0;
            List<Book_ViewModel> listBook = new List<Book_ViewModel>();
            foreach (DataRow row in table.Rows)
            {
                var bookdata = new Book_ViewModel();
                bookdata.ShelfId = Convert.ToInt32(row["ShelfId"]);
                bookdata.ShelfCode = row["ShelfCode"].ToString();
                bookdata.BookId = Convert.ToInt32(row["BookId"]);
                bookdata.BookName = row["BookName"].ToString();
                bookdata.Author = row["Author"].ToString();
                bookdata.Price = Convert.ToDouble(row["Price"]);
                bookdata.YearOfPublishing = Convert.ToInt32(row["YearOfPublishing"]);
                listBook.Add(bookdata);
            }
            #endregion




            #region Search Area
            if (BookName != "nil")
            {
                listBook = listBook.Where(b => b.BookName.ToLower().Trim() == BookName.ToLower().Trim()).ToList();
            }
            if (AuthorName != "nil")
            {
                listBook = listBook.Where(b => b.Author.ToLower().Trim() == AuthorName.ToLower().Trim()).ToList();
            }
            if (YearOfPublishing != 0)
            {
                listBook = listBook.Where(b => b.YearOfPublishing == YearOfPublishing).ToList();
            }
            if (ShelfId != 0)
            {
                listBook = listBook.Where(b => b.ShelfId == ShelfId).ToList();

            }
            if (IsDeleted != "NULL")
            {
                if (IsDeleted == "Deleted")
                    listBook = listBook.Where(b => b.IsDeleted == true).ToList();
                if (IsDeleted == "NotDeleted")
                    listBook = listBook.Where(b => b.IsDeleted == null).ToList();
            }
            #endregion
            return View(listBook);
        }

        // GET: BookController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BookController/Create
        public ActionResult Create()
        {
            var Shelfddl = _context.Shelves.FromSqlRaw($"exec shelves_proc_SelectListForDDL").ToList().Select(s => new
            {
                ShelfId = s.ShelfId,
                Code = s.Code
            });
            ViewBag.ShelfDDL = new SelectList(Shelfddl.Select(s => new { value = s.ShelfId, text = s.Code }).ToList(), "value", "text");




            return View();
        }

        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Book_ViewModel book)
        {
            try
            {
                if (_cf.ShelfExist(book.ShelfId.Value, book.BookName))
                {
                    return Json(new { message = "Book Already Exist", returnStatus = 1 });
                }
                var resu = await _context.Database.ExecuteSqlAsync($"Book_proc_CreateShelf  @Code= {book.Code},@BookName= {book.BookName},@Author={book.Author},@Price={book.Price},@ShelfId={book.ShelfId},@YearOfPublishing={book.YearOfPublishing},@IsAvailable={true}");
                return Json(new { message = "success", returnStatus = 0 });
            }
            catch (Exception ex)
            {
                return Json(new { message = "Error: " + ex.Message, returnStatus = 1 });
            }
        }

        // GET: BookController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BookController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BookController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BookController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
