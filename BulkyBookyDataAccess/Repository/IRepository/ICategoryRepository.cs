using BulkyBook.Models;

namespace BulkyBooky.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category product);
    }
}
