using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleDictionary.Data;
using SimpleDictionary.Models.DataModels;

namespace SimpleDictionary.Controllers
{
    [Authorize]
    public class RatingController : Controller
    {
        private readonly DictionaryContext _context;
        private readonly UserManager<User> _userManager;

        public RatingController(DictionaryContext context, UserManager<User> userManager) 
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Like(int defId)
        {
            if(User.Identity.IsAuthenticated)
            {
                User user = await _userManager.FindByNameAsync(User.Identity.Name);

                var definition = _context.Definitions.FirstOrDefault(x => x.Id == defId);

                if(definition != null)
                {
                    LikedDefinition likedDefinition = new LikedDefinition
                    {
                        DefinitionId = defId,
                        UserId = user.Id,
                    };
                    //_context.LikedDefinition.Add(likedDefinition);
                    //definition.Likes++;
                    //await _context.SaveChangesAsync();
                    
                } else
                {
                    return NotFound();
                }
                return Ok();
            }
            return Ok();
        }

    }
}
