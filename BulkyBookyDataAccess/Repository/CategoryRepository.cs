using BulkyBook.DataAccess.Data;
using BulkyBook.Models;
using BulkyBooky.DataAccess.Repository.IRepository;
using System.Linq;

namespace BulkyBooky.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db)
            : base(db)
        {
            _db = db;
        }

        public void Update(Category product)
        {
            var objFromDb = _db.Categories.FirstOrDefault(c => c.Id == product.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = product.Name;
            }
        }
    }
}
