using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ZemogaModel_Repository : ZemogaDBEntities
    {
        public ZemogaModel_Repository() {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public System.Data.Entity.DbSet<Repository.Get_Employees_Result> Get_Employees_Result { get; set; }
    }
}
