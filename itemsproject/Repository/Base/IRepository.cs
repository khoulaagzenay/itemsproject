namespace itemsproject.Repository.Base
{
    public interface IRepository<T> where T : class
    {
        Task <T> GetByIdAsync(int id, string? includeProperties = null);

        Task<IEnumerable<T>> GetAllAsync(string? includeProperties = null);

        Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<int> ids);

        Task AddAsync(T myItem);

        Task UpdateAsync(T myItem);

        Task DeleteAsync(int id);

    }
}
