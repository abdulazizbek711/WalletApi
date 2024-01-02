using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletApi.Authentication;
using WalletApi.Data;
using WalletApi.Dto;
using WalletApi.Interface;
using WalletApi.Models;

namespace WalletApi.Controller;
[Route("api/[controller]")]
[ApiController]
public class UserController: Microsoft.AspNetCore.Mvc.Controller
{
     private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IUserService _userService;

        public UserController(IUserRepository userRepository, IMapper mapper, DataContext context, IUserService userService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _context = context;
            _userService = userService;
        }
        [HttpGet("generate-api-key")]
        public IActionResult GenerateApiKey(int userId, string apiKey)
        {
            string hashedApiKey = _userService.GenerateXAuthToken(userId, apiKey);
            return Ok(new { HashedApiKey = hashedApiKey });
        }
        [HttpGet("{hashedApiKey}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [Authorize]
        public IActionResult GetUsers()
        {
            var users = _userRepository.GetUsers();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(users);
        }
        [HttpPost("{hashedApiKey}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUser([FromBody] UserDto userCreate)
        {
            if (!HttpContext.Request.Headers.TryGetValue("X-Api-Key", out var hashedApiKey))
            {
                return Unauthorized("Hashed API Key is missing");
            }

            // Perform authentication and authorization based on the hashed API key
            if (!IsApiKeyValid(hashedApiKey))
            {
                return Unauthorized("Invalid Hashed API Key");
            }
            if (userCreate == null)
                return BadRequest(ModelState);

            var user = _userRepository.GetUsers()
                .Where(c => c.UserName.Trim().ToUpper() == userCreate.UserName.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (user != null)
            {
                ModelState.AddModelError("", "User already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userMap = _mapper.Map<User>(userCreate);

            if (!_userRepository.CreateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }
        [HttpPut("{UserId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public IActionResult UpdateUser(int UserId, [FromBody] UserDto updatedUser)
        {
            if (updatedUser == null)
                return BadRequest(ModelState);
            if (UserId != updatedUser.UserId)
                return BadRequest(ModelState);
            if (!_userRepository.UserExists(UserId))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest();
            var userMap = _mapper.Map<User>(updatedUser);
            if (!_userRepository.UpdateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong updating user");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        [HttpDelete("{UserId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteUser(int UserId)
        {
            if (!_userRepository.UserExists(UserId))
                return NotFound();
            var userToDelete = _userRepository.GetUser(UserId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_userRepository.DeleteUser(userToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting user");
            }

            return NoContent();

        }
        private bool IsApiKeyValid(string hashedApiKey)
        {
            return true;
        }
}