using EmployesWork.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployesWork.Repository.Abstract
{
    public interface ISheduleRepository
    {
        IQueryable<Shedule> Shedules { get; }

        bool Save(Shedule Day);
        Shedule Delete(int shedId);
    }
}
