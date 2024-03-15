namespace TodoApp.BlazorServer.Service.Contract
{
    public interface IBaseService<T> where T : class
    {
        Task<bool> Create(string url, T entity);
        Task<bool> Update(string url, string id, T entity);
        Task<T> GetById(string url, string id);
        Task<bool> Delete(string url, string id);
        Task<IList<T>> GetAll(string url);
        Task<bool> Save();
    }
}
