using CreationApp.Models;
using CreationApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CreationApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _userService.GetListAsync();
            return View(list);
        }

        public async Task<IActionResult> ActiveUsers()
        {
            var list = await _userService.GetActiveListAsync();
            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,Name,Surname,Age,Active")] User user)
        {
            if (ModelState.IsValid)
            {
                bool result = await _userService.AddAsync(user);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(user);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userService.GetByIdAsync(id.Value);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Name,Surname,Age,Active")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _userService.UpdateAsync(user);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(user);
        }
    }
}
