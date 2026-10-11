using PetProject1.DAL;
using PetProject1.Domain;
using PetProject1.DTOs;

namespace PetProject1.BLL
{
    public class TodoListService : ITodoListService
    {
        private ITodoListRepository _repository;

        public TodoListService(ITodoListRepository repository)
        {
            _repository = repository;
        }
        public async Task CreateListAsync(TodoList list)
        {
            await _repository.CreateListAsync(list);
        }

        public async Task DeleteListAsync(string id, string userId)
        {
            var list = await _repository.GetListByIdAsync(id);
            if(list == null)
            {
                throw new Exception("list is not found");
            }
            if(list.OwnerId != userId)
            {
                throw new UnauthorizedAccessException("Only the owner can delete this list");
            }
            await _repository.DeleteListAsync(id);
        }

        public async Task<List<TodoListShortDto>> GetAllListAsync(string userId, int pageNumber = 1, int pageSize = 10)
        {
            var allLists = await _repository.GetAllListAsync();
            var filteredLists = allLists
                .OrderByDescending(list => list.CreatedAt)
                .Where(list => list.OwnerId == userId || list.SharedUserIds.Contains(userId))
                .Select(list => new TodoListShortDto { Id = list.Id, Title = list.Title})
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return filteredLists;
        }

        public async Task<TodoList> GetListByIdAsync(string id, string userId)
        {
            var list = await _repository.GetListByIdAsync(id);
            if(list == null)
            {
                throw new Exception("list is not found");
            }
            if(list.OwnerId != userId && !list.SharedUserIds.Contains(userId))
            {
                throw new UnauthorizedAccessException("Only the owner can do this operation");
            }

            return list;
        }

        public async Task UpdateListAsync(TodoList list, string userId)
        {
            var existingList = await _repository.GetListByIdAsync(list.Id);
            if(existingList == null)
            {
                throw new Exception("list is not found");
            }
            if (existingList.OwnerId != userId && !existingList.SharedUserIds.Contains(userId))
            {
                throw new UnauthorizedAccessException("Only the owner can do this operation");
            }
            list.OwnerId = existingList.OwnerId;
            list.CreatedAt = existingList.CreatedAt;
            list.SharedUserIds = existingList.SharedUserIds;
            await _repository.UpdateListAsync(list);
        }

        public async Task ShareListAsync(string id, string userId, string currentUserId)
        {
            var list = await _repository.GetListByIdAsync(id);
            if( list == null)
            {
                throw new Exception("list is not found");
            }
            if(list.OwnerId != currentUserId && !list.SharedUserIds.Contains(currentUserId))
            {
                throw new UnauthorizedAccessException("Owner or shared user can do this operation");
            }
            if (!list.SharedUserIds.Contains(userId))
            {
                list.SharedUserIds.Add(userId);
            }
            await _repository.UpdateListAsync(list);
        }

        public async Task UnshareListAsync(string id, string userId, string currentUserId)
        {
            var list = await _repository.GetListByIdAsync(id);
            if (list == null)
            {
                throw new Exception("list is not found");
            }
            if (list.OwnerId != currentUserId && !list.SharedUserIds.Contains(currentUserId))
            {
                throw new UnauthorizedAccessException("Owner or shared user can do this operation");
            }
            list.SharedUserIds.Remove(userId);
            await _repository.UpdateListAsync(list);
        }
    }
}
