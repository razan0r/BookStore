namespace BookStore.Models
{
    public class BookCategory
    {
        public int BookId { get; set; }
        public Book book { get; set; }

        public int CategoryId { get; set; }
        public Category category { get; set; }
    }
}
