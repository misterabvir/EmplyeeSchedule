﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class UAM_TABLE_DBEntities : DbContext
    {
        public UAM_TABLE_DBEntities()
            : base("name=UAM_TABLE_DBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Employes> Employes { get; set; }
        public virtual DbSet<Shedules> Shedules { get; set; }
        public virtual DbSet<WorkDays> WorkDays { get; set; }
    }
}
