using System.ComponentModel.DataAnnotations;

namespace BookStore.ViewModel
{
    public class CategoryVM
    {

        public int Id { get; set; }


        [Required(ErrorMessage ="plz enter name")]
        [MaxLength(30,ErrorMessage ="max 30")]

        public string Name { get; set; } = null!;

    }
}
