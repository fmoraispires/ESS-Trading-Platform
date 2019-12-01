using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Collections.Generic;
using esstp.Models;
using esstp.Models.ViewModels;


namespace esstp.Controllers
{
    //[Authorize]
    //[Produces("application/json")]
    //[ApiController]
    //[Route("api/[controller]")]
    public class UserController : Controller
    {

        private IUserService _userService;
        private IMapper _mapper;


        public UserController(
            IUserService userService,
            IMapper mapper
            )
        {
            _userService = userService;
            _mapper = mapper;
        }



        //GET /User/ReadById/:useroid

        //[Authorize(Policy = "user-readbyid")]
        //[HttpGet, Route("ReadById/{id}")]
        public async Task<IActionResult> ReadById(int id)
        {
            //var dbItem = await _userService.FindAllAsync(j => j.Id == id);
            var dbItem = await _userService.FindAsync(j => j.Id == id);
            //404
            if (dbItem == null)
                return NotFound();

            //var userDtos = _mapper.Map<IList<UserDto>>(dbItem);
            var userDtos = _mapper.Map<UserViewModel>(dbItem);
            return View(userDtos);
        }



        // GET /User/ReadAll

        //ReadAll
        //[Authorize(Policy = "user-readall")]
        //[HttpGet("ReadAll")]
        public async Task<IActionResult> ReadAll()
        {
            var all = await _userService.GetAllAsync();
            var userDtos = _mapper.Map<IList<UserViewModel>>(all);
            return View(userDtos);
            
        }





        // GET: Create

        public IActionResult Create()
        {
            return View(new UserViewModel
            {
                UserName = "",
                Password = "",
                Email = "",
                Nif = 0,
                //Role_Oid = 0,
                //IsInactive = 0
            }
                );
        }


        //POST /User/Create

        //[Authorize(Policy = "user-create")]
        //[HttpPost("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, UserName, Password, Email, Nif, Role_id, IsInactive")] UserViewModel model)
        {
            //400
            if (!ModelState.IsValid)
                return BadRequest();

            //404
            var user = _mapper.Map<User>(model);
            //var currentUserId = int.Parse(User.Identity.Name);
            var currentUserId = 1;

            var dbItem = await _userService.AddAsync(user, model.Password, currentUserId);

            if (dbItem == null)
                return NotFound();


            return RedirectToAction(nameof(ReadAll));
        }



        // GET: User/Edit/:useroid

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();


            var dbItem = await _userService.FindAsync(j => j.Id == id);
            //404
            if (dbItem == null)
                return NotFound();


            //var userDtos = _mapper.Map<IList<UserDto>>(dbItem);
            var userDtos = _mapper.Map<UserViewModel>(dbItem);
            return View(userDtos);
        }


        //POST /User/Update

        //[Authorize(Policy = "user-update")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, [Bind("Id, UserName, Email, Nif, Role_id, IsInactive")] UserViewModel model)
        {
            //// 400
            if (Id <= 0)
                return BadRequest();

            //// 404
            if (Id != model.Id)
                return NotFound();
            

            //// map dto to entity
            //var user = model.GetDbItem(dbItem);
            var user = _mapper.Map<User>(model);
            //user.CreatedBy = dbItem.CreatedBy;
            //user.CreatedDate = dbItem.CreatedDate;

            ////var currentUserId = int.Parse(User.Identity.Name);
            var currentUserId = Id;
            var dbItem = await _userService.UpdateAsync(user, currentUserId);

            return RedirectToAction(nameof(ReadAll));

        }



    }
}
