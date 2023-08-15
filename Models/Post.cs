using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models
{
    public class Post
    {
        public Post()
        {
            Tags = new List<Tag>();
        }

        public int Id { get; set; }

        public string Title { get; set; } = null!;
        public string Summary { get; set; } = null!;
        public string Body { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public int AuthorId { get; set; }
        public User Author { get; set; } = null!;

        public virtual IList<Tag> Tags { get; set; }
    }
}