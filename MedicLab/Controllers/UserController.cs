using MedicLab.Models;
using MedicLab.Models.DTO;
using MedicLab.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicLab.Controllers
{
    [Route("")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService userService;
        private readonly IHttpContextAccessor httpContextAccessor;
        public UserController(IUserService _userService, IHttpContextAccessor _httpContextAccessor)
        {
            userService = _userService;
            httpContextAccessor = _httpContextAccessor;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(dtoUserLogin dtoUser)
        {
            var (errorStatus, token) = await userService.Login(dtoUser);
            if (errorStatus.Status == true)
                return BadRequest(errorStatus.Name);
            return Ok(token);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var (errorStatus, users) = await userService.GetUsers();
            if (errorStatus.Status == true)
                return BadRequest(errorStatus.Name);
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("users/update/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] dtoUserUpdate user)
        {
            var errorStatus = await userService.UpdateUser(id,user);
            if (errorStatus.Status == true)
                return BadRequest(errorStatus.Name);
            return Ok(errorStatus.Name);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("users/details/{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] int id)
        {
            var (errorStatus, user) = await userService.GetUserById(id);
            if (errorStatus.Status == true)
                return BadRequest(errorStatus.Name);
            return Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("users/block/{id}")]
        public async Task<IActionResult> BlockUserById([FromRoute] int id)
        {
            var errorStatus = await userService.BlockUserById(id);
            if (errorStatus.Status == true)
                return BadRequest(errorStatus.Name);
            return Ok(errorStatus.Name);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("logout")]
        public async Task<IActionResult> LogOut()
        {
            var errorStatus = await userService.LogOut(Request);
            if (errorStatus.Status == true)
                return BadRequest(errorStatus.Name);
            return Ok(errorStatus);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public async Task<IActionResult> Register(dtoUserRegistration dtoUser)
        {
            var errorStatus = await userService.Register(dtoUser);
            if (errorStatus.Status == true)
                return BadRequest(errorStatus.Name);
            return Ok(errorStatus);
        }

    }
}
