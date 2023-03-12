using EmployesWork.Domain;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace EmployesWork.WebUI.Models
{
    public class HelpClass
    {
        #region Дополнения
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

        internal void SetDays(NameValueCollection list, ModelBox box)
        {
            var dayIDS = list.GetValues("DayId");
            var empIDS = list.GetValues("EmployeId");
            var lits = list.GetValues("Literal");
            var sheds = list.GetValues("Shedule");
            var dates = list.GetValues("Date");

            for (int i = 0; i < dayIDS.Length; i++)
            {
                if (!string.IsNullOrEmpty(lits[i]))
                {

                    WorkDay wd = new WorkDay();
                    {
                        if (!string.IsNullOrEmpty(dayIDS[i]))
                            wd.DayId = Convert.ToInt32(dayIDS[i]);
                        wd.EmployeId = Convert.ToInt32(empIDS[i]);
                        wd.Literal = lits[i];
                        wd.Date = Convert.ToDateTime(dates[i]);

                    };

                    if (GetExit(lits[i]))
                        wd.ShedulesId = Convert.ToInt32(sheds[i]);
                    else
                        wd.ShedulesId = 6;

                    box.WRepository.Save(wd);
                }
                else if (string.IsNullOrEmpty(lits[i]) && !string.IsNullOrEmpty(dayIDS[i]))
                {
                    box.WRepository.Delete(Convert.ToInt32(dayIDS[i]));
                }
            }
        }

        internal void DeleteEmploye(int id, ModelBox box)
        {
            box.ERepository.Delete(id);
        }

        internal void VacationSet(int id, string literal, DateTime datestart, DateTime dateend, int idchanger, ModelBox box)
        {
            DateTime conDT = new DateTime(2016, 1, 1);
            Employe employe = box.Employes.Where(e => e.Id == id).FirstOrDefault();
            Employe changer = box.Employes.Where(e => e.Id == idchanger).FirstOrDefault();
            if (employe == null || GetExit(literal)) return;
            for (; datestart <= dateend; datestart = datestart.AddDays(1))
            {

                if (employe.WorkDays.Where(w => w.Date == datestart).FirstOrDefault() == null)
                {
                    WorkDay day = new WorkDay
                    {
                        Date = datestart,
                        EmployeId = id,
                        Literal = literal,
                        ShedulesId = 6
                    };
                    box.WRepository.Save(day);
                }
                else
                {
                    var index = employe.WorkDays.Where(w => w.Date == datestart).FirstOrDefault().DayId;
                    WorkDay day = employe.WorkDays.Where(w => w.DayId == index).FirstOrDefault();
                    day.Literal = literal;
                    day.ShedulesId = 6;
                    box.WRepository.Save(day);
                }

                if (changer == null) continue;
                int shedule = employe.ShedulesId.Value;
                string lit = "";
                int days = (datestart - conDT).Days % 4;
                if (shedule == 1)
                {
                    switch (datestart.DayOfWeek)
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


                if (changer.WorkDays.Where(w => w.Date == datestart).FirstOrDefault() == null)
                {
                    WorkDay day = new WorkDay
                    {
                        Date = datestart,
                        EmployeId = idchanger,
                        Literal = lit,
                        ShedulesId = GetExit(lit) ? shedule : 6
                    };
                    box.WRepository.Save(day);
                }
                else
                {
                    var index = changer.WorkDays.Where(w => w.Date == datestart).FirstOrDefault().DayId;
                    WorkDay day = changer.WorkDays.Where(w => w.DayId == index).FirstOrDefault();
                    day.Literal = lit;
                    day.EmployeId = idchanger;
                    day.ShedulesId = GetExit(lit) ? shedule : 6;
                    box.WRepository.Save(day);
                }


            }
        }
        public bool GetExit(string str)
        {
            double d;
            return double.TryParse(str, out d)|| double.TryParse(str.Replace(".", ","), out d) || double.TryParse(str.Replace(",", "."), out d) || str == "Д" || str == "Н";
        }
        internal void EmloyeSave(int id, int persID, string name, string desc, bool ing, int shedID, ModelBox box)
        {
            Employe employe = new Employe
            {
                Id = id,
                Name = name,
                Description = desc,
                PersonnelId = persID,
                ING = ing,
                ShedulesId = shedID
            };
            box.ERepository.Save(employe);
        } 
        #endregion

        #region CRUD SHEDULES
        public void SheduleSave(int id, string brig, string shed, ModelBox box)
        {
            Shedule shedule = new Shedule
            {
                Id = id,
                Brig = brig,
                Sheduler = shed
            };
            box.SRepository.Save(shedule);
        }
        public void SheduleDelete(int id, ModelBox box)
        {

            box.SRepository.Delete(id);
        } 
        #endregion
             
        #region Методы авто - заполнения
        public void FillMonthForConcreteEmploye(DateTime date, int idEmploye, ModelBox box)
        {
            DateTime start = new DateTime(date.Year, date.Month, 1);
            DateTime end = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
            DateTime conDT = new DateTime(2016, 1, 1);


            Employe empl = box.Employes.Where(e => e.Id == idEmploye).FirstOrDefault();
            if (empl == null||empl.Show==false) return;


            IEnumerable<WorkDay> wds = empl.WorkDays.Where(wdd => wdd.Date >= start && wdd.Date <= end);

            WorkDay wd = new WorkDay();

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

                    wd = new WorkDay();
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
                    wd = new WorkDay();
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
                    wd = new WorkDay();
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
                    wd = new WorkDay();
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
                    wd = new WorkDay();
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

                    if (start.DayOfWeek == DayOfWeek.Sunday || start.DayOfWeek == DayOfWeek.Saturday)
                    {
                        lit = "В";
                        wd = new WorkDay();
                        wd.EmployeId = empl.Id;
                        wd.Literal = lit;
                        wd.Date = start;
                        if (GetExit(lit))
                            wd.ShedulesId = empl.ShedulesId.Value;
                        else
                            wd.ShedulesId = 6;
                    }
                }

                if (wd.EmployeId == 0) continue;
                box.WRepository.Save(wd);

                
            }

        }



        internal void SaveDay(int did, DateTime date, string literal, int emplId, int shedId, ModelBox box)
        {
            WorkDay day = new WorkDay {
                DayId = did,
                Literal = literal,
                Date = date,
                EmployeId = emplId,
                ShedulesId = GetExit(literal) ? shedId : 6
            };
            box.WRepository.Save(day);
        }

        internal void DeleteDay(int did, ModelBox box)
        {
            box.WRepository.Delete(did);
        }

        internal void FillClearForConcreteEmploye(DateTime date, int id, ModelBox box)
        {
            DateTime start = new DateTime(date.Year, date.Month, 1);
            DateTime end = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
            box.Days.Where(d => d.Date >= start && d.Date <= end && d.EmployeId == id).Select(d => d.DayId).ToList().ForEach((d) => box.WRepository.Delete(d));
        }

        internal void FillClearForAllEmploye(DateTime date, ModelBox box)
        {
            DateTime start = new DateTime(date.Year, date.Month, 1);
            DateTime end = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
            box.Days.Where(d => d.Date >= start && d.Date <= end).Select(d => d.DayId).ToList().ForEach((d) => box.WRepository.Delete(d));
        }

        public void FillMonthForAllEmploye(DateTime date, ModelBox box)
        {
            box.Employes.Select(e => e.Id).ToList().ForEach((x) => FillMonthForConcreteEmploye(date, x, box));

        } 
        #endregion

        #region Методы для запросов

        public int GetExitDayCount(DateTime dt, IEnumerable<WorkDay> days)
            => days.Where(d => d.Date == dt && d.Employe.Show==true).Where(l => GetExit(l.Literal)).Count();

        public int GetExitDayCount(DateTime dt, IEnumerable<WorkDay> days, bool ing)
            => days.Where(d => d.Date == dt && d.Employe.Show == true).Where(l => GetExit(l.Literal) && l.Employe.ING == ing).Count();

        public int GetExitDayCount(DateTime dt, IEnumerable<WorkDay> days, int sheduleId)
            => days.Where(d => d.Date == dt && d.Employe.Show == true).Where(l => l.ShedulesId == sheduleId).Count();

        public int GetExitDayCount(DateTime dt, IEnumerable<WorkDay> days, int sheduleId, bool ing)
            => days.Where(d => d.Date == dt && d.Employe.Show == true).Where(l => l.ShedulesId == sheduleId && l.Employe.ING == ing).Count();


        public int GetNoExitDayCount(DateTime dt, IEnumerable<WorkDay> days)
            => days.Where(d => d.Date == dt && d.Employe.Show == true).Where(l => !GetExit(l.Literal)).Count();

        public string[] GetLiterals(IEnumerable<WorkDay> days)
            => days.Where(x => !GetExit(x.Literal)).Select(x => x.Literal).Distinct().ToArray();

        public Shedule[] GetShedules(Employe empl, DateTime dt, int day = 32)
            => empl.WorkDays.Where(x => x.Date.Month == dt.Month && x.Date.Year == dt.Year &&x.Date.Day <= day && x.Date <= DateTime.Now && x.ShedulesId != 6).Select(x => x.Shedule).Distinct().OrderBy(x => x.Id).ToArray();



        public int GetNoExitDayCount(DateTime dt, IEnumerable<WorkDay> days, string slit)
            => days.Where(d => d.Date == dt && d.Employe.Show == true).Where(l => !GetExit(l.Literal) && l.Literal == slit).Count();
        public int GetNoExitDayCount(DateTime dt, IEnumerable<WorkDay> days, bool ing)
            => days.Where(d => d.Date == dt && d.Employe.Show == true).Where(l => !GetExit(l.Literal) && l.Employe.ING == ing).Count();
        public int GetNoExitDayCount(DateTime dt, IEnumerable<WorkDay> days, string slit, bool ing)
            => days.Where(d => d.Date == dt && d.Employe.Show == true).Where(l => !GetExit(l.Literal) && l.Employe.ING == ing && l.Literal == slit).Count();




        public int GetCountExitForEmployeInMonth(DateTime dt, Employe empl, int shedid)
            => empl.WorkDays.Where(x => x.Date.Year == dt.Year && x.Date.Month == dt.Month && x.Date <= DateTime.Now && x.ShedulesId == shedid && GetExit(x.Literal)&& x.Employe.Show==true).Count();


        public int GetCountExitForEmployeInMonthAllItog(DateTime dt, Employe empl)
            => empl.WorkDays.Where(x => x.Date.Year == dt.Year && x.Date.Month == dt.Month && x.Date <= DateTime.Now && GetExit(x.Literal) && x.Employe.Show == true).Count();


        public double GetCountExitForEmployeInMonthHD(DateTime dt, Employe empl, int shedid)
        => empl
                .WorkDays
                .Where(x => x.Date.Year == dt.Year && x.Date.Month == dt.Month && x.Date <= DateTime.Now &&  x.ShedulesId == shedid && GetExit(x.Literal) && x.Employe.Show == true)
                .Select(x => GetExitHD(x.Literal)).Sum();

        public double GetCountExitForEmployeInMonthED(DateTime dt, Employe empl, int shedid)
        => empl
                .WorkDays
                .Where(x => x.Date.Year == dt.Year && x.Date.Month == dt.Month && x.Date <= DateTime.Now && x.ShedulesId == shedid && GetExit(x.Literal) && x.Employe.Show == true)
                .Select(x => GetExitED(x.Literal)).Sum();

        public double GetCountExitForEmployeInMonthND(DateTime dt, Employe empl, int shedid)
        => empl
                .WorkDays
                .Where(x => x.Date.Year == dt.Year && x.Date.Month == dt.Month && x.Date <= DateTime.Now && x.ShedulesId == shedid && GetExit(x.Literal) && x.Employe.Show == true)
                .Select(x => GetExitND(x.Literal)).Sum();

        public double GetCountExitForEmployeInMonthAll(DateTime dt, Employe empl, int shedid)
        => GetCountExitForEmployeInMonthHD(dt, empl, shedid) +
            GetCountExitForEmployeInMonthED(dt, empl, shedid) +
            GetCountExitForEmployeInMonthND(dt, empl, shedid);

        public double GetHoursExitForEmployeInMonthAllItog(DateTime dt, Employe empl)
            => empl.WorkDays
                .Where(x => x.Date.Year == dt.Year && x.Date.Month == dt.Month && x.Date <= DateTime.Now && x.Employe.Show == true)
            .Select(x => GetExitHD(x.Literal) + GetExitND(x.Literal) + GetExitED(x.Literal)).Sum();

        /**/


        public int GetCountExitForEmployeInMonth_Admin(DateTime dt, Employe empl, int shedid)
            => empl.WorkDays.Where(x => x.Date.Year == dt.Year && x.Date.Month == dt.Month && x.ShedulesId == shedid && GetExit(x.Literal) && x.Employe.Show == true).Count();


        public int GetCountExitForEmployeInMonthAllItog_Admin(DateTime dt, Employe empl)
            => empl.WorkDays.Where(x => x.Date.Year == dt.Year && x.Date.Month == dt.Month && GetExit(x.Literal) && x.Employe.Show == true).Count();


        public double GetCountExitForEmployeInMonthHD_Admin(DateTime dt, Employe empl, int shedid)
        => empl
                .WorkDays
                .Where(x => x.Date.Year == dt.Year && x.Date.Month == dt.Month && x.ShedulesId == shedid && GetExit(x.Literal) && x.Employe.Show == true)
                .Select(x => GetExitHD(x.Literal)).Sum();

        public double GetCountExitForEmployeInMonthED_Admin(DateTime dt, Employe empl, int shedid)
        => empl
                .WorkDays
                .Where(x => x.Date.Year == dt.Year && x.Date.Month == dt.Month && x.ShedulesId == shedid && GetExit(x.Literal) && x.Employe.Show == true)
                .Select(x => GetExitED(x.Literal)).Sum();

        public double GetCountExitForEmployeInMonthND_Admin(DateTime dt, Employe empl, int shedid)
        => empl
                .WorkDays
                .Where(x => x.Date.Year == dt.Year && x.Date.Month == dt.Month && x.ShedulesId == shedid && GetExit(x.Literal) && x.Employe.Show == true)
                .Select(x => GetExitND(x.Literal)).Sum();

        public double GetCountExitForEmployeInMonthAll_Admin(DateTime dt, Employe empl, int shedid)
        => GetCountExitForEmployeInMonthHD_Admin(dt, empl, shedid) +
            GetCountExitForEmployeInMonthED_Admin(dt, empl, shedid) +
            GetCountExitForEmployeInMonthND_Admin(dt, empl, shedid);

        public double GetHoursExitForEmployeInMonthAllItog_Admin(DateTime dt, Employe empl)
            => empl.WorkDays
                .Where(x => x.Date.Year == dt.Year && x.Date.Month == dt.Month && x.Employe.Show == true)
            .Select(x => GetExitHD(x.Literal) + GetExitND(x.Literal) + GetExitED(x.Literal)).Sum();



        public Shedule[] GetShedules_Admin(Employe empl, DateTime dt, int day = 32)
    => empl.WorkDays.Where(x => x.Date.Month == dt.Month && x.Date.Year == dt.Year && x.Date.Day <= day && x.ShedulesId != 6 && x.Employe.Show == true).Select(x => x.Shedule).Distinct().OrderBy(x => x.Id).ToArray();


        /**/

        public double GetExitHD(string str)
        {
            double d;
            if (double.TryParse(str, out d) || double.TryParse(str.Replace(".", ","), out d) || double.TryParse(str.Replace(",", "."), out d)) return d;
            else if (str == "Д") return 11;
            else if (str == "Н") return 1;
            return 0;
        }
        public double GetExitED(string str)
        {
            if (str == "Н") return 5;
            return 0;
        }
        public double GetExitND(string str)
        {
            if (str == "Н") return 7;
            return 0;
        }
        #endregion

        public string[] GetNoExistsLiterals(IEnumerable<WorkDay> days)
        {
            return days.Where(d=>!GetExit(d.Literal)).Select(d => d.Literal).Distinct().ToArray();
        }
        public int GetCountNoExistInMonth(DateTime dt, Employe empl, string literal)
        {
            return empl.WorkDays.Where(w => w.Literal == literal && w.Date.Year == dt.Year && w.Date.Month == dt.Month && w.Date<=DateTime.Now && w.Employe.Show == true).Count();
        }

        public int GetCountNoExistInMonth_Admin(DateTime dt, Employe empl, string literal)
        {
            return empl.WorkDays.Where(w => w.Literal == literal && w.Date.Year == dt.Year && w.Date.Month == dt.Month && w.Employe.Show == true).Count();
        }

    }
}