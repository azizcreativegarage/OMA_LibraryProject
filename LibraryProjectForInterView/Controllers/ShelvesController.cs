using LibraryProjectForInterView.Helper;
using LibraryProjectForInterView.Models;
using LibraryProjectForInterView.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibraryProject.Controllers
{
    public class ShelvesController : Controller
    {
        private readonly LibraryDbContext _context;
        private readonly CommonFunction _cf;

        private readonly IConfiguration _configuration;


        public ShelvesController(LibraryDbContext context, CommonFunction cf, IConfiguration configuration)
        {
            _context = context;
            _cf = cf;
            _configuration = configuration;
        }
        // GET: ShelvesController
        public async Task<ActionResult> Index()
        {
            #region
            var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand("proc_SelectAllShelves", conn);
            SqlDataAdapter adap = new SqlDataAdapter(cmd);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            // cmd.Parameters.AddWithValue("@VoucherMasterId", SqlDbType.Int).Value = int.Parse("1");
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

            List<Shelves_ViewModel> listShelves = new List<Shelves_ViewModel>();
            foreach (DataRow row in table.Rows)
            {
                var shelves = new Shelves_ViewModel();
                shelves.ShelfId = Convert.ToInt32(row["ShelfId"]);
                shelves.RackId = Convert.ToInt32(row["RackId"]);
                shelves.Code = row["Code"].ToString();
                shelves.RackCode = row["RackCode"].ToString();
                listShelves.Add(shelves);

            }
            #endregion


            return View(listShelves);

        }

        // GET: ShelvesController/Details/5
        public ActionResult Details(int ShelfId)
        {
            var shelfdetail = _context.Shelves.FromSql($"shelves_proc_shelfbyId @ShelfId= {ShelfId}").ToList();
            var data = shelfdetail.FirstOrDefault();
            Shelves_ViewModel shelfVm = new Shelves_ViewModel();
            shelfVm.RackId = data.RackId;
            shelfVm.ShelfId = data.ShelfId;
            shelfVm.Code = data.Code;
            return View(shelfVm);
        }

        // GET: ShelvesController/Create
        public ActionResult Create()
        {
            var Rack = _context.Racks.FromSqlRaw($"exec proc_SelectAllRacks").ToList().Select(s => new
            {
                RackId = s.RackId,
                Code = s.Code
            });

            ViewBag.RackDDL = new SelectList(Rack.Select(s => new { value = s.RackId, text = s.Code }).ToList(), "value", "text");

            return View();
        }

        // POST: ShelvesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Shelves_ViewModel shelf)
        {
            try
            {
                if (_cf.ShelfExist(shelf.RackId, shelf.Code.Trim().ToLower()))
                {
                    return Json(new { message = "Code Already Exist", returnStatus = 1 });
                }

                var resu = await _context.Database.ExecuteSqlAsync($"proc_CreateShelf @RackId={shelf.RackId} ,@Code= {shelf.Code}");
                return Json(new { message = "success", returnStatus = 0 });
            }
            catch (Exception ex)
            {
                return Json(new { message = "Error: " + ex.Message, returnStatus = 1 });
            }
        }

        // GET: ShelvesController/Edit/5
        public ActionResult Edit(int ShelfId)
        {



            var shelfdetail = _context.Shelves.FromSql($"shelves_proc_shelfbyId @ShelfId= {ShelfId}").ToList();
            var data = shelfdetail.FirstOrDefault();
            Shelves_ViewModel shelfVm = new Shelves_ViewModel();
            shelfVm.RackId = data.RackId;
            shelfVm.ShelfId = data.ShelfId;
            shelfVm.Code = data.Code;


            var Rack = _context.Racks.FromSqlRaw($"exec proc_SelectAllRacks").ToList().Select(s => new
            {
                RackId = s.RackId,
                Code = s.Code
            });
            ViewBag.RackDDL = new SelectList(Rack.Select(s => new { value = s.RackId, text = s.Code }).ToList(), "value", "text", shelfVm.RackId);

            return View(shelfVm);

        }

        // POST: ShelvesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Shelves_ViewModel shelf)
        {
            try
            {
                if (_cf.Shelf_EditExist(shelf.ShelfId.Value, shelf.RackId, shelf.Code))
                {
                    return Json(new { message = "Code Already Exist", returnStatus = 1 });
                }
                var resu = await _context.Database.ExecuteSqlAsync($"shelves_proc_shelfUpdate @ShelfId= {shelf.ShelfId}, @RackId= {shelf.RackId}, @Code={shelf.Code}");
                return Json(new { message = "success", returnStatus = 0 });
            }
            catch (Exception ex)
            {
                return Json(new { message = "Error: " + ex.Message, returnStatus = 1 });
            }
        }

        // GET: ShelvesController/Delete/5
        public ActionResult Delete(int ShelfId)
        {
            var shelfdetail = _context.Shelves.FromSql($"shelves_proc_shelfbyId @ShelfId= {ShelfId}").ToList();
            var data = shelfdetail.FirstOrDefault();
            Shelves_ViewModel shelfVm = new Shelves_ViewModel();
            shelfVm.RackId = data.RackId;
            shelfVm.ShelfId = data.ShelfId;
            shelfVm.Code = data.Code;
            return View(shelfVm);
        }

        // POST: ShelvesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Shelves_ViewModel shelf)
        {

            try
            {
                var resu = await _context.Database.ExecuteSqlAsync($"shelves_proc_shelfdelete @ShelfId= {shelf.ShelfId.Value}");
                return Json(new { message = "successfylly deleted", returnStatus = 0 });
            }
            catch (Exception ex)
            {
                return Json(new { message = "Error: " + ex.Message, returnStatus = 1 });
            }
        }
    }
}
