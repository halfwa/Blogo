using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Blogoblog.DAL.Models;
using Blogoblog.DAL.Repositories;


namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentsController : Controller
    {
        private readonly IRepository<Comment> _repo;
        private readonly ILogger<CommentsController> _logger;

        public CommentsController(IRepository<Comment> repo, ILogger<CommentsController> logger)
        {
            _repo = repo;
            _logger = logger;
            _logger.LogDebug(1, "NLog подключен к CommentsController");
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var comments = await _repo.GetAll();
            _logger.LogInformation("CommentsController - Index");
            return View(comments);
        }


        [HttpGet("{getbyid}")]
        public async Task<IActionResult> GetCommentById(int id)
        {
            var comment = await _repo.Get(id);
            _logger.LogInformation("CommentsController - GetCommentById");
            return View(comment);
        }


        [HttpGet("{reg}")]        
        public IActionResult Register()
        {
            _logger.LogInformation("CommentsController - Add");
            return View();
        }


        [HttpPost("{areg}")]
        public async Task<IActionResult> Register(Comment newComment)
        {
            await _repo.Add(newComment);
            _logger.LogInformation("CommentsController - Add - complete");
            return View(newComment);
        }


        [HttpPost("{del}")]
        public async Task<IActionResult> Delete(int id)
        {
            var role = await _repo.Get(id);
            await _repo.Delete(role);
            _logger.LogInformation("CommentsController - Delete");
            return RedirectToAction("Index", "Comments");
        }


        [HttpGet("{upd}")]
        public async Task<IActionResult> Update(int id)
        {
            var comment = await _repo.Get(id);
            _logger.LogInformation("CommentsController - Update");
            return View(comment);
        }


        [HttpPost]
        public async Task<IActionResult> ConfirmUpdating(Comment comment)
        {
            await _repo.Update(comment);
            _logger.LogInformation("CommentsController - Update - complete");
            return RedirectToAction("Index", "Comments");
        }
    }
}
