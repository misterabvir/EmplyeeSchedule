//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConsoleApplication1
{
    using System;
    using System.Collections.Generic;
    
    public partial class WorkDay
    {
        public int DayId { get; set; }
        public string Literal { get; set; }
        public System.DateTime Date { get; set; }
        public int EmployeId { get; set; }
        public int ShedulesId { get; set; }
    
        public virtual Employe Employe { get; set; }
        public virtual Shedule Shedule { get; set; }
    }
}
