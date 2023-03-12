using EmployesWork.Repository.Abstract;
using EmployesWork.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace EmployesWork.WebUI.Controllers
{
    public class HomeController : Controller
    {
        #region Сущностные репозитории
        ModelBox box;

        public HomeController(IEmployeRepository employerRepository,
            IWorkdayRepository workdayRepository,
            ISheduleRepository sheduleRepository)
        {
            this.box = new ModelBox(employerRepository, workdayRepository, sheduleRepository);
        }
        #endregion

        #region Главная пользовательская страница
        /// <summary>
        /// Главная страница
        /// </summary>
        /// <param name="year">Год</param>
        /// <param name="month">Месяц</param>
        /// <returns></returns>





        public ViewResult Index(int? year, int? month)
        {

      

            ViewBag.Title = "Главная страница";

            ViewBag.Help = new HelpClass();
            try
            {

                if (year != null && month != null) return View(new DateTime(year.Value, month.Value, 1));
            }
            catch
            {

                return View(DateTime.Now);
            }
            return View(DateTime.Now);
        }
        #endregion



        public ActionResult Tables(int? year, int? month)
        {

            ViewBag.LengthMonth = DateTime.DaysInMonth(year.Value, month.Value);
            ViewBag.Date = new DateTime(year.Value, month.Value, 1);
            ViewBag.Title = new HelpClass().NameOfMonth(month.Value) + ", " + year.Value;
            ViewBag.Help = new HelpClass();
            return PartialView(box);
        }




        public ViewResult Month(int month, int year)
        {

            ViewBag.LengthMonth = DateTime.DaysInMonth(year, month);
            ViewBag.Date = new DateTime(year, month, 1);
            ViewBag.Title = new HelpClass().NameOfMonth(month) + ", " + year;
            ViewBag.Help = new HelpClass();
            return View(box);
        }

        

       
    }
}