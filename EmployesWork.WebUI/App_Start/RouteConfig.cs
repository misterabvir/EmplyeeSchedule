using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EmployesWork.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                name: "Main",
                url: "",
                defaults: new { controller = "Home", action = "Index" }
            );
            routes.MapRoute(
                name: "1",
                url: "Home",
                defaults: new { controller = "Home", action = "Index" }
            );
            routes.MapRoute(
                name: "2",
                url: "Admin",
                defaults: new { controller = "Admin", action = "Index" }
            );



            routes.MapRoute(
                name: "Vacation",
                url: "{controller}/VacationIndex/{id}/{year}/{month}",
                defaults: new { controller="Admin", action= "VacationIndex", id = "id", year="year", month="month" }
            );

            routes.MapRoute(
                name: "VacationIndexSet",
                url: "{controller}/VacationIndexSet",
                defaults: new { controller="Admin", action= "VacationIndexSet"}
            );

            routes.MapRoute(
                name: "Index_User_Page",
                url: "Home/Index",
                defaults: new { controller = "Home", action = "Index" }
            );

            routes.MapRoute(
                name: "FillMonthConcrete",
                url: "Admin/FillMonthConcrete/{id}/{year}/{month}",
                defaults: new { controller = "Admin", action = "FillMonthConcrete", id="id", year="year",month="month" }
            );


            routes.MapRoute(
                name: "FillClearConcrete",
                url: "Admin/FillClearConcrete/{id}/{year}/{month}",
                defaults: new { controller = "Admin", action = "FillClearConcrete", id="id", year="year",month="month" }
            );

            routes.MapRoute(
                name: "FillMonthAll",
                url: "Admin/FillMonthAll/{year}/{month}",
                defaults: new { controller = "Admin", action = "FillMonthAll", year="year",month="month" }
            );
            routes.MapRoute(
                name: "FillClearAll",
                url: "Admin/FillClearAll/{year}/{month}",
                defaults: new { controller = "Admin", action = "FillClearAll", year="year",month="month" }
            );

            routes.MapRoute(
                name: "Admin_Index_User_Page",
                url: "Admin/Index",
                defaults: new { controller = "Admin", action = "Index" }
            );

            routes.MapRoute(
                name: "Admin_Index_User_Page_shed",
                url: "Admin/EditShedules",
                defaults: new { controller = "Admin", action = "EditShedules" }
            );

            routes.MapRoute(
                name: "Admin_Index_User_Page_shed_concrete",
                url: "Admin/EditShedulesConcrete/{id}",
                defaults: new { controller = "Admin", action = "EditShedulesConcrete", id="id" }
            );

            routes.MapRoute(
                name: "Admin_Index_User_Page_shed_concrete_create",
                url: "Admin/CreateShedulesConcrete",
                defaults: new { controller = "Admin", action = "CreateShedulesConcrete"}
            );
            routes.MapRoute(
                name: "Admin_Index_User_Page_shed_del",
                url: "Admin/DeleteShedulesConcrete/{id}",
                defaults: new { controller = "Admin", action = "DeleteShedulesConcrete", id="id" }
            );
            routes.MapRoute(
                name: "Admin_Index_User_Page_profs",
                url: "Admin/EditProfiles",
                defaults: new { controller = "Admin", action = "EditProfiles" }
            );
            routes.MapRoute(
                name: "Admin_Index_User_Page_profs_create",
                url: "Admin/Create",
                defaults: new { controller = "Admin", action = "Create" }
            );

            routes.MapRoute(
                name: "Admin_Index_User_Page_profs_edit",
                url: "Admin/Edit/{id}",
                defaults: new { controller = "Admin", action = "Edit", id="id" }
            );

            routes.MapRoute(
                name: "Admin_Index_User_Page_profs_del",
                url: "Admin/Delete",
                defaults: new { controller = "Admin", action = "Delete" }
            );

            routes.MapRoute(
                name: "Index_User_Page_With_Date",
                url: "Home/Index/{year}/{month}",
                defaults: new { controller = "Home", action = "Index", year= "year", month="month"}
            );
            routes.MapRoute(
                name: "Month",
                url: "Home/Month/{year}/{month}",
                defaults: new { controller = "Home", action = "Month", year="year", month="month" }
            );
            routes.MapRoute(
                name: "Tables",
                url: "Home/Month/{year}/{month}",
                defaults: new { controller = "Home", action = "Tables", year="year", month="month" }
            );


            routes.MapRoute(
                name: "AMonth",
                url: "{controller}/FillMonth/{year}/{month}",
                defaults: new { controller = "Admin", action = "FillMonth", year="year", month="month" }
            );


            routes.MapRoute(
                name: "Day",
                url: "{controller}/Day/{year}/{month}/{day}",
                defaults: new { controller = "Admin", action = "Day", year="year", month="month", day="day" }
            );



            routes.MapRoute(
                name: "FillChangeDayConcreteRoute",
                url: "{controller}/FillChangeDayConcrete/{id}/{year}/{month}/{day}",
                defaults: new { controller = "Admin", action = "FillChangeDayConcrete", id="id", year="year", month="month", day="day" }
            );

            routes.MapRoute(
                null,
                "{controller}/{action}");

        }
    }
}
