using EmployesWork.Repository.Abstract;
using EmployesWork.Repository.Concrete;
using Ninject;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace EmployesWork.WebUI.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        private void AddBindings()
        {
            ninjectKernel.Bind<IEmployeRepository>().To<EmployeRepository>();
            ninjectKernel.Bind<IWorkdayRepository>().To<WorkdayRepository>();
            ninjectKernel.Bind<ISheduleRepository>().To<SheduleRepository>();
            ninjectKernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ?
                    null :
                    (IController)ninjectKernel.Get(controllerType);
        }
    }
}