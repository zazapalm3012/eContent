using eContentApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration; 

namespace eContentApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration; 

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration) // Modified
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration; 
            _httpClient.BaseAddress = new System.Uri(_configuration.GetValue<string>("ApiSettings:BaseUrl")); // Modified
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.BaseApiUrl = _configuration.GetValue<string>("ApiSettings:BaseUrl");
            var response = await _httpClient.GetAsync("posts");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var posts = JsonSerializer.Deserialize<List<PostViewModel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(posts);
            }
            return View(new List<PostViewModel>());
        }

        public async Task<IActionResult> Details(Guid id)
        {
            ViewBag.BaseApiUrl = _configuration.GetValue<string>("ApiSettings:BaseUrl");
            var response = await _httpClient.GetAsync($"posts/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var post = JsonSerializer.Deserialize<PostDetailViewModel>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(post);
            }
            return NotFound();
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.BaseApiUrl = _configuration.GetValue<string>("ApiSettings:BaseUrl");
            var categoriesResponse = await _httpClient.GetAsync("categories");
            if (categoriesResponse.IsSuccessStatusCode)
            {
                var content = await categoriesResponse.Content.ReadAsStringAsync();
                var categories = JsonSerializer.Deserialize<List<CategoryViewModel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                ViewBag.Categories = categories;
            }
            else
            {
                ViewBag.Categories = new List<CategoryViewModel>();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var json = JsonSerializer.Serialize(model);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("posts", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Error creating post: {errorContent}");
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.BaseApiUrl = _configuration.GetValue<string>("ApiSettings:BaseUrl");
            var categoriesResponse = await _httpClient.GetAsync("categories");
            if (categoriesResponse.IsSuccessStatusCode)
            {
                var content = await categoriesResponse.Content.ReadAsStringAsync();
                var categories = JsonSerializer.Deserialize<List<CategoryViewModel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                ViewBag.Categories = categories;
            }
            else
            {
                ViewBag.Categories = new List<CategoryViewModel>();
            }
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
