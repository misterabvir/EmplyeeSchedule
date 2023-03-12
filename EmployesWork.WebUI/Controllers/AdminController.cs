using EmployesWork.Domain;
using EmployesWork.Repository.Abstract;
using EmployesWork.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployesWork.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        ModelBox box;

        public AdminController(IEmployeRepository employerRepository,
            IWorkdayRepository workdayRepository,
            ISheduleRepository sheduleRepository)
        {
            this.box = new ModelBox(employerRepository, workdayRepository, sheduleRepository);
        }


        public ViewResult Index()
        {
          
            return View();
        }

        #region CRUD профили
        public ViewResult EditProfiles()
        {
            ViewBag.Title = "Редактирование данных";
            return View(box.Employes);
        }

        public ViewResult Edit(int? id)
        {
            ViewBag.Referer = HttpContext.Request.UrlReferrer;
            ViewBag.Shedules = box.Shedules;
            return View(box.Employes.Where(x => x.Id == id.Value).FirstOrDefault());
        }

        [HttpPost]
        public RedirectResult Edit()
        {
            int id = Convert.ToInt32(Request.Form.GetValues("EmployeId")[0]);
            bool ing = Request.Form.AllKeys.Contains("ING");
            string name = Request.Form.GetValues("Name")[0];
            string desc = Request.Form.GetValues("Description")[0];
            int shedID = Convert.ToInt32(Request.Form.GetValues("SheduleId")[0]);
            int persID = Convert.ToInt32(Request.Form.GetValues("PersonnelId")[0]);

            new HelpClass().EmloyeSave(id, persID, name, desc, ing, shedID, box);

            return Redirect("EditProfiles");
        }
        [HttpPost]
        public RedirectResult Delete()
        {
            int id = Convert.ToInt32(Request.Form.GetValues(0)[0]);
            new HelpClass().DeleteEmploye(id, box);
            return Redirect("EditProfiles");
        }

        public ActionResult Create()
        {
            ViewBag.Shedules = box.Shedules;
            return View("Edit", new Employe());
        } 
        #endregion


        #region CRUD графики
        public ViewResult EditShedules()
        {
            ViewBag.Title = "Редактирование графиков";
            return View(box.Shedules);
        }

        public ViewResult EditShedulesConcrete(int id)
        {
            ViewBag.Referer = HttpContext.Request.UrlReferrer;
            ViewBag.Title = "Редактирование графика";
            return View(box.Shedules.Where(x => x.Id == id).FirstOrDefault());
        }
        public ActionResult DeleteShedulesConcrete(int? id)
        {
            new HelpClass().SheduleDelete(id.Value, box);
            return RedirectToAction("EditShedules");
        }


        [HttpPost]
        public RedirectResult EditShedulesConcrete()
        {
            int id = Convert.ToInt32(Request.Form.GetValues("Id")[0]);
            string brig = Request.Form.GetValues("Brig")[0];
            string shed = Request.Form.GetValues("Sheduler")[0];

            new HelpClass().SheduleSave(id, brig, shed, box);

            return Redirect("EditShedules");
        }
        public ActionResult CreateShedulesConcrete()
        {
            ViewBag.Title = "Добавление графика";
            return View("EditShedulesConcrete", new Shedule());
        }
        #endregion

        #region Отпуска
        public ActionResult VacationIndex(int? id, int? year, int? month)
        {            
            
            {
                ViewBag.Date = new DateTime(year.Value, month.Value, 1);
                ViewBag.Id = id.Value;

            }
          

            return View(box);
        }
        [HttpPost]
        public ActionResult VacationIndexSet()
        {
            string str = "";
            foreach (var item in Request.Form.AllKeys)
            {
                str += item + ": " + Request.Form.GetValues(item)[0] + " | ";
            }
            try
            {
                DateTime datestart = Convert.ToDateTime(Request.Form.GetValues("startDay")[0]);
                DateTime dateend = Convert.ToDateTime(Request.Form.GetValues("endDay")[0]);
                int id = Convert.ToInt32(Request.Form.GetValues("employeId")[0]);
                int idchanger = Convert.ToInt32(Request.Form.GetValues("idChangerEmploye")[0]);
                string literal = Request.Form.GetValues("literal")[0];
                new HelpClass().VacationSet(id, literal, datestart, dateend, idchanger, box);
            }
            catch
            {
                DateTime date = Convert.ToDateTime(Request.Form.GetValues("Date")[0]);
                return Redirect("VacationIndex?id=" + Convert.ToInt32(Request.Form.GetValues("employeId")[0])+"&year="+ date.Year+"&month="+date.Month );
            }

            DateTime dt = Convert.ToDateTime(Request.Form.GetValues("Date")[0]);
            return RedirectToAction("FillMonth", new { year = dt.Year, month = dt.Month });
        }
        #endregion

        #region FILLS
        public ViewResult FillMonth(int? year, int? month)
        {
            if (year == null || month == null)
            {
                ViewBag.Date = DateTime.Now;
                year = DateTime.Now.Year;
                month = DateTime.Now.Month;
            }
            else
                ViewBag.Date = new DateTime(year.Value, month.Value, 1);

            ViewBag.LengthMonth = DateTime.DaysInMonth(year.Value, month.Value);
            ViewBag.Title = new HelpClass().NameOfMonth(month.Value) + ", " + year;
            ViewBag.Help = new HelpClass();
            return View(box);
        }

        public ActionResult FillMonthConcrete(int? id, int? year, int? month)
        {

            if (id == null || year == null || month == null) return Redirect("Admin/FillMonth/");

            
            int ID = id.Value;

            new HelpClass().FillMonthForConcreteEmploye(new DateTime(year.Value, month.Value, 1), ID, box);

            return RedirectToAction("FillMonth", new { year = year.Value, month = month.Value });
        }

        public ActionResult FillClearConcrete(int? id, int? year, int? month)
        {
            if (id == null || year == null || month == null) return Redirect("Admin/FillMonth/");
            int ID = id.Value;
            new HelpClass().FillClearForConcreteEmploye(new DateTime(year.Value, month.Value, 1), ID, box);
            return RedirectToAction("FillMonth", new { year = year.Value, month = month.Value });
        } 


        public ActionResult FillMonthAll(int? year, int? month)
        {
            if (year == null || month == null) return RedirectToAction("FillMonth", new { year = DateTime.Now.Year, month = DateTime.Now.Month });
            new HelpClass().FillMonthForAllEmploye(new DateTime(year.Value, month.Value, 1), box);
            return RedirectToAction("FillMonth", new { year = year.Value, month = month.Value });
        }

        public ActionResult FillClearAll(int? year, int? month)
        {
            if (year == null || month == null) return RedirectToAction("FillMonth", new { year = DateTime.Now.Year, month = DateTime.Now.Month });
            new HelpClass().FillClearForAllEmploye(new DateTime(year.Value, month.Value, 1), box);
            return RedirectToAction("FillMonth", new { year = year.Value, month = month.Value });
        }






        public ActionResult FillChangeDayConcrete(int? ID, int? year, int? month, int? day)
        {
           


            if (ID == null || year== null || month == null || day == null) return Redirect("FillMonth");

            DateTime ddt = new DateTime(year.Value, month.Value, day.Value);
            ViewBag.Date = ddt;
            ViewBag.Box = box;
            int id = ID.Value;           
            return View(box.Employes.Where(w=>w.Id== id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult FillChangeDayConcrete()
        {
            DateTime dt = DateTime.Now;
            try
            {
                string date = Request.Form.AllKeys.Contains("Date") ? Request.Form.GetValues("Date")[0] : "empty";
                dt = Convert.ToDateTime(date);
                string literal = !Request.Form.AllKeys.Contains("Literal") ? "empty" : Request.Form.GetValues("Literal")[0] != "" ? Request.Form.GetValues("Literal")[0] : "empty";
                string dayId = Request.Form.AllKeys.Contains("DayId") ? Request.Form.GetValues("DayId")[0] : "empty";

                if (literal == "empty" && dayId == "empty")
                    return RedirectToAction("FillMonth", new { year = dt.Year, month = dt.Month });


                if (literal == "empty" && dayId != "empty")
                {
                    int did = Convert.ToInt32(dayId);

                    new HelpClass().DeleteDay(did, box);
                    return RedirectToAction("FillMonth", new { year = dt.Year, month = dt.Month });
                }

                int Did = 0; int.TryParse(dayId, out Did);



                string emplId = Request.Form.AllKeys.Contains("EmployeId") ? Request.Form.GetValues("EmployeId")[0] : "empty";
                int Eid = Convert.ToInt32(emplId);
                string shedId = Request.Form.AllKeys.Contains("Shedule") ? Request.Form.GetValues("Shedule")[0] : "empty";
                int Sid = Convert.ToInt32(shedId);



                new HelpClass().SaveDay(Did, dt, literal, Eid, Sid, box);

                return RedirectToAction("FillMonth", new { year = dt.Year, month = dt.Month });
            }
            catch (Exception)
            {

                return RedirectToAction("FillMonth", new { year = dt.Year, month = dt.Month });
            }



        }

        #endregion
        public ViewResult Day(int month, int year, int day)
        {
            ViewBag.Date = new DateTime(year, month, day);
            ViewBag.Title = new DateTime(year, month, day).ToLongDateString();
            return View(box);
        }
        [HttpPost]
        public ActionResult Day()
        {
            int year = Convert.ToDateTime(Request.Form.GetValues("Date")[0]).Year;
            int month = Convert.ToDateTime(Request.Form.GetValues("Date")[0]).Month;
            new HelpClass().SetDays(Request.Form, box);
            return RedirectToAction("FillMonth", new { year = year, month = month});
        }
    }
}