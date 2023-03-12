namespace EmployesWork.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Shedule
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Shedule()
        {
            Employes = new HashSet<Employe>();
            WorkDays = new HashSet<WorkDay>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Brig { get; set; }

        [Required]
        [StringLength(50)]
        public string Sheduler { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Employe> Employes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkDay> WorkDays { get; set; }
    }
}
