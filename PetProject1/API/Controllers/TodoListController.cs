using Microsoft.AspNetCore.Mvc;
using PetProject1.BLL;
using PetProject1.DTOs;

namespace PetProject1.Controllers
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
            var lists = await _todoListService.GetAllListAsync(userId, pageNumber, pageSize);
            return Ok(lists);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var userId = GetCurrentUserId();
            var listDto = await _todoListService.GetListByIdAsync(id, userId);
            if (listDto == null) return NotFound();

            return Ok(listDto);
        }

        [HttpGet("{id}/shared")] 
        public async Task<IActionResult> GetSharedUsers(string id)
        {
            var userId = GetCurrentUserId();
            var listDto = await _todoListService.GetListByIdAsync(id, userId);

            return Ok(listDto.SharedUserIds);
        }

        [HttpPost]
        public async Task<IActionResult> CreateList(CreateTodoListDto dto)
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId)) return BadRequest("User Id empty");

            await _todoListService.CreateListAsync(dto, userId);
            return Ok();
        }

        [HttpPost("{id}/share")]
        public async Task<IActionResult> ShareUser(string id, string userId)
        {
            var ownerId = GetCurrentUserId();
            await _todoListService.ShareListAsync(id, userId, ownerId);
            return Ok();
        }

        [HttpPost("{id}/unshare")]
        public async Task<IActionResult> UnshareUser(string id, string userId)
        {
            var ownerId = GetCurrentUserId();
            await _todoListService.UnshareListAsync(id, userId, ownerId);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateList(UpdateTodoListDto dto)
        {
            var userId = GetCurrentUserId();
            await _todoListService.UpdateListAsync(dto, userId);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteList(string id)
        {
            var userId = GetCurrentUserId();
            await _todoListService.DeleteListAsync(id, userId);
            return Ok();
        }

        private string GetCurrentUserId()
        {
            return Request.Headers["User-Id"].ToString();
        }
    }
}