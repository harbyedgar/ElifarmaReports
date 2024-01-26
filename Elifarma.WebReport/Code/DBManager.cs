using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Elifarma.WebReport.Models;

namespace Elifarma.WebReport.Code
{
    public partial class DbManager : DbContext
    {
        public DbManager() : base("BASEDATOS") { }

        //Transaccionales
        public DbSet<ReportFile> ReportFile { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<ReportFile>().HasKey(o => o.idreport);
        }

    }
}