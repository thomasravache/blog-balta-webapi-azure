namespace Blog.Models
{
    public class Category
    {
        public Category()
        {
            Posts = new List<Post>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;

        public IList<Post> Posts { get; set; }
    }
}