using BulkyBook.DataAccess.Data;
using BulkyBook.Models;
using BulkyBooky.DataAccess.Repository.IRepository;
using System.Linq;

namespace BulkyBooky.DataAccess.Repository
{
    public class CoverTypeRepository : Repository<CoverType>, ICoverTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public CoverTypeRepository(ApplicationDbContext db)
            : base(db)
        {
            _db = db;
        }

        public void Update(CoverType coverType)
        {
            CoverType objFromDb = _db.CoverTypes.FirstOrDefault(c => c.Id == coverType.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = coverType.Name;
            }
        }
    }
}
