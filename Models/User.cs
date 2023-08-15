using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Blog.Models
{
    public class User
    {
        public User()
        {
            Posts = new List<Post>();
            Roles = new List<Role>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        [JsonIgnore] // Ignora uma propriedade na hora de ser renderizado na tela
        public string PasswordHash { get; set; } = null!;
        // public string GitHub { get; set; } = null!;
        public string Bio { get; set; } = null!;
        public string Image { get; set; } = null!;
        public string Slug { get; set; } = null!;

        public IList<Post> Posts { get; set; }
        public IList<Role> Roles { get; set; }
    }
}