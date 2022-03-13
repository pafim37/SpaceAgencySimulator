using Sas.Identity.Service.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Identity.Service.Data
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UserContext() : base()
        {
        }
    }
}
