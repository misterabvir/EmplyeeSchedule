using EmployesWork.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployesWork.Repository.Abstract
{
    public interface IWorkdayRepository
    {
        IQueryable<WorkDay> WorkDays { get; }

        bool Save(WorkDay Day);
        //bool Update(WorkDay dayOld, WorkDay dayNew);
        //bool Update(int dayOldID, WorkDay dayNew);
        //WorkDay Delete(WorkDay day);
        WorkDay Delete(int dayId);
    }
}
