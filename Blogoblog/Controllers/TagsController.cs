using Microsoft.AspNetCore.Mvc;
using Blogoblog.DAL.Models;
using Blogoblog.DAL.Repositories;
using Microsoft.AspNetCore.Authorization;


namespace Blogoblog.Controllers
{
    public class TagsController : Controller
    {
        private readonly IRepository<Tag> _repo;
        private readonly ILogger<TagsController> _logger;

        public TagsController(IRepository<Tag> repo, ILogger<TagsController> logger)
        {
            _repo = repo;
            _logger = logger;
            _logger.LogDebug(1, "NLog подключен к TagsController");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tags = await _repo.GetAll();
            _logger.LogInformation("TagsController - Index");
            return View(tags);
        }

        [HttpGet]
        public IActionResult GetTagById()
        {
            _logger.LogInformation("TagsController - GetTagById");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetTagById(int id)
        {
            var tag = await _repo.Get(id);
            _logger.LogInformation("TagsController - GetTagById - complete");
            return View(tag);
        }

        [HttpGet]
        public IActionResult AddTag()
        {
            _logger.LogInformation("TagsController - Add");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTag(Tag newTag)
        {
            await _repo.Add(newTag);
            _logger.LogInformation("TagsController - Add - complete");
            return View(newTag);
        }

        [HttpPost]
        //[Authorize(Roles = "3")]
        public async Task<IActionResult> Delete(int id)
        {
            var tag = await _repo.Get(id);
            await _repo.Delete(tag);
            _logger.LogInformation("TagsController - Delete");
            return RedirectToAction("Index", "Tags");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var tag = await _repo.Get(id);
            _logger.LogInformation("TagsController - Update");
            return View(tag);
        }

        [HttpPost]
        //[Authorize(Roles = "3")]
        public async Task<IActionResult> ConfirmUpdating(Tag tag)
        {
            await _repo.Update(tag);
            _logger.LogInformation("TagsController - Update - complete");
            return RedirectToAction("Index", "Tags");
        }
    }
}