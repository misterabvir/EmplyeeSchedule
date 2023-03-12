using EmployesWork.Domain;
using EmployesWork.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployesWork.Repository.Concrete
{
    public class EmployeRepository: IEmployeRepository
    {
        private TableContext context = new TableContext();
        public IQueryable<Employe> Employes => context.Employes;


        public bool Save(Employe employe)
        {
            try
            {
                if (employe == null) return false;

                var pr = context.Employes.Where(x => x.Id == employe.Id).FirstOrDefault();
                if (pr == null)
                {
                    context.Employes.Add(employe);
                }
                else
                {
                    pr.Name = employe.Name;
                    pr.ING = employe.ING;
                    pr.ShedulesId = employe.ShedulesId;
                    pr.Description = employe.Description;
                    pr.PersonnelId = employe.PersonnelId;
                }
                context.SaveChanges();
                return true;
            }
            catch
            {

                return false;
            }
        }

        public Employe Delete(int employeID)
        {
            Employe em = Employes.FirstOrDefault(x => x.Id == employeID);
            if (em == null) return null;
            em.Show = em.Show ==true? false : true;
            context.SaveChanges();
            return em;
        }

        public Employe Delete(Employe employe)
        {
            try
            {
                context.Employes.Remove(employe);
                context.SaveChanges();
                return employe;
            }
            catch
            {
                return null;
            }
        }

    }
}

