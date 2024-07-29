using MedicLab.Models.DTO;
using MedicLab.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicLab.Controllers
{
    [Route("api/[controller]")]
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

        [HttpPost]
        public async Task<IActionResult> Login(dtoUserLogin dtoUser)
        {
            var (errorStatus, token) = await userService.Login(dtoUser);
            if (errorStatus.Status == true)
                return BadRequest(errorStatus.Name);
            return Ok(token);
        }

    }
}
