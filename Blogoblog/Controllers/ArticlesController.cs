using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Blogoblog.DAL.Models;
using Blogoblog.DAL.Repositories;
using System.Collections.Generic;


namespace Blogoblog.Controllers
{
    [Authorize]
    public class ArticlesController : Controller
    {
        private readonly IArticleRepository _articleRepo;
        private readonly IRepository<Tag> _tagRepo;
        private readonly IUserRepository _userRepo;
        private readonly ILogger<ArticlesController> _logger;

        public ArticlesController(IArticleRepository article_repo, IRepository<Tag> tag_repo, IUserRepository user_repo, ILogger<ArticlesController> logger)
        {
            _articleRepo = article_repo;
            _tagRepo = tag_repo;
            _userRepo = user_repo;
            _logger = logger;
            _logger.LogDebug(1, "NLog подключен к ArtController");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var articles = await _articleRepo.GetAll();
            _logger.LogInformation("ArticlesController - Index");
            return View(articles);
        }

        [HttpGet]
        public async Task<IActionResult> AddArticle()
        {
            var tags = await _tagRepo.GetAll();
            _logger.LogInformation("ArticlesController - Add");
            return View(new AddArticleViewModel() { Tags = tags.ToList() });            
        }

        [HttpPost]
        public async Task<IActionResult> AddArticle(AddArticleViewModel model, List<int> SelectedTags)
        {
            // Получаем логин текущего пользователя из контекста сессии
            string? currentUserLogin = User?.Identity?.Name;
            var user = _userRepo.GetByLogin(currentUserLogin);

            var tags = new List<Tag>();

            SelectedTags.ForEach(async id => tags.Add(await _tagRepo.Get(id)));

            var article = new Article
            {
                User_Id = user.Id,
                User = user,
                Article_Date = DateTimeOffset.UtcNow,
                Title = model.Title,
                Content = model.Content,
                Tags = tags
            };

            await _articleRepo.Add(article);
            _logger.LogInformation("ArticlesController - Add - complete");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ViewArticle(int id)
        {
            var article = await _articleRepo.Get(id);
            _logger.LogInformation("ArticlesController - GetArticleById");
            return View(article);
        }

        [HttpGet]
        public IActionResult Delete()
        {
            _logger.LogInformation("ArticlesController - Delete");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var article = await _articleRepo.Get(id);
            await _articleRepo.Delete(article);
            _logger.LogInformation("ArticlesController - Delete - complete");
            return RedirectToAction("Index", "Articles");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var article = await _articleRepo.Get(id);
            var tags = await _tagRepo.GetAll();
            _logger.LogInformation("ArticlesController - Update");
            
            return View(new EditArticleViewModel() {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                TagsSelected = article.Tags.ToList(),
                Tags = tags.ToList() });            
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmUpdating(EditArticleViewModel model, List<int> SelectedTags)
        {
            string? currentUserLogin = User?.Identity?.Name;
            var user = _userRepo.GetByLogin(currentUserLogin);

            var tags = new List<Tag>();
            SelectedTags.ForEach(async id => tags.Add(await _tagRepo.Get(id)));

            var article = new Article
            {
                Id = model.Id,
                User_Id = user.Id,
                User = user,
                Article_Date = DateTimeOffset.UtcNow,
                Title = model.Title,
                Content = model.Content,
                Tags = tags                
            };

            await _articleRepo.Update(article);
            _logger.LogInformation("ArticlesController - Update - complete");
            return RedirectToAction("Index", "Articles");
        }
    }
}