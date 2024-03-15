namespace TodoApp.BlazorServer.Service.Contract
{
    public interface IBaseService<T> where T : class
    {
        Task<bool> Create(string url, T entity);
        Task<bool> Update(string url, int id, T entity);
        Task<T> GetById(string url, int id);
        Task<bool> Delete(string url, int id);
        Task<IList<T>> GetAll(string url);
        Task<bool> Save();
    }
}
