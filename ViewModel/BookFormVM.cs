using BookStore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BookStore.ViewModel
{
    public class BookFormVM
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Title { get; set; } = null!;

        [Display(Name = "Author")]
        public int AuthorId { get; set; }

        public List<SelectListItem>? Authors { get; set; }
        public string publisher { get; set; } = null!;
        [Display(Name = "publish Date")]

        public DateTime publishDate { get; set; }=DateTime.Now;
        [Display(Name ="Choose Image")]
        public IFormFile? ImageUrl { get; set; }

        public string description { get; set; }

        [Display(Name = "The Categories")]

        public List<int> SelectedCategories { get; set; } = new List<int>();
        public List<SelectListItem>? Categories { get; set; }






    }
}
