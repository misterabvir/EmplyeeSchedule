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
    public class ShedulesController : Controller
    {
        private UAM_TABLE_DBEntities db = new UAM_TABLE_DBEntities();

        // GET: Admin/Shedules
        public ActionResult Index()
        {
            return View();
        }


        public async Task<ActionResult> Main()
        {
            return PartialView(await db.Shedules.ToListAsync());
        }



        // GET: Admin/Shedules/Create
        public ActionResult Create()
        {
            return PartialView();
        }


        [HttpPost]
        
        public async Task<string> Create([Bind(Include = "Id,Brig,Sheduler")] Shedules shedules)
        {
            
                db.Shedules.Add(shedules);
                await db.SaveChangesAsync();


            return "";
            
        }

        // GET: Admin/Shedules/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shedules shedules = await db.Shedules.FindAsync(id);
            if (shedules == null)
            {
                return HttpNotFound();
            }
            return PartialView(shedules);
        }

        // POST: Admin/Shedules/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       
        public async Task<string> Edit([Bind(Include = "Id,Brig,Sheduler")] Shedules shedules)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shedules).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return "";
            }
            return "";
        }


        public async Task<string> DeleteConfirmed(int id)
        {
            Shedules shedules = await db.Shedules.FindAsync(id);
            db.Shedules.Remove(shedules);
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
