using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Blogoblog.DAL.Models;
using Blogoblog.DAL.Repositories;

namespace PracticeBlog.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private readonly IRepository<Role> _repo;
        private readonly ILogger<RolesController> _logger;

        public RolesController(IRepository<Role> repo, ILogger<RolesController> logger)
        {
            _repo = repo;
            _logger = logger;
            _logger.LogDebug(1, "NLog подключен к RolesController");
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var roles = await _repo.GetAll();
            _logger.LogInformation("RolesController - Index");
            return View(roles);
        }
        [HttpGet]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await _repo.Get(id);
            _logger.LogInformation("RolesController - GetRoleById");
            return View(role);
        }
        [HttpGet]
        public IActionResult AddRole()
        {
            _logger.LogInformation("RolesController - Add");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(Role newRole)
        {
            await _repo.Add(newRole);
            _logger.LogInformation("RolesController - Add - complete");
            return View(newRole);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var role = await _repo.Get(id);
            await _repo.Delete(role);
            _logger.LogInformation("RolesController - Delete");
            return RedirectToAction("Index", "Roles");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var role = await _repo.Get(id);
            _logger.LogInformation("RolesController - Update");
            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmUpdating(Role role)
        {
            await _repo.Update(role);
            _logger.LogInformation("RolesController - Update - confirm");
            return RedirectToAction("Index", "Roles");
        }
    }
}