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
            var authors = context.Authors.ToList();
            var authorsVm = new List<AuthorVm>();

            foreach (var author in authors)
            {

                var authorVm = new AuthorVm()
                {
                    Id = author.Id,
                    Name = author.Name,
                    CreatedOn = author.CreatedOn,
                    UpdatedOn = author.UpdatedOn
                };
                authorsVm.Add(authorVm);
            }



            return View(authorsVm);
        }

        public IActionResult Create()
        {
            return View("Form");

        }

    }
}
