using ThoughtsAligned.IService;
using ThoughtsAligned.Models.Dto;
using ThoughtsAligned.Models.ViewModels;
using ThoughtsAligned.Utility.Helpers;
using ThoughtsAligned.Utilties.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ThoughtsAligned.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    [Authorize]

    public class AccountController : ControllerBase
    {
        private IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: api/Account/Authenticate
        /// <summary>
        /// To check Authantication For Email and password
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>        
        [Route("Authenticate")]
        [AllowAnonymous]
        [HttpGet("authenticate")]
        public IActionResult Authenticate([FromBody] UserClaimModel loginDto)
        {
            //---authenticate user
            var user = _userService.Authenticate(loginDto);

            if (user == null)
                user = _userService.Create(loginDto).Result;

            //Get JWT token
            var tokenString = _userService.GetToken(loginDto, user);

            //return basic user info (without password) and token to store client side
            return Ok(new
            {
                access_token = tokenString,
                expires_in = 7
            });
        }


        // POST: api/Account/Authenticate
        /// <summary>
        /// To check Authantication ForEmail and password
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>        
        [Route("SetPassword")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SetPassword([FromBody] LoginModel loginDto)
        {
            var user = _userService.GetByEmail(loginDto.Email);

            if (user == null)
                return BadRequest(new { message = "Email or password is incorrect." });

            return BadRequest(new { message = "Something happened wrong at server!" });
        }

        // POST: api/Account/Register
        /// <summary>
        /// Register new Admin on Behalf of model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserClaimModel model)
        {
                var objSuperAdmin = _userService.GetByEmail(HopperConstants.SuperAdminEmail);
                if (objSuperAdmin == null)
                    objSuperAdmin = await CreateSuperAdmin();

                //---changes in super
                if (objSuperAdmin != null && !HopperConstants.SuperAdminEmail.Equals(model.preferred_username))
                {
                    await _userService.Create(model);
                    return Ok(new { message = "Account activated." });
                }
                //---return bad requst
                return BadRequest(new { message = "Something went wrong on the server!" });
        }

        private async Task<UserDto?> CreateSuperAdmin()
        {
            await _userService.CreateSuperAdmin();
            var objSuperAdmin = _userService.GetByEmail(HopperConstants.SuperAdminEmail);
            return objSuperAdmin;
        }

        [Route("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var obj = _userService.GetAll();
            return Ok(obj);
        }

        [Route("GetAllPendingUsers")]
        public IActionResult GetAllPendingUsers()
        {
            var obj = _userService.GetAllPendingUsers();
            return Ok(obj);
        }

        [Route("GetUser")]
        public IActionResult GetUser(string id)
        {
            var user = _userService.GetById(id);
            var userDto = new UserDto
            {
                CreatedBy = user.CreatedBy,
                Email = user.Email,
                CreatedOn = user.CreatedOn,
                Id = user.Id,
                Name = user.Name,
                Status = user.Status,
                UpdatedBy = user.UpdatedBy,
                UpdateOn = user.UpdateOn,
                UserRole = user.Status
            };
            return Ok(user);
        }

        [Route("UpdateUserStatus")]
        [HttpPut]
        public async Task<IActionResult> UpdateUserStatus([FromQuery] string userId, byte userStatus)
        {
            await _userService.UpdateUserStatus(userId, userStatus);
            return Ok();
        }

        [Route("UpdateUserRole")]
        [HttpPut]
        public async Task<IActionResult> UpdateUserRole([FromQuery] string userId, byte userRole)
        {
            await _userService.UpdateUserRole(userId, userRole);
            return Ok();
        }

        [Route("DeleteUser")]
        [HttpDelete]
        public async Task<ActionResult> DeleteUser(string id)
        {
            await _userService.DeleteUser(id);
            return Ok();
        }
    }
}
