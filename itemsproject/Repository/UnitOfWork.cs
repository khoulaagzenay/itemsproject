using itemsproject.Data;
using itemsproject.Models;
using itemsproject.Repository.Base;

namespace itemsproject.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            categories = new Repository<Category>(_context);
            items = new Repository<Item>(_context);
            clients = new Repository<Client>(_context);
        }
        public IRepository<Category> categories { get; private set; }
        public IRepository<Item> items { get; private set; }
        public IRepository<Client> clients { get; private set; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
