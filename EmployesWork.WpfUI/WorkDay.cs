//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EmployesWork.WpfUI
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
