using Blog.Data;
using Blog.Models;
using Blog.ViewModels;
using Blog.ViewModels.Posts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

[ApiController]
public class PostController : ControllerBase
{
    [HttpGet("v1/posts")]
    public async Task<IActionResult> GetAsync(
        [FromServices] BlogDataContext context,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 25
    )
    {
        try
        {
            var postsCount = await context.Posts.AsNoTracking().CountAsync();

            var posts = await context
                .Posts
                .AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.Author)
                // .Select(x => new
                // {
                //     x.Id,
                //     x.Title,
                // }) // outro modo de mandar as informações com objeto anonimo
                .Select(x => new ListPostsViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Slug = x.Slug,
                    LastUpdateDate = x.LastUpdateDate,
                    Category = x.Category.Name,
                    Author = $"{x.Author.Name} - {x.Author.Email}"
                })
                .Skip(page * pageSize)
                .Take(pageSize)
                .OrderByDescending(x => x.LastUpdateDate)
                .ToListAsync();

            return Ok(new ResultViewModel<dynamic>(new
            {
                total = postsCount,
                page,
                totalPages = postsCount / pageSize,
                pageSize,
                posts
            }));
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResultViewModel<List<Post>>("05X04 - Falha interna no servidor"));
        }
    }

    [HttpGet("v1/posts/{id:int}")]
    public async Task<IActionResult> GetAsync(
        [FromServices] BlogDataContext context,
        [FromRoute] int id
    )
    {
        try
        {
            var postsCount = await context.Posts.AsNoTracking().CountAsync();

            var post = await context
                .Posts
                .AsNoTracking()
                .Include(x => x.Author)
                    .ThenInclude(x => x.Roles)
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (post == null)
                return NotFound(new ResultViewModel<Post>("Conteúdo não encontrado"));

            return Ok(new ResultViewModel<Post>(post));
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResultViewModel<Post>("05X04 - Falha interna no servidor"));
        }
    }

    [HttpGet("v1/posts/category/{categorySlug}")]
    public async Task<IActionResult> GetByCategoryAsync(
        [FromRoute] string categorySlug,
        [FromServices] BlogDataContext context,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 25
    )
    {
        try
        {
            var postsCount = await context.Posts.AsNoTracking().CountAsync();

            var posts = await context
                .Posts
                .AsNoTracking()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .Where(x => x.Category.Slug == categorySlug)
                .Select(x => new ListPostsViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Slug = x.Slug,
                    LastUpdateDate = x.LastUpdateDate,
                    Category = x.Category.Name,
                    Author = $"{x.Author.Name} ({x.Author.Email})"
                })
                .Skip(page * pageSize)
                .Take(pageSize)
                .OrderByDescending(x => x.LastUpdateDate)
                .ToListAsync();

            return Ok(new ResultViewModel<dynamic>(new
            {
                total = postsCount,
                page,
                totalPages = postsCount / pageSize,
                pageSize,
                posts
            }));
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResultViewModel<List<Post>>("05X4 - Erro inesperado no servidor"));
        }
    }
}