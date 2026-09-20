using PetProject1.DAL;
using PetProject1.DTOs;

namespace PetProject1.BLL
{
    public interface ITodoListService
    {
 Task<List<TodoListShortDto>> GetAllListAsync(string userId, int pageNumber = 1, int pageSize = 10);       
        Task<TodoList> GetListByIdAsync(string id, string userId);
        Task CreateListAsync(TodoList list);
        Task UpdateListAsync(TodoList list, string userId);
        Task DeleteListAsync(string id, string userId);

        Task ShareListAsync(string id, string userId, string OwnerId);

        Task UnshareListAsync(string id, string userId, string OwnerId);
    }
}
