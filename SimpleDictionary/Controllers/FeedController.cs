using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleDictionary.Data;
using SimpleDictionary.Models;

namespace SimpleDictionary.Controllers
{
    public class FeedController : Controller
    {
        private readonly DictionaryContext _context;
        private readonly ILogger<FeedController> _logger;

        public FeedController(DictionaryContext context, ILoggerFactory factory)
        {
            _logger = factory.CreateLogger<FeedController>();
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var words = await _context.Definitions.Include(h => h.Hashtags).ToListAsync();
            words[0].Hashtags.ForEach(h => _logger.LogInformation(h.Name));
            return View(words);
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
