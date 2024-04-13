using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Blogoblog.DAL.Models;
using Blogoblog.DAL.Repositories;
using System.Security.Authentication;
using System.Security.Claims;


namespace Blogoblog.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserRepository _repo;        
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserRepository repo, ILogger<UsersController> logger)
        {
            _repo = repo;            
            _logger = logger;            
            _logger.LogDebug(1, "NLog подключен к UsersController");
        }

        [HttpGet]
        [Route("Authenticate")]
        public IActionResult Authenticate()
        {
            _logger.LogInformation("UsersController - Authenticate");
            return View();
        }

        [HttpPost]
        [Route("Authenticate")]
        public async Task<IActionResult> Authenticate(string email, string password)
        {
            if (String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password))
                throw new ArgumentNullException("Некорректные данные");

            User user = _repo.GetByLogin(email);
            // Лучше переделать и не давать косвенную информацию, что такой пользователь есть
            if (user is null)
                throw new AuthenticationException("Пользователь на найден");

            if (user.Password != password)
                throw new AuthenticationException("Введенный пароль некорректен");

            string role = user.Role_Id.ToString();

            List<Claim> userClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Email),
                            new Claim(ClaimTypes.Role, role),
                        };

            var identity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
            _logger.LogInformation("UsersController - Authenticate - complete");
            return View("Profile", user);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _logger.LogInformation("UsersController - LogOut");
            return RedirectToAction("Index", "Home");
        }

        // GET: All Users
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _repo.GetAll();
            _logger.LogInformation("UsersController - Index");
            return View(users);
        }

        // GET: User
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _repo.Get(id);
            _logger.LogInformation("UsersController - GetUserById");
            return View(user);
        }

        // GET: User/Create(Register)
        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            _logger.LogInformation("UsersController - Register");
            return View();
        }

        // POST: User/Create(Register)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Register")]
        public async Task<IActionResult> Register([Bind("Id,FirstName,LastName,Email,Password")] User newUser)
        {
            if (ModelState.IsValid)
            {
                await _repo.Add(newUser);
                _logger.LogInformation("UsersController - Register - complete");
                return View(newUser);
                //return RedirectToAction(nameof(Index));
            }
            return View(newUser);
        }

        //GET: User/Edit(Update)
        [Authorize(Roles = "3")]
        [HttpGet]
        [Route("Update")]
        public async Task<IActionResult> Update(int id)
        {
            var user = await _repo.Get(id);
            _logger.LogInformation("UsersController - Update");
            return View(user);
        }

        //POST: User/Edit(Update)
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmUpdating([Bind("Id,FirstName,LastName,Email,Password")] User user)
        {
            await _repo.Update(user);
            _logger.LogInformation("UsersController - Update - complete");
            return RedirectToAction("Index", "Users");
        }

        // POST: User/Delete
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _repo.Get(id);
            if (user != null)
            {
                await _repo.Delete(user);
            }
            _logger.LogInformation("UsersController - Delete");
            return RedirectToAction("Index", "Users");
        }
    }
}