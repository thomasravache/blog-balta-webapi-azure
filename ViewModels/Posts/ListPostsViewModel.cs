using Blog.Models;

namespace Blog.ViewModels.Posts;

public class ListPostsViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public DateTime LastUpdateDate { get; set; }
    public string? Category { get; set; }
    public string? Author { get; set; }

    // public static explicit operator ListPostsViewModel(Post post)
    //     => new ListPostsViewModel
    //     {
    //         Id = post.Id,
    //         Title = post.Title,
    //         Slug = post.Slug,
    //         LastUpdateDate = post.LastUpdateDate,
    //         Category = post.Category.Name,
    //         Author = $"{post.Author.Name} - {post.Author.Email}"
    //     };

    // public static ListPostsViewModel FromPost(Post post)
    //     => new ListPostsViewModel
    //     {
    //         Id = post.Id,
    //         Title = post.Title,
    //         Slug = post.Slug,
    //         LastUpdateDate = post.LastUpdateDate,
    //         Category = post.Category.Name,
    //         Author = $"{post.Author.Name} - {post.Author.Email}"
    //     };
}