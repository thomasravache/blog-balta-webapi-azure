using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModels.Categories;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Blog.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("v1/categories")]
        public async Task<IActionResult> GetAsync(
            [FromServices] BlogDataContext context,
            [FromServices] IMemoryCache cache
        )
        {
            try
            {
                // Todo cache possui um nome de chave

                /*
                    No caso abaixo as categorias são carregadas do banco a primeira vez,
                    armazenadas no cache da aplicação e nos próximos 20 minutos essa requisição buscará a informação do cache e não do banco
                    após 20 minutos, o cache expirará, portanto necessitando de outra consulta no banco para retornar as informações e recomeçar o processo
                */
                var categories = cache.GetOrCreate("CategoriesCache", entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20);

                    return GetCategories(context);
                });

                return Ok(new ResultViewModel<List<Category>>(categories));
            }
            catch
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ResultViewModel<List<Category>>("05XE1 - Falha interna no servidor")
                    );
            }
        }

        // método criado apenas para fins didáticos
        private List<Category> GetCategories(BlogDataContext context)
        => context.Categories.ToList();

        [HttpGet("v1/categories/{id:int}")] // dá pra tipar os parâmetros de rota
        public async Task<IActionResult> GetByIdAsync(
            [FromRoute] int id,
            [FromServices] BlogDataContext context
        )
        {
            try
            {
                var category = await context.Categories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado"));

                return Ok(new ResultViewModel<Category>(category));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResultViewModel<Category>("05XE2 - Falha interna no servidor"));
            }
        }

        [HttpPost("v1/categories")] // dá pra tipar os parâmetros de rota
        public async Task<IActionResult> PostAsync(
            [FromBody] EditorCategoryViewModel model,
            [FromServices] BlogDataContext context
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

            try
            {
                var category = new Category
                {
                    Name = model.Name,
                    Slug = model.Slug
                };

                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();

                return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResultViewModel<Category>("05XE9 - Não foi possível incluir a categoria"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResultViewModel<Category>("05XE10 - Falha interna no servidor"));
            }
        }

        [HttpPut("v1/categories/{id:int}")] // dá pra tipar os parâmetros de rota
        public async Task<IActionResult> PutAsync(
            [FromRoute] int id,
            [FromBody] EditorCategoryViewModel model,
            [FromServices] BlogDataContext context
        )
        {
            try
            {

                var category = await context.Categories
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado"));

                category.Name = model.Name;
                category.Slug = model.Slug;

                context.Categories.Update(category);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResultViewModel<Category>("05XE8 - Não foi possível alterar a categoria"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResultViewModel<Category>("05XE11 - Falha interna no servidor"));
            }
        }

        [HttpDelete("v1/categories/{id:int}")] // dá pra tipar os parâmetros de rota
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] int id,
            [FromServices] BlogDataContext context
        )
        {
            try
            {
                var category = await context.Categories
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado"));

                context.Categories.Remove(category);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResultViewModel<Category>("05XE7 - Não foi possível excluir a categoria"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResultViewModel<Category>("05XE12 - Falha interna no servidor"));
            }
        }
    }
}