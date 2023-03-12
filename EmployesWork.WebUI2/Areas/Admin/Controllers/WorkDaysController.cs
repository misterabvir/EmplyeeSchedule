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
using EmployesWork.WebUI2.Areas.Admin.Models;
using System.Threading;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using GemBox.Spreadsheet;

namespace EmployesWork.WebUI2.Areas.Admin.Controllers
{
    public class WorkDaysController : Controller
    {
        private UAM_TABLE_DBEntities db = new UAM_TABLE_DBEntities();

        Dictionary<string, string> pathes = new Dictionary<string, string>();

        public WorkDaysController()
        {
            
        }



        // GET: Admin/WorkDays
        public ActionResult Index(DateTime? date)
        {
            
            if (date == null)
                date = DateTime.Now;
           
            ViewBag.Date = date;
            var g = db.Shedules.Select(s => new { Id = s.Id, text = s.Brig + " " + s.Sheduler }).ToArray();

            ViewBag.ShedulesId = new SelectList(g, "Id", "text");
            return View();
        }


       
        public async Task<ActionResult> Tables(DateTime? date, bool? all)
        {
            IQueryable<WorkDays> workDays;
            if (date == null)
                date = DateTime.Now;
            if(all.GetValueOrDefault())
            workDays = db.WorkDays.Where(w => w.Date.Year == date.Value.Year && w.Date.Month == date.Value.Month).Include(w => w.Employes).Include(w => w.Shedules);
            else
                workDays = db.WorkDays.Where(w => w.Date.Year == date.Value.Year && w.Date.Month == date.Value.Month && w.Date <= DateTime.Now).Include(w => w.Employes).Include(w => w.Shedules);
            ViewBag.Date = date;
            ViewBag.Help = new HelpClass();
            var g = db.Shedules.Select(s => new { Id = s.Id, text = s.Brig + " " + s.Sheduler }).ToArray();

            ViewBag.Employes = db.Employes.OrderBy(o => o.Description).ToArray();




            ViewBag.ShedulesId = new SelectList(g, "Id", "text");
            return PartialView(await workDays.ToListAsync());
        }

        
        [HttpPost]
        public ActionResult Tables(int? EmployeId, int? DayId, DateTime? Date, string Literal, int? ShedulesId, bool? all)
        {

            if (EmployeId != null && DayId != null && Date != null && ShedulesId != null)
            {
                if (DayId.Value == 0)
                {
                    WorkDays day = new WorkDays();
                    day.Date = Date.Value;
                    day.DayId = DayId.Value;
                    day.EmployeId = EmployeId.Value;
                    day.Literal = Literal;

                    if (new HelpClass().GetExit(Literal))
                        day.ShedulesId = ShedulesId.Value;
                    else
                        day.ShedulesId = 6;
                    db.WorkDays.Add(day);
                }
                else
                {
                    var wd = db.WorkDays.Where(w => w.DayId == DayId).FirstOrDefault();
                    if (wd != null)
                    {
                        if (Literal != null && Literal != "")
                        {

                            wd.Literal = Literal;
                            if (new HelpClass().GetExit(Literal))
                                wd.ShedulesId = ShedulesId.Value;
                            else
                                wd.ShedulesId = 6;
                        }
                        else
                            db.WorkDays.Remove(wd);

                    }
                }
                db.SaveChanges();
            }

            return RedirectToAction("Tables", new { date = Date, all = all });
        }

     
        public ActionResult DayConcrete(DateTime date)
        {
            ViewBag.Date = date;
            ViewBag.Title = "Изменить выхода на ";
            ViewBag.Employes = db.Employes.OrderBy(o => o.Description).ToArray();
            ViewBag.WorkDays = db.WorkDays.Where(w => w.Date.Year == date.Year && w.Date.Month == date.Month && w.Date.Day == date.Day).ToArray();
            ViewBag.Shedules = db.Shedules.ToArray();
            var g = db.Shedules.Select(s => new { Id = s.Id, text = s.Brig + " " + s.Sheduler }).ToArray();
            ViewBag.ShedulesId = new SelectList(g, "Id", "text");
            return PartialView();
        }

     
        [HttpPost]
        public void DayConcreteResult(List<WorkDays> days)
        {
            foreach (var item in days)
            {
                if (item.DayId == 0 && item.Literal != null)
                {
                    db.WorkDays.Add(item);
                }
                else if (item.DayId != 0 && (item.Literal == null || item.Literal == ""))
                {
                    var d = db.WorkDays.Where(w => w.DayId == item.DayId).FirstOrDefault();
                    db.WorkDays.Remove(d);
                }
                else if (item.DayId != 0 && item.Literal != null)
                {
                    var d = db.WorkDays.Where(w => w.DayId == item.DayId).FirstOrDefault();
                    d.Literal = item.Literal;
                    d.ShedulesId =new HelpClass().GetExit(item.Literal)? item.ShedulesId:6;                    
                }
                db.SaveChanges();

            }

        }

        public ActionResult DayConcreteEm(DateTime date, int employeId)
        {
            ViewBag.Date = date;
            ViewBag.Title = "Изменить выход";
            ViewBag.Employe = db.Employes.Where(w=>w.Id == employeId).FirstOrDefault();
            ViewBag.WorkDays = db.WorkDays.Where(w => w.Date.Year == date.Year && w.Date.Month == date.Month && w.Date.Day == date.Day && w.EmployeId == employeId).FirstOrDefault();
            ViewBag.Shedules = db.Shedules.ToArray();

            return PartialView();
        }

        [HttpPost]
        public void DayConcreteEmResult(WorkDays day)
        {
            if (day.DayId == 0 && day.Literal != null)
            {
                db.WorkDays.Add(day);
            }
            else if (day.DayId != 0 && (day.Literal == null || day.Literal == ""))
            {
                var d = db.WorkDays.Where(w => w.DayId == day.DayId).FirstOrDefault();
                db.WorkDays.Remove(d);
            }
            else if (day.DayId != 0 && day.Literal != null)
            {
                var d = db.WorkDays.Where(w => w.DayId == day.DayId).FirstOrDefault();
                d.Literal = day.Literal;
                d.ShedulesId = new HelpClass().GetExit(day.Literal) ? day.ShedulesId : 6;
            }
            db.SaveChanges();
        }

    
        public ActionResult FillMonth(DateTime date)
        {
            new HelpClass().FillMonthForAllEmploye(date, db);
            return RedirectToAction("Tables", new { date = date });
        }

       
        public ActionResult ClearMonth(DateTime date)
        {
            new HelpClass().ClearMonth(date, db);
            return RedirectToAction("Tables", new { date = date });
        }

      
        public ActionResult Monthes(DateTime date)
        {
            ViewBag.Date = date;
            return PartialView();
        }

        public ActionResult MonthExists(int id, DateTime date)
        {
            ViewBag.Date = date;
            ViewBag.AllEmployes = db.Employes.Where(w => w.Id != id).ToArray();
            ViewBag.Employe = db.Employes.Where(w => w.Id == id).FirstOrDefault();
            ViewBag.Shedules = db.Shedules.ToArray();

            return PartialView();
        }

     
        public ActionResult MonthForEmploye(int id, DateTime date)
        {
            ViewBag.Date = date;
           
            ViewBag.Employe = db.Employes.Where(w => w.Id == id).FirstOrDefault();
            ViewBag.WorkDays = db.WorkDays.Where(w => w.Date.Year == date.Year && w.Date.Month == date.Month && w.EmployeId == id).ToArray();
            ViewBag.Shedules = db.Shedules.ToArray();
            return PartialView();
        }

       
        public void InShedule(int id, DateTime dateStart, DateTime dateEnd, int sheddid)
        {
            DateTime conDT = new DateTime(2016, 1, 1);
            Employes employe = db.Employes.Where(e => e.Id == id).FirstOrDefault();
            if (employe == null) return;
            for (; dateStart <= dateEnd; dateStart = dateStart.AddDays(1))
            {


               
                int shedule = sheddid;
                string lit = "";
                int days = (dateStart - conDT).Days % 4;
                if (shedule == 1)
                {
                    switch (dateStart.DayOfWeek)
                    {
                        case DayOfWeek.Friday: lit = "7.2"; break;
                        case DayOfWeek.Sunday:
                        case DayOfWeek.Saturday: lit = "В"; break;
                        default: lit = "8.2"; break;
                    }
                }
                else if (shedule == 2)
                {
                    switch (days)
                    {
                        case 0: lit = "Д"; break;
                        case 2: lit = "Н"; break;
                        default: lit = "В"; break;
                    }
                }
                else if (shedule == 3)
                {
                    switch (days)
                    {
                        case 1: lit = "Н"; break;
                        case 3: lit = "Д"; break;
                        default: lit = "В"; break;
                    }
                }
                else if (shedule == 4)
                {
                    switch (days)
                    {
                        case 1: lit = "Д"; break;
                        case 3: lit = "Н"; break;
                        default: lit = "В"; break;
                    }
                }
                else if (shedule == 5)
                {
                    switch (days)
                    {
                        case 0: lit = "Н"; break;
                        case 2: lit = "Д"; break;
                        default: lit = "В"; break;
                    }
                }
                else if (shedule == 9)
                {
                    switch (dateStart.DayOfWeek)
                    {
                        case DayOfWeek.Tuesday: lit = "8.0"; break;
                        default: lit = "В"; break;
                    }
                }

                if (employe.WorkDays.Where(w => w.Date == dateStart).FirstOrDefault() == null)
                {
                    WorkDays day = new WorkDays
                    {
                        Date = dateStart,
                        EmployeId = id,
                        Literal = lit,
                        ShedulesId = new HelpClass().GetExit(lit) ? shedule : 6
                    };
                    db.WorkDays.Add(day);
                    db.SaveChanges();
                }
                else
                {
                    var index = employe.WorkDays.Where(w => w.Date == dateStart).FirstOrDefault().DayId;
                    WorkDays day = employe.WorkDays.Where(w => w.DayId == index).FirstOrDefault();
                    day.Literal = lit;
                    day.EmployeId = id;
                    day.ShedulesId = new HelpClass().GetExit(lit) ? shedule : 6;
                    if (day.Literal == "") db.WorkDays.Remove(day);
                    db.SaveChanges();
                }


            }
        }

        public void AddNoExit(int id, DateTime dateStart, DateTime dateEnd, int secondid, string literal)
        {
            DateTime conDT = new DateTime(2016, 1, 1);
            Employes employe = db.Employes.Where(e => e.Id == id).FirstOrDefault();
            Employes changer = db.Employes.Where(e => e.Id == secondid).FirstOrDefault();
            if (employe == null || new HelpClass().GetExit(literal)) return;
            for (; dateStart <= dateEnd; dateStart = dateStart.AddDays(1))
            {

                if (employe.WorkDays.Where(w => w.Date == dateStart).FirstOrDefault() == null)
                {
                    WorkDays day = new WorkDays
                    {
                        Date = dateStart,
                        EmployeId = id,
                        Literal = literal,
                        ShedulesId = 6
                    };
                    db.WorkDays.Add(day);
                    db.SaveChanges();
                }
                else
                {
                    var index = employe.WorkDays.Where(w => w.Date == dateStart).FirstOrDefault().DayId;
                    WorkDays day = employe.WorkDays.Where(w => w.DayId == index).FirstOrDefault();
                    day.Literal = literal;
                    day.ShedulesId = 6;
                    db.SaveChanges();
                }

                if (changer == null) continue;
                int shedule = employe.ShedulesId.Value;
                string lit = "";
                int days = (dateStart - conDT).Days % 4;
                if (shedule == 1)
                {
                    switch (dateStart.DayOfWeek)
                    {
                        case DayOfWeek.Friday: lit = "7.2"; break;
                        case DayOfWeek.Sunday:
                        case DayOfWeek.Saturday: lit = "В"; break;
                        default: lit = "8.2"; break;
                    }
                }
                else if (shedule == 2)
                {
                    switch (days)
                    {
                        case 0: lit = "Д"; break;
                        case 2: lit = "Н"; break;
                        default: lit = "В"; break;
                    }
                }
                else if (shedule == 3)
                {
                    switch (days)
                    {
                        case 1: lit = "Н"; break;
                        case 3: lit = "Д"; break;
                        default: lit = "В"; break;
                    }
                }
                else if (shedule == 4)
                {
                    switch (days)
                    {
                        case 1: lit = "Д"; break;
                        case 3: lit = "Н"; break;
                        default: lit = "В"; break;
                    }
                }
                else if (shedule == 5)
                {
                    switch (days)
                    {
                        case 0: lit = "Н"; break;
                        case 2: lit = "Д"; break;
                        default: lit = "В"; break;
                    }
                }


                if (changer.WorkDays.Where(w => w.Date == dateStart).FirstOrDefault() == null)
                {
                    WorkDays day = new WorkDays
                    {
                        Date = dateStart,
                        EmployeId = secondid,
                        Literal = lit,
                        ShedulesId = new HelpClass().GetExit(lit) ? shedule : 6
                    };
                    db.WorkDays.Add(day);
                    db.SaveChanges();
                }
                else
                {
                    var index = changer.WorkDays.Where(w => w.Date == dateStart).FirstOrDefault().DayId;
                    WorkDays day = changer.WorkDays.Where(w => w.DayId == index).FirstOrDefault();
                    day.Literal = lit;
                    day.EmployeId = secondid;
                    day.ShedulesId = new HelpClass().GetExit(lit) ? shedule : 6;
                    
                    db.SaveChanges();
                }


            }
        }

        string path = "";

        public string ExcelCreatorStart(DateTime date, bool all)
        {
            
            try

            {
                pathes.Clear();
                pathes.Add("Для Бычина(входящие)", @"\\Fsdmz\work\Отделы\УАМ\Входящие\Для Бычина");
                pathes.Add("Для Фоменко(входящие)", @"\\Fsdmz\work\Отделы\УАМ\Входящие\Для Бычина");



                string name = new HelpClass().NameOfMonth(date.Month) + ", " + date.Year;
                string filename = "Табель выходов УАМ ТЭЦ-ПВС за " + name;
                int countDay = DateTime.DaysInMonth(date.Year, date.Month);
                Employes[] employes = db.Employes.OrderBy(o => o.Description).ToArray();
                WorkDays[] workDays;
                if (all) workDays = db.WorkDays.Where(w => w.Date.Year == date.Year && w.Date.Month == date.Month).ToArray();
                else workDays = db.WorkDays.Where(w => w.Date.Year == date.Year && w.Date.Month == date.Month && w.Date <= DateTime.Now).ToArray();
                var noexistsLit = workDays.Where(x => x.ShedulesId == 6).Select(x => x.Literal).Distinct().ToArray();
                Shedules[] shedules = db.Shedules.ToArray();
                #region creating
                SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
                // Server.MapPath("~/Files/PDFIcon.pdf");
                ExcelFile file = ExcelFile.Load(Server.MapPath("~/File/") + countDay + ".xls");

                ExcelWorksheet sheet = file.Worksheets.ActiveWorksheet;
                sheet.Name = name;

                var range = sheet.Cells[0, 0];
                range.Value = "Табель выходов УАМ ТЭЦ-ПВС за " + name;
                var ranges = sheet.Cells.GetSubrangeAbsolute(2, 2, 2, 2 + countDay);
                int count = 1;
                foreach (var item in ranges)
                {
                    if (count > countDay) break;
                    DateTime d = new DateTime(date.Year, date.Month, count);
                    if (d.DayOfWeek == DayOfWeek.Sunday || d.DayOfWeek == DayOfWeek.Saturday)
                    {
                        item.Style.FillPattern.SetSolid(SpreadsheetColor.FromName(ColorName.Red));
                        item.Style.Font.Color = SpreadsheetColor.FromName(ColorName.White);
                    }
                    item.Style.Font.Weight = 900;
                    count++;
                }
                int row = 1;
                int col = 10 + countDay;
                foreach (var item in noexistsLit)
                {
                    ranges = sheet.Cells.GetSubrangeRelative(row, col, 1, 2);
                    ranges.Merged = true;
                    ranges.Value = item;
                    ranges.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                    ranges.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                    ranges.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Medium);
                    ranges.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Medium);
                    col++;
                }





                row = 4;
                col = 0;

                foreach (var employe in employes)
                {
                    #region employes
                    var shedsem = workDays.Where(w => w.EmployeId == employe.Id && w.ShedulesId != 6).Select(s => s.Shedules).Distinct().ToArray();
                    var countsh = shedsem.Length;
                    countsh = countsh < 1 ? 1 : countsh;


                    ranges = sheet.Cells.GetSubrangeRelative(row, col, 1, countsh);
                    if (countsh > 1) ranges.Merged = true;
                    ranges.Value = employe.PersonnelId;
                    ranges.Style.Font.Weight = 900;
                    ranges.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                    ranges.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                    ranges.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                    ranges.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                    col++;

                    ranges = sheet.Cells.GetSubrangeRelative(row, col, 1, countsh);
                    if (countsh > 1) ranges.Merged = true;
                    ranges.Value = employe.Name;
                    ranges.Style.Font.Weight = 900;
                    ranges.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                    ranges.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                    ranges.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                    ranges.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                    if (employe.ING.GetValueOrDefault()) ranges.Style.Font.Color = SpreadsheetColor.FromName(ColorName.Red);
                    col++;
                    #endregion
                    #region literals
                    for (int i = 1; i <= countDay; i++)
                    {
                        DateTime d = new DateTime(date.Year, date.Month, i);
                        ranges = sheet.Cells.GetSubrangeRelative(row, col, 1, countsh);
                        if (countsh > 1) ranges.Merged = true;
                        WorkDays wd = workDays.Where(w => w.EmployeId == employe.Id && w.Date == d).FirstOrDefault();
                        string literal;
                        if (wd == null) literal = "";
                        else literal = wd.Literal;
                        ranges.Value = literal;
                        if (!new HelpClass().GetExit(literal)) ranges.Style.Font.Color = SpreadsheetColor.FromName(ColorName.Red);
                        else if (wd.ShedulesId == 1) ranges.Style.Font.Color = SpreadsheetColor.FromName(ColorName.Blue);
                        else if (wd.ShedulesId == 9) ranges.Style.Font.Color = SpreadsheetColor.FromName(ColorName.DarkBlue);
                        else ranges.Style.Font.Color = SpreadsheetColor.FromName(ColorName.Black);
                        if (!all && d.Year == DateTime.Now.Year && d.Month == DateTime.Now.Month && d.Day == DateTime.Now.Day)
                            ranges.Style.FillPattern.SetSolid(SpreadsheetColor.FromName(ColorName.Yellow));
                        ranges.Style.Font.Weight = 900;
                        ranges.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                        ranges.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                        ranges.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                        ranges.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                        col++;
                    }
                    #endregion
                    #region itogs in shedules
                    var itemdays = workDays.Where(w => w.EmployeId == employe.Id).Where(s => s.Date.Month == date.Date.Month && s.Date.Year == date.Year && s.ShedulesId != 6);
                    var allhour = 0d;
                    var allExist = 0d;

                    var column = col;
                    foreach (var shed in itemdays.Select(d => d.Shedules).Distinct())
                    {



                        range = sheet.Cells[row, col];
                        range.Value = shed.Brig + " Гр." + shed.Sheduler;
                        range.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                        range.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                        range.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                        range.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                        col++;

                        range = sheet.Cells[row, col];
                        var employerexists = itemdays.Where(s => s.ShedulesId == shed.Id).Count();
                        allExist += employerexists;
                        range.Value = employerexists == 0 ? "" : employerexists.ToString();
                        range.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                        range.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                        range.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                        range.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);

                        col++;

                        var dhour = itemdays.Where(s => s.ShedulesId == shed.Id).Select((s) =>
                        {
                            double hour;
                            if (double.TryParse(s.Literal, out hour) || double.TryParse(s.Literal.Replace(".", ","), out hour) || double.TryParse(s.Literal.Replace(",", "."), out hour)) return hour;
                            else if (s.Literal == "Д") return 11;
                            else if (s.Literal == "Н") return 1;
                            else return 0;
                        }).Sum();
                        range = sheet.Cells[row, col];
                        range.Value = dhour == 0 ? "" : dhour.ToString();
                        range.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                        range.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                        range.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                        range.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                        col++;

                        var ehour = itemdays.Where(s => s.ShedulesId == shed.Id).Select((s) =>
                        {
                            if (s.Literal == "Н") return 5;
                            else return 0;
                        }).Sum();
                        range = sheet.Cells[row, col];
                        range.Value = ehour == 0 ? "" : ehour.ToString();
                        range.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                        range.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                        range.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                        range.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                        col++;

                        var nhour = itemdays.Where(s => s.ShedulesId == shed.Id).Select((s) =>
                        {
                            if (s.Literal == "Н") return 7;
                            else return 0;
                        }).Sum();
                        range = sheet.Cells[row, col];
                        range.Value = nhour == 0 ? "" : nhour.ToString();
                        range.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                        range.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                        range.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                        range.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                        col++;

                        var employesHours = nhour + ehour + dhour;
                        allhour += employesHours;
                        range = sheet.Cells[row, col];
                        range.Value = employesHours == 0 ? "" : employesHours.ToString();
                        range.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                        range.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                        range.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                        range.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                        col++;


                        col = column;
                        if (countsh > 1) row++;
                    }
                    #endregion
                    #region itogslast
                    if (countsh > 1)
                        row -= countsh;
                    col += 6;
                    ranges = sheet.Cells.GetSubrangeRelative(row, col, 1, countsh);
                    if (countsh > 1) ranges.Merged = true;
                    ranges.Value = allExist == 0 ? "" : allExist.ToString();
                    ranges.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                    ranges.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                    ranges.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                    ranges.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                    col++;

                    ranges = sheet.Cells.GetSubrangeRelative(row, col, 1, countsh);
                    if (countsh > 1) ranges.Merged = true;
                    ranges.Value = allhour == 0 ? "" : allhour.ToString();
                    ranges.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                    ranges.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                    ranges.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                    ranges.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                    col++;


                    #endregion

                    for (int i = 0; i < noexistsLit.Count(); i++)
                    {
                        ranges = sheet.Cells.GetSubrangeRelative(row, col, 1, countsh);
                        if (countsh > 1) ranges.Merged = true;
                        var c = workDays.Where(w => w.EmployeId == employe.Id).Where(w => w.Literal == noexistsLit[i]).Count();
                        ranges.Value = c == 0 ? "" : c.ToString();
                        ranges.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                        ranges.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                        ranges.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                        ranges.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                        col++;


                    }
                    row += countsh == 1 ? 1 : countsh;
                    ExcelRow r = sheet.Rows[row];
                    r.Height = 100;
                    row++;
                    col = 0;

                }

                ranges = sheet.Cells.GetSubrangeRelative(row, col, 2, 1);
                ranges.Merged = true;
                ranges.Value = "Итого за сутки отработало";
                ranges.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                ranges.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                ranges.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Medium);
                ranges.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Medium);
                col += 2;

                for (int i = 1; i <= countDay; i++)
                {
                    DateTime d = new DateTime(date.Year, date.Month, i);
                    ranges = sheet.Cells.GetSubrangeRelative(row, col, 1, 1);
                    int exdays = workDays.Where(w => w.Date == d && w.ShedulesId != 6).Count();
                    ranges.Value = exdays;
                    if (!all && d.Year == DateTime.Now.Year && d.Month == DateTime.Now.Month && d.Day == DateTime.Now.Day)
                        ranges.Style.FillPattern.SetSolid(SpreadsheetColor.FromName(ColorName.Yellow));
                    ranges.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                    ranges.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                    ranges.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Medium);
                    ranges.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Medium);
                    col++;
                }

                col = 0;
                row += 2;

                #endregion



                
                if (all)
                {
                    path = pathes["Для Бычина(входящие)"];
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    path +="/" +  filename + "(план).xls";
                    file.Save(path);
                }
                else
                {
                    path = pathes["Для Фоменко(входящие)"] + (new HelpClass().NameOfMonth(date.Month)) + "/";
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    path += "/" + filename + ".xls";
                    
                }





            }
            catch (Exception exc)
            {

                return exc.Message;
            }
          

            /**/
            return "<a href='Workdays/Load?path="+path+"' target='_blank'>Документ сохранен. Щелкните, чтобы скачать.</a>";
        }

        public FileResult Load(string path)
        {
            string filename = path.Substring(path.LastIndexOf('\\'));

            return File(path, "application/vnd.ms-excel", filename);
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
