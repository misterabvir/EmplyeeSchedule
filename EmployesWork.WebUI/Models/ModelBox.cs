using EmployesWork.Domain;
using EmployesWork.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployesWork.WebUI.Models
{
    public class ModelBox
    {
        private IEmployeRepository employerRepository;
        private IWorkdayRepository workdayRepository;
        private ISheduleRepository sheduleRepository;
        public ModelBox(IEmployeRepository employerRepository,
            IWorkdayRepository workdayRepository,
            ISheduleRepository sheduleRepository)
        {
            this.employerRepository = employerRepository;
            this.workdayRepository = workdayRepository;
            this.sheduleRepository = sheduleRepository;

        }

        public IEnumerable<Employe> Employes => employerRepository.Employes;
        public IEnumerable<WorkDay> Days => workdayRepository.WorkDays;
        public IEnumerable<Shedule> Shedules => sheduleRepository.Shedules;
        public IEmployeRepository ERepository => employerRepository;
        public IWorkdayRepository WRepository => workdayRepository;
        public ISheduleRepository SRepository => sheduleRepository;


    }
}