using System;
using System.Threading.Tasks;
using AutoMapper;
using esstp.Models;
using esstp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace esstp.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class SignupController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;


        public SignupController(
            IUserService userService,
            IMapper mapper
            )
        {
            _userService = userService;
            _mapper = mapper;
        }


        [AllowAnonymous]
        [HttpPost("Create")]
        public async Task<IActionResult> Signup([FromBody] SignupViewModel model)
        {
            //400
            //if (model.Username == null)
            //    return BadRequest();
            if (!ModelState.IsValid)
                //return BadRequest(ModelState);
                return BadRequest();

            // username exists
            //400
            //var dbItem = await _userService.FindAsync(j => j.UserName == model.UserName || j.Email == model.Email || j.Nif == model.Nif);
            //if (dbItem != null)
                //return BadRequest();


            var user = _mapper.Map<User>(model);
            //system or superadmin
            var currentUserId = 1;
            //404
            var dbItem = await _userService.AddAsync(user, model.Password, currentUserId);
            if (dbItem == null)
                //throw new Exception("Database error adding null record!");
                return NotFound();

            return dbItem != null
                ? (IActionResult)Ok()
                : BadRequest(new Exception("Database error adding valid record!"));
        }

    }
}
