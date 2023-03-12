namespace EmployesWork.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Employe
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employe()
        {
            WorkDays = new HashSet<WorkDay>();
        }
        
        public int Id { get; set; }

        public int? PersonnelId { get; set; }
       
        public string Name { get; set; }

        public string Description { get; set; }

        public bool? ING { get; set; }

        public bool? Show { get; set; }
        public int? ShedulesId { get; set; }

        public virtual Shedule Shedule { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkDay> WorkDays { get; set; }
    }
}
