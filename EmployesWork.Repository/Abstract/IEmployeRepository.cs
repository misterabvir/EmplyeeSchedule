using EmployesWork.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployesWork.Repository.Abstract
{
    public interface IEmployeRepository
    {
        IQueryable<Employe> Employes { get; }
        bool Save(Employe employe);
        //Employe Delete(Employe employe);
        Employe Delete(int employeID);
        //bool Update(Employe employeOld, Employe employeNew);
        //bool Update(int employeId, Employe employeNew);
    }
}
