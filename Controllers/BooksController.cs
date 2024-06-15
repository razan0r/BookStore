using BookStore.Data;
using BookStore.Models;
using BookStore.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace BookStore.Controllers
{
    [Authorize]

    public class BooksController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public ApplicationDbContext Context { get; }

        public BooksController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            Context = context;
            this.webHostEnvironment = webHostEnvironment;
        }
        /*
         [AllowAnonymous]
        هاي بحطها اذا بدي بس هاد الاكشن ادخلو وانا مش مسطجل دخول
         */
        
        public IActionResult Index()
        {
            var books = Context.Books.
            Include(book => book.Author).
            Include(book => book.Categories).
            ThenInclude(book => book.category).
            ToList();

            var bookVms = books.Select(book => new BookVM
            { 
                Id = book.Id,
                Title = book.Title,
                Author = book.Author.Name,
                Publisher = book.publisher,
                publishDate = book.publishDate,
                ImageUrl = book.ImageUrl,
                Categories = book.Categories.Select(book => book.category.Name).ToList()
                }).ToList() ;
            return View(bookVms);


            // الطريقة القديمة 
            /* var bookVms = new List<BookVM>();

             foreach (var book in books)
             {
                 var bookVM = new BookVM
                 {
                     Id = book.Id,
                     Title = book.Title,
                     Author = book.Author.Name,
                     Publisher = book.publisher,
                     publishDate = book.publishDate,
                     ImageUrl = book.ImageUrl,
                     Categories = new List<string>(),
                 };


                 foreach (var c in book.Categories)
                 {
                     bookVM.Categories.Add(c.category.Name);
                 }
                 bookVms.Add(bookVM);
             }

             */


            //----------------------------------------------
            /*   foreach (var item in books)
               {
                   Console.WriteLine($"Title : {item.Title}.... { item.Author.Name}");
                   foreach (var item2 in item.Categories)
                   {
                   Console.WriteLine(item2.category.Name);
                    }
               }*/

        }

        [HttpGet]
        public IActionResult Create()
        {

            var authros = Context.Authors.OrderBy(author => author.Name).ToList();
            var categories = Context.Categories.OrderBy(author => author.Name).ToList();

            var authorList = Context.Authors.Select(author => new SelectListItem
            {
                Value = author.Id.ToString(),
                Text = author.Name,
            }).ToList();

            var categoryList = categories.Select(category => new SelectListItem
            {
                Value = category.Id.ToString(),
                Text = category.Name,
            }).ToList();

            var viewModel = new BookFormVM
            {
                Authors = authorList,
                Categories=categoryList,
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
            string ImageName = null;
            if (viewModel.ImageUrl != null)
            {
                ImageName = Path.GetFileName(viewModel.ImageUrl.FileName);
                var path = Path.Combine($"{webHostEnvironment.WebRootPath}/img/books", ImageName);
                var stream=System.IO.File.Create(path);
                viewModel.ImageUrl.CopyTo(stream);// انسخ الصورة من المكان المؤقت لل ستريم
            }

            var book = new Book {
                Title = viewModel.Title,
                AuthorId = viewModel.AuthorId,
                publisher = viewModel.publisher,
                publishDate = viewModel.publishDate,
                description = viewModel.description,
                ImageUrl = ImageName,
                Categories = viewModel.SelectedCategories.Select(id => new BookCategory
                {
                    CategoryId = id,
                }).ToList(),

            };

            Context.Books.Add(book);
            Context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var book=Context.Books.Find(id);
            if (book is null)
            {
                return NotFound();
            }
            
            if(book.ImageUrl!= null)
            {
               var path = Path.Combine(webHostEnvironment.WebRootPath,"img/books",book.ImageUrl);
               if (System.IO.File.Exists(path))
                {
                  System.IO.File.Delete(path);
                }
            }
         

            Context.Books.Remove(book);
            Context.SaveChanges();
          
            return RedirectToAction("index");
        }
    }
}
