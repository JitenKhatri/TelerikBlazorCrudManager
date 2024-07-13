using BlazorImplementation.Models;
using BlazorImplementation.Models.QueryStrings;
using DataAPIs.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DataAPIs.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IJWTTokenService _jwtTokenService;
        private readonly IUserService _userService;

        public UserController(IJWTTokenService jwtTokenService, IUserService userService)
        {
            _jwtTokenService = jwtTokenService;
            _userService = userService;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(TokenModel tokenModel)
        {
            var result = _userService.Login(tokenModel);
            if (result != null) {
                string token = _jwtTokenService.BuildToken(result);
                return Ok(token);
            }
            else
            {
                return Unauthorized("Please Enter a valid UserName and Password Combination");
            }
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(UserModel userModel)
        {
            var result = _userService.Register(userModel);
            if (result != null)
            {
                return StatusCode(StatusCodes.Status200OK,result);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,result);
            }
        }

        [HttpPut]
        [Route("update")]
        public IActionResult Update(UserModel userModel)
        {
            var result = _userService.UpdateUser(userModel);
            if (result != null)
            {
                return StatusCode(StatusCodes.Status200OK, result);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
        }

        [HttpGet]
        [Route("GetUsers")]
        public IActionResult GetUsers([FromQuery] UserQueryString userQueryString)
        {
            var result = _userService.GetUsers(userQueryString);
            return Ok(result);
        }

        [HttpDelete]
        [Route("deleteUser/{id}")]
        public IActionResult Delete(long id)
        {
            var result = _userService.DeleteUser(id);
            return Ok(result);
        }
    }
}
