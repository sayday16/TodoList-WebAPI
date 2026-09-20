
namespace PetProject1.DAL
{
    public interface ITodoListRepository
    {
        Task<List<TodoList>> GetAllListAsync();
        Task<TodoList> GetListByIdAsync(string id);
        Task CreateListAsync(TodoList list);
        Task UpdateListAsync(TodoList list);
        Task DeleteListAsync(string id);

    }
}
