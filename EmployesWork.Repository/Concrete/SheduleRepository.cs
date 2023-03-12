using EmployesWork.Domain;
using EmployesWork.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployesWork.Repository.Concrete
{
    public class SheduleRepository : ISheduleRepository
    {
        private TableContext context = new TableContext();
        public IQueryable<Shedule> Shedules => context.Shedules;

        public Shedule Delete(int shedId)
        {
            Shedule shed = Shedules.Where(s => s.Id == shedId).FirstOrDefault();
            if (shed == null) return null;
            return Delete(shed);
        }

        private Shedule Delete(Shedule shed)
        {
            if (shed == null) return null;
            try
            {
                context.Shedules.Remove(shed);
                context.SaveChanges();
                return shed;
            }
            catch
            {
                return null;
            }
        }


        public bool Save(Shedule shed)
        {
            try
            {
                if (shed == null) return false;

                var pr = context.Shedules.Where(x => x.Id == shed.Id).FirstOrDefault();
                if (pr == null)
                {
                    context.Shedules.Add(shed);
                }
                else
                {
                    pr.Brig = shed.Brig;
                    pr.Sheduler = shed.Sheduler;
                    

                }
                context.SaveChanges();
                return true;
            }
            catch
            {

                return false;
            }
        }
    }
}
