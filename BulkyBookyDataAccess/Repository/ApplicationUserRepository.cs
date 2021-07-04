using BulkyBook.DataAccess.Data;
using BulkyBook.Models;
using BulkyBooky.DataAccess.Repository.IRepository;

namespace BulkyBooky.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;

        public ApplicationUserRepository(ApplicationDbContext db)
            : base(db)
        {
            _db = db;
        }
    }
}
