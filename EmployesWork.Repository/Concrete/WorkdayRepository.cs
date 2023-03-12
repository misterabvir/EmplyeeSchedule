using EmployesWork.Domain;
using EmployesWork.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployesWork.Repository.Concrete
{
    public class WorkdayRepository : IWorkdayRepository
    {
        private TableContext context = new TableContext();
        public IQueryable<WorkDay> WorkDays => context.WorkDays;

        public bool Save(WorkDay day)
        {
            try
            {
                if (day == null) return false;

                var pr = context.WorkDays.Where(x => x.DayId == day.DayId).FirstOrDefault();
                if (pr == null)
                {
                    context.WorkDays.Add(day);
                }
                else
                {
                   
                    pr.Date = day.Date;
                    pr.Literal = day.Literal;
                    pr.ShedulesId = day.ShedulesId;
                    pr.EmployeId = day.EmployeId;

                }
                context.SaveChanges();
                return true;
            }
            catch
            {

                return false;
            }
        }

        public WorkDay Delete(int dayId)
        {
            WorkDay wd = WorkDays.FirstOrDefault(d => d.DayId == dayId);
            if (wd == null) return null;
            return Delete(wd);
        }

        private WorkDay Delete(WorkDay day)
        {
            if (day == null) return null;
            try
            {
                context.WorkDays.Remove(day);
                context.SaveChanges();
                return day;
            }
            catch
            {
                return null;
            }
        }

        //public bool Update(int dayOldID, WorkDay dayNew)
        //{
        //    WorkDay wd = WorkDays.FirstOrDefault(x => x.DayId == dayOldID);
        //    if (wd == null || dayNew == null) return false;
        //    return Update(wd, dayNew);
        //}

        //public bool Update(WorkDay dayOld, WorkDay dayNew)
        //{
        //    try
        //    {
        //        if (dayOld == null || dayNew == null) return false;
        //        dayOld.Date = dayNew.Date;
        //        dayOld.Literal = dayNew.Literal;
        //        context.SaveChanges();
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
    }
}
