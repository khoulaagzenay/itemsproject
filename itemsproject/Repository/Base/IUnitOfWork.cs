using itemsproject.Models;

namespace itemsproject.Repository.Base
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Category> categories {get; }
        IRepository<Item> items { get; }
        IRepository<Client> clients { get; }
        Task<int> CompleteAsync();
    }
}
