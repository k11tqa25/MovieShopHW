using ApplicationCore.Models;
using ApplicationCore.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterRequestModel model)
        {
            var createdUser = await _userService.RegisterUserAsync(model);

            // 200 (Not bad practice)
            //return Ok(createdUser);

            // 201 and send the URL for newly created user also (api/account/user/{id})  (Better practice)
            return CreatedAtRoute(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult>  LoginUser([FromBody] UserLoginRequestModel model)
        {
            var logedInUser = await _userService.LoginAsync(model.Email, model.Password);
            if (logedInUser == null) return NotFound("User not found");
            return Ok(logedInUser);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var user = await _userService.GetAllUsersAsync();
            if (user == null) return NotFound($"No user is found.");
            return Ok(user);
        }


        [HttpGet]
        [Route("{id:int}", Name = nameof(GetUserById))]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound($"User not found with id = {id}");
            return Ok(user);
        }
        
    }
}
