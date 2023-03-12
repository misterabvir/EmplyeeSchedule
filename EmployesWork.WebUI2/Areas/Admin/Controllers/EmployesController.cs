using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EmployesWork.WebUI2;

namespace EmployesWork.WebUI2.Areas.Admin.Controllers
{
    public class EmployesController : Controller
    {
        private UAM_TABLE_DBEntities db = new UAM_TABLE_DBEntities();

        // GET: Admin/Employes
        public ActionResult Index()
        {
            

            return View();
        }

        public async Task<ActionResult> Main()
        {
            var employes = db.Employes.Include(e => e.Shedules);

            return PartialView(await employes.OrderBy(o => o.Description).ToListAsync());
        }


        // GET: Admin/Employes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employes employes = await db.Employes.FindAsync(id);
            if (employes == null)
            {
                return HttpNotFound();
            }

            return PartialView(employes);
        }

        // GET: Admin/Employes/Create
        public ActionResult Create()
        {

            
                var g = db.Shedules.Where(s=>s.Id!=6).Select(s=>new { Id = s.Id, text=s.Brig + " " +s.Sheduler}).ToArray();

            ViewBag.ShedulesId = new SelectList(g, "Id", "text");



            return PartialView();
        }
        [HttpPost]
        public async Task<string> Create([Bind(Include = "Id,PersonnelId,Name,Description,ING,ShedulesId,Show")] Employes employes)
        {
            if (ModelState.IsValid)
            {
                db.Employes.Add(employes);
                await db.SaveChangesAsync();
                return "";
            }

           
            return "";
        }

        // GET: Admin/Employes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employes employes = await db.Employes.FindAsync(id);
            if (employes == null)
            {
                return HttpNotFound();
            }
            var g = db.Shedules.Where(s => s.Id != 6).Select(s => new { Id = s.Id, Brig = s.Brig + " " + s.Sheduler }).ToArray();

            ViewBag.ShedulesId = new SelectList(g, "Id", "Brig", employes.ShedulesId);
            return PartialView(employes);
        }

        // POST: Admin/Employes/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,PersonnelId,Name,Description,ING,ShedulesId,Show")] Employes employes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employes).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ShedulesId = new SelectList(db.Shedules, "Id", "Brig", employes.ShedulesId);
            return PartialView(employes);
        }

        // GET: Admin/Employes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employes employes = await db.Employes.FindAsync(id);
            if (employes == null)
            {
                return HttpNotFound();
            }
            return PartialView(employes);
        }

        [HttpPost]
        public async Task<string> DeleteConfirmed(int id)
        {
            Employes employes = await db.Employes.FindAsync(id);
            db.Employes.Remove(employes);
            await db.SaveChangesAsync();
            return "";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
