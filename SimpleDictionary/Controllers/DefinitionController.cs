using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleDictionary.Data;
using SimpleDictionary.Models.DataModels;
using SimpleDictionary.Models.ViewModels;
using SimpleDictionary.Services;

namespace SimpleDictionary.Controllers
{
    public class DefinitionController : Controller
    {
        private readonly DictionaryContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IHashtagParser<Hashtag> _hashtagParser;

        public DefinitionController(
            UserManager<User> userManager,
            DictionaryContext context,
            IHashtagParser<Hashtag> hashtagParser
            )
        {
            _hashtagParser = hashtagParser;
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(DefinitionViewModel model)
        {

            User user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (ModelState.IsValid && user != null)
            {
                Definition definition = new Definition()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Example = model.Example,
                    UserId = user.Id,
                    CreationDate = DateTime.Now,
                    AuthorUsername = user.UserName,
                    Hashtags = _hashtagParser.Parse(model.Hashtags)
                };
                await _context.Definitions.AddAsync(definition);
                user.Definitions.Add(definition);
                _context.SaveChanges();
            }
            else return RedirectToAction("Index", "Feed");
            return RedirectToAction("Index", "Feed");
        }

        [HttpGet]
        public async Task<IActionResult> UserDefinitions(string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var defList = _context.Definitions.Where(x => x.UserId == user.Id).Include(h => h.Hashtags).ToList();
                UserDefinitionsViewModel userDefinitions = new UserDefinitionsViewModel()
                {
                    Author = user,
                    Definitions = defList
                };
                return View(userDefinitions);
            }
            return RedirectToAction("Index", "Feed");
        }

        [HttpGet]
        public async Task<IActionResult> Search(string hashtag)
        {

            if (string.IsNullOrEmpty(hashtag))
            {
                return RedirectToAction("Index", "Feed");
            }

            if (hashtag.Contains("#"))
            {
                List<Definition> definitions = await _context.Definitions.Include(h => h.Hashtags).ToListAsync();
                List<Definition> result = new List<Definition>();

                foreach (var def in definitions)
                {
                    if (def.Hashtags.Contains(new Hashtag { Name = hashtag }))
                    {
                        result.Add(def);
                    }
                }

                return View(result);
            } else
            {
                return View(_context.Definitions.Where(d => d.Name == hashtag));
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            Definition definition = await _context.Definitions.FirstOrDefaultAsync(x => x.Id == id);

            if(definition != null)
            {
                if(definition.AuthorUsername.Equals(User.Identity.Name))
                {
                    _context.Definitions.Remove(definition);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Profile", "Account", new { user = User.Identity.Name });
        }

    }
}
