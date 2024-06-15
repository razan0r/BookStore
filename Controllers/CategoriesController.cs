using AutoMapper;
using BookStore.Data;
using BookStore.Models;
using BookStore.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CategoriesController(ApplicationDbContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            var categories=context.Categories.ToList();
            var viewModel=mapper.Map<List<CategoryVM>>(categories);
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();//بروح ع الالانشاء
        }

        [HttpPost]
        public IActionResult Create(CategoryVM categoryVM )
        {
            if(!ModelState.IsValid)
            {
                return View("Create", categoryVM);
            }
            
            //الطريقة بدون المابر     
            //var category = new Category()
            //{
            //    Name = categoryVM.Name
            //};

            var category=mapper.Map<Category>(categoryVM);


            try
            {
                context.Categories.Add(category);
                context.SaveChanges();
                return RedirectToAction("index");
            }
            catch
            {
                ModelState.AddModelError("Name", "category name allredy exists");
                return View("Create", categoryVM);
            }
          
        }

        [HttpGet]
        public IActionResult Edit(int id) // هاي لما اروح ع الايديت بكون القيم موجودة
        {
            var category = context.Categories.Find(id);
            if (category is null)
            {
                return NotFound();

            }

            var viewModel = new CategoryVM()
            {

                Id = id,
                Name = category.Name
            };

            return View("Create", viewModel);

        }


        [HttpPost]
        public IActionResult Edit(CategoryVM categoryVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", categoryVM);
            }
           var category = context.Categories.Find(categoryVM.Id);
            if(category is null)
            {
                return NotFound();
            }
            category.Name = categoryVM.Name;
            context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Details(int id)
        {
            
            var category = context.Categories.Find(id);

            if (category is null)
            {
                return NotFound();
            }

            var viewModel = new CategoryVM()
            {

                Id = id,
                Name = category.Name,
                CreatedOn=category.CreatedOn,
                UpdatedOn=category.UpdatedOn,

            };

            return View(viewModel);
        }
        public IActionResult Delete(int id)
        {
            var category = context.Categories.Find(id);
            if (category is null)
            {
                return NotFound();
            }
            context.Categories.Remove(category); 
            context.SaveChanges();
            return Ok();
        }

    
        public IActionResult CheckName(CategoryVM categoryvm)
        {
        var isExsits = context.Categories.Any(category => category.Name == categoryvm.Name);
        return Json(!isExsits);
        }

    }
}
