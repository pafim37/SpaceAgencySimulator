using Microsoft.EntityFrameworkCore;
using Sas.Identity.Service.Models;

namespace Sas.Identity.Service.Data
{
    public interface IUserContext
    {
        DbSet<User> Users { get; set; }
    }
}
