using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployesWork.WebUI2.Areas.Admin.Models
{
    public class HelpClass
    {
        public bool GetExit(string str)
        {
            double d;
            try
            {
   return double.TryParse(str, out d) || double.TryParse(str.Replace(".", ","), out d) || double.TryParse(str.Replace(",", "."), out d) || str == "Д" || str == "Н";
        
            }
            catch 
            {

                return false;
            }
         }

        public string NameOfMonth(int month)
        {
            switch (month)
            {
                case 1: return "Январь";
                case 2: return "Февраль";
                case 3: return "Март";
                case 4: return "Апрель";
                case 5: return "Май";
                case 6: return "Июнь";
                case 7: return "Июль";
                case 8: return "Август";
                case 9: return "Сентябрь";
                case 10: return "Октябрь";
                case 11: return "Ноябрь";
                case 12: return "Декабрь";
            }
            return "";
        }

        public void FillMonthForConcreteEmploye(DateTime date, int employeId, UAM_TABLE_DBEntities db)
        {
            DateTime start = new DateTime(date.Year, date.Month, 1);
            DateTime end = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
            DateTime conDT = new DateTime(2016, 1, 1);


            Employes empl = db.Employes.Where(e => e.Id == employeId).FirstOrDefault();
            if (empl == null || empl.Show == false) return;


            IEnumerable<WorkDays> wds = empl.WorkDays.Where(wdd => wdd.Date >= start && wdd.Date <= end);

            WorkDays wd = new WorkDays();

            for (; start <= end; start = start.AddDays(1))
            {
                string lit = "";
                if (wds.Where(wdd => wdd.Date == start).Count() != 0)
                {
                    start = start.AddDays(1);
                    continue;
                }
                int days = (start - conDT).Days % 4;
                if (empl.ShedulesId == 1)
                {
                    if (start.DayOfWeek == DayOfWeek.Sunday || start.DayOfWeek == DayOfWeek.Saturday)
                    {
                        lit = "В";
                    }
                    else if (start.DayOfWeek == DayOfWeek.Friday)
                    {
                        lit = "7,2";
                    }
                    else
                    {
                        lit = "8,2";
                    }

                    wd = new WorkDays();
                    wd.EmployeId = empl.Id;
                    wd.Literal = lit;
                    wd.Date = start;
                    if (GetExit(lit))
                        wd.ShedulesId = empl.ShedulesId.Value;
                    else
                        wd.ShedulesId = 6;
                }
                else if (empl.ShedulesId == 2)
                {

                    switch (days)
                    {
                        case 0: lit = "Д"; break;
                        case 2: lit = "Н"; break;
                        default: lit = "В"; break;
                    }
                    wd = new WorkDays();
                    wd.EmployeId = empl.Id;
                    wd.Literal = lit;
                    wd.Date = start;
                    if (GetExit(lit))
                        wd.ShedulesId = empl.ShedulesId.Value;
                    else
                        wd.ShedulesId = 6;

                }
                else if (empl.ShedulesId == 5)
                {

                    switch (days)
                    {
                        case 0: lit = "Н"; break;
                        case 2: lit = "Д"; break;
                        default: lit = "В"; break;
                    }
                    wd = new WorkDays();
                    wd.EmployeId = empl.Id;
                    wd.Literal = lit;
                    wd.Date = start;
                    if (GetExit(lit))
                        wd.ShedulesId = empl.ShedulesId.Value;
                    else
                        wd.ShedulesId = 6;

                }
                else if (empl.ShedulesId == 3)
                {

                    switch (days)
                    {
                        case 1: lit = "Н"; break;
                        case 3: lit = "Д"; break;
                        default: lit = "В"; break;
                    }
                    wd = new WorkDays();
                    wd.EmployeId = empl.Id;
                    wd.Literal = lit;
                    wd.Date = start;
                    if (GetExit(lit))
                        wd.ShedulesId = empl.ShedulesId.Value;
                    else
                        wd.ShedulesId = 6;

                }
                else if (empl.ShedulesId == 4)
                {

                    switch (days)
                    {
                        case 1: lit = "Д"; break;
                        case 3: lit = "Н"; break;
                        default: lit = "В"; break;
                    }
                    wd = new WorkDays();
                    wd.EmployeId = empl.Id;
                    wd.Literal = lit;
                    wd.Date = start;
                    if (GetExit(lit))
                        wd.ShedulesId = empl.ShedulesId.Value;
                    else
                        wd.ShedulesId = 6;

                }
                else
                {

                    if (start.DayOfWeek != DayOfWeek.Tuesday)
                    {
                        lit = "В";
                        wd = new WorkDays();
                        wd.EmployeId = empl.Id;
                        wd.Literal = lit;
                        wd.Date = start;
                        if (GetExit(lit))
                            wd.ShedulesId = empl.ShedulesId.Value;
                        else
                            wd.ShedulesId = 6;
                    }
                    else
                    {
                        wd = new WorkDays();
                        lit = "8,0";
                        wd = new WorkDays();
                        wd.EmployeId = empl.Id;
                        wd.Literal = lit;
                        wd.Date = start;
                        
                            wd.ShedulesId = empl.ShedulesId.Value;
                        
                    }
                }

                if (wd.EmployeId == 0) continue;

                db.WorkDays.Add(wd);
                db.SaveChanges();


            }

        }

        public void FillMonthForAllEmploye(DateTime date, UAM_TABLE_DBEntities db)
        {
            db.Employes.Select(e => e.Id).ToList().ForEach((x) => FillMonthForConcreteEmploye(date, x, db));

        }


        internal void ClearMonth(DateTime date, UAM_TABLE_DBEntities db)
        {
            DateTime start = new DateTime(date.Year, date.Month, 1);
            DateTime end = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
            db.WorkDays.Where(d => d.Date >= start && d.Date <= end).ToList().ForEach((d) => db.WorkDays.Remove(d));
            db.SaveChanges();
        }
    }
}