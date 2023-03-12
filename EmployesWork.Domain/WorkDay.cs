namespace EmployesWork.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class WorkDay
    {
        [Key]
        public int DayId { get; set; }

        [StringLength(5)]
        public string Literal { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public int EmployeId { get; set; }

        public int ShedulesId { get; set; }

        public virtual Employe Employe { get; set; }

        public virtual Shedule Shedule { get; set; }
    }
}
