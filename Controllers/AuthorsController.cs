using BookStore.Data;
using BookStore.Models;
using BookStore.ViewModel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
  

    public class AuthorsController : Controller
    {  private readonly ApplicationDbContext context;

    public AuthorsController(ApplicationDbContext context)
    {
        this.context = context;
    }
        public IActionResult Index()
        {
            var authorsVm = context.Authors
                .Select(author => new AuthorVm
                {
                    Id = author.Id,
                    Name = author.Name,
                    CreatedOn = author.CreatedOn,
                    UpdatedOn = author.UpdatedOn
                })
                .ToList();

            return View(authorsVm);
        }

        [HttpGet]

        public IActionResult Create()
        {
            return View("Form");
        }

        [HttpPost]
        public IActionResult Create(AuthorFormVM authorFVm)
        {
            if (!ModelState.IsValid)
            {
                return View("Form", authorFVm);
            }
            var author = new Author()
            {
                Name = authorFVm.Name
            };
            try
            {
                context.Authors.Add(author);
                context.SaveChanges();
                return RedirectToAction("index");
            }
            catch
            {
                ModelState.AddModelError("Name", "Author name allredy exists");
                return View("Form", authorFVm);
            }

        }

        [HttpGet]
        public IActionResult Edit(int id) // هاي لما اروح ع الايديت بكون القيم موجودة
        {
            var author = context.Authors.Find(id);
            if (author is null)
            {
                return NotFound();

            }

            var viewModel = new AuthorFormVM()
            {
                Id = id,
                Name = author.Name
            };

            return View("Form", viewModel);
        }


        [HttpPost]
        public IActionResult Edit(AuthorFormVM authorFVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Form", authorFVM);
            }
            var author = context.Authors.Find(authorFVM.Id);
            if (author is null)
            {
                return NotFound();
            }
            author.Name = authorFVM.Name;
            context.SaveChanges();
            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            var author = context.Authors.Find(id);
            if (author is null)
            {
                return NotFound();
            }
            context.Authors.Remove(author);
            context.SaveChanges();
            return RedirectToAction("index");
        }

    }
}
