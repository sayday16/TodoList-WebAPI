
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using PetProject1.Domain;

namespace PetProject1.DAL
{
    public class TodoListRepository : ITodoListRepository
    {
        private  IMongoCollection<TodoList> _todoLists;

        public TodoListRepository(IOptions<MongoDbSettings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            var database = client.GetDatabase(options.Value.DatabaseName);
            _todoLists = database.GetCollection<TodoList>("TodoLists");
        }
        public async Task CreateListAsync(TodoList list)
        {
            await _todoLists.InsertOneAsync(list);
        }

        public async Task DeleteListAsync(string id)
        {
            await _todoLists.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<List<TodoList>> GetAllListAsync()
        {
            return await _todoLists.Find(x => true).ToListAsync();
        }

        public async Task<TodoList> GetListByIdAsync(string id)
        {
            return await _todoLists.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateListAsync(TodoList list)
        {
            await _todoLists.ReplaceOneAsync(x => x.Id == list.Id, list);
        }
    }
}
