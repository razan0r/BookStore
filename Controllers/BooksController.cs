using BookStore.Data;
using BookStore.Models;
using BookStore.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Net.Mime.MediaTypeNames;

namespace BookStore.Controllers
{
    public class BooksController : Controller
    {
        public ApplicationDbContext Context { get; }

        public BooksController(ApplicationDbContext context)
        {
            Context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {

            var authros = Context.Authors.OrderBy(author => author.Name).ToList();
            var categories = Context.Categories.OrderBy(author => author.Name).ToList();

            var authorList = new List<SelectListItem>();

            foreach (var author in authros)
            {
                authorList.Add(new SelectListItem
                {
                    Value = author.Id.ToString(),
                    Text = author.Name,
                });
            }
            var categotyList = new List<SelectListItem>();

            foreach (var category in categories)
            {
                categotyList.Add(new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.Name,
                });
            }
            var viewModel = new BookFormVM
            {
                Authors = authorList,
                Categories=categotyList,
            };
                
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(BookFormVM viewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var book = new Book {
                Title = viewModel.Title,
                AuthorId = viewModel.AuthorId,
                publisher = viewModel.publisher,
                publishDate = viewModel.publishDate,
                description = viewModel.description,
                Categories = viewModel.SelectedCategories.Select(id => new BookCategory
                {
                    CategoryId = id,
                }).ToList(),

            };

            Context.Books.Add(book);
            Context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
