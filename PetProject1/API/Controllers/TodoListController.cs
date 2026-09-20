using Microsoft.AspNetCore.Mvc;
using PetProject1.BLL;
using PetProject1.DAL;

namespace PetProject1.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoListController : ControllerBase
    {
        private ITodoListService _todoListService;

        public TodoListController(ITodoListService todoListService)
        {
            _todoListService = todoListService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var userId = GetCurrentUserId();
           var lists =  await _todoListService.GetAllListAsync(userId, pageNumber, pageSize);
           return Ok(lists);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var userId = GetCurrentUserId();
            var list = await _todoListService.GetListByIdAsync(id, userId);
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateList(TodoList list)
        {
            var userId = GetCurrentUserId();
            await _todoListService.UpdateListAsync(list, userId);
            
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateList(TodoList list)
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId)) {
                return BadRequest("user Id empty");
            }
            list.OwnerId = userId;
            list.CreatedAt = DateTime.UtcNow;
            await _todoListService.CreateListAsync(list);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteList(string id)
        {
            var userId = GetCurrentUserId();
            await _todoListService.DeleteListAsync(id, userId);
            return Ok();
        }

        [HttpGet("{id}/shares")]
        public async Task<IActionResult> GetSharedUsers(string id)
        {
            var userId = GetCurrentUserId();
            var list = await _todoListService.GetListByIdAsync(id, userId);

            return Ok(list.SharedUserIds);
        }

        [HttpPost("{id}/share")]
        public async Task<IActionResult> ShareUser(string id,  string userId)
        {
            var Ownerid = GetCurrentUserId();
            await _todoListService.ShareListAsync(id, userId, Ownerid);
            return Ok();
        }

        [HttpPost("{id}/unshare")]
        public async Task<IActionResult> UnshareUser(string id, string userId)
        {
            var OwnerId = GetCurrentUserId();
            await _todoListService.UnshareListAsync(id, userId, OwnerId);
            return Ok();
        }

        private string GetCurrentUserId()
        {
            return Request.Headers["User-Id"].ToString();
        }
    }
}
