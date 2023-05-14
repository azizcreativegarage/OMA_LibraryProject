using LibraryProjectForInterView.Helper;
using LibraryProjectForInterView.Models;
using LibraryProjectForInterView.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Model;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibraryProjectForInterView.Controllers
{
    public class RacksController : Controller
    {
        private readonly LibraryDbContext _context;
        private readonly CommonFunction _cf;
        public RacksController(LibraryDbContext context, CommonFunction cf)
        {
            _context = context;
            _cf = cf;
        }
        public ActionResult Index()
        {
            var racks = _context.Racks.FromSqlRaw($"exec proc_SelectAllRacks").ToList().Select(s => new Rack_ViewModel
            {
                RackId = s.RackId,
                Code = s.Code
            });
            return View(racks);
        }
        [HttpPost]
        public ActionResult Index(Rack_ViewModel rvm)
        {


            return View();
        }

        // GET: RacksController/Details/5
        public ActionResult Details(int RackId)
        {
            var racdetai = _context.Racks.FromSql($"proc_racbyId {@RackId = RackId}").ToList();
            var racdetail = racdetai.FirstOrDefault();
            Rack_ViewModel rackVM = new Rack_ViewModel();
            rackVM.RackId = racdetail.RackId;
            rackVM.Code = racdetail.Code;
            return View(racdetail);
        }

        // GET: RacksController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RacksController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Rack_ViewModel rack)
        {
            try
            {
                if (_cf.RackExist(rack.Code.Trim().ToLower()))
                {
                    return Json(new { message = "Code Already Exist", returnStatus = 1 });
                }

                var resu = await _context.Database.ExecuteSqlAsync($"proc_CreateRack @Code= {rack.Code}");
                return Json(new { message = "success", returnStatus = 0 });
            }
            catch (Exception ex)
            {
                return Json(new { message = "Error: " + ex.Message, returnStatus = 1 });
            }
        }

        // GET: RacksController/Edit/5
        public ActionResult Edit(int RackId)
        {
            var racdetai = _context.Racks.FromSql($"proc_racbyId @RackId= {RackId}").ToList();
            var data = racdetai.FirstOrDefault();
            Rack_ViewModel rackVM = new Rack_ViewModel();
            rackVM.RackId = data.RackId;
            rackVM.Code = data.Code;
            return View(rackVM);
        }

        // POST: RacksController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Rack_ViewModel data)
        {
            try
            {
                if (_cf.Rack_EditExist(data.RackId.Value, data.Code))
                {
                    return Json(new { message = "Code Already Exist", returnStatus = 1 });
                }
                var resu = await _context.Database.ExecuteSqlAsync($"proc_UpdateRack @RackId= {data.RackId.Value}, @Code={data.Code}");
                return Json(new { message = "success", returnStatus = 0 });
            }
            catch (Exception ex)
            {
                return Json(new { message = "Error: " + ex.Message, returnStatus = 1 });
            }
        }
        // GET: RacksController/Delete/5
        public ActionResult Delete(int RackId)
        {
            var racdetai = _context.Racks.FromSql($"proc_racbyId @RackId= {RackId}").ToList();
            var data = racdetai.FirstOrDefault();
            Rack_ViewModel rackVM = new Rack_ViewModel();
            rackVM.RackId = data.RackId;
            rackVM.Code = data.Code;
            return View(rackVM);

        }
        // POST: RacksController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Rack_ViewModel rack_ViewModel)
        {
            try
            {
                var resu = await _context.Database.ExecuteSqlAsync($"proc_DeleteRack @RackId= {rack_ViewModel.RackId.Value}");
                return Json(new { message = "successfylly deleted", returnStatus = 0 });
            }
            catch (Exception ex)
            {
                return Json(new { message = "Error: " + ex.Message, returnStatus = 1 });
            }
        }
    }
}
