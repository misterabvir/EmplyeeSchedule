//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EmployesWork.WebUI2
{
    using System;
    using System.Collections.Generic;
    
    public partial class Employes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employes()
        {
            this.WorkDays = new HashSet<WorkDays>();
        }
    
        public int Id { get; set; }
        public Nullable<int> PersonnelId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<bool> ING { get; set; }
        public Nullable<int> ShedulesId { get; set; }
        public Nullable<bool> Show { get; set; }
    
        public virtual Shedules Shedules { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkDays> WorkDays { get; set; }
    }
}
