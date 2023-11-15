using AuctionAPI.DTO;
using AuctionAPI.Entities;
using AuctionAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuctionAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            List<User>? users = await _userService.GetAllUser();
            return Ok(users);
        }

        [HttpGet]
        public async Task<IActionResult> GetIdUser([FromQuery] int id)
        {
            var users = await _userService.GetUserWithId(id);
            return Ok(users);
        }


        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserOperationDTO userDTO)
        {
            var userInfo = await _userService.CreateUser(userDTO);
            return Ok(userInfo);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            await _userService.UpdateUser(user);
            return Ok(user);
        }


    }
}
