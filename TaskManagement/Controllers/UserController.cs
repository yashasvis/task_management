using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManagement.Factory;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserManagementService _userManagementService;
        private readonly IMapper _mapper;
        public UserController(IUserManagementService userManagementService, IMapper mapper)
        {
            _userManagementService = userManagementService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("users")]
        public IActionResult Index(string? message)
        {
            MainUserModel model = new MainUserModel();
            model.message = message;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetUsersWithPagination()
        {
            try
            {
                GetUsersWithPaginationModel model = new GetUsersWithPaginationModel();
                int totalRecord = 0;
                int filterRecord = 0;
                var draw = Request.Form["draw"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
                int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
                model = await _userManagementService.GetUsersWithPagination(pageSize, skip, searchValue! ,sortColumn!, sortColumnDirection!);
                filterRecord = model.totalRecords;
                totalRecord = model.totalRecords;
                var returnObj = new
                {
                    draw = draw,
                    recordsTotal = totalRecord,
                    recordsFiltered = filterRecord,
                    data = model.users
                };
                return new JsonResult(returnObj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Route("create-user")]
        public IActionResult Create()
        {
            Users model = new Users();
            var userDTO = _mapper.Map<UsersDTO>(model);
            return View(userDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Creates(UsersDTO user)
        {
            try
            {
                int userId = 0;
                string message = "";
                if (ModelState.IsValid)
                {
                    var userDTO = _mapper.Map<Users>(user);
                    userId = await _userManagementService.SaveUser(userDTO);
                    if (userId == 0)
                        message = "Something went wrong while saving user, please try again.!";
                    else
                        message = "New user added successfully.!";
                    return RedirectToAction("Index", "User", new { message = message });
                }
                return View("Create", user);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Route("edit-user")]
        public async Task<IActionResult> Edit(int id)
        {
            Users model = new Users();
            model = await _userManagementService.GetUserById(id);
            var userDTO = _mapper.Map<UsersDTO>(model);
            return View(userDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Edits(UsersDTO user)
        {
            try
            {
                int userId = 0;
                string message = "";
                if (ModelState.IsValid)
                {
                    var userDTO = _mapper.Map<Users>(user);
                    userId = await _userManagementService.EditUser(userDTO);
                    if (userId == 0)
                        message = "Something went wrong while saving user, please try again.!";
                    else
                        message = "User's Details Updated Successfully.!";
                    return RedirectToAction("Index", "User", new { message = message });
                }
                return View("Edit", user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
