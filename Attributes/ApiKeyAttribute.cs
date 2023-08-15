using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blog.Attributes;

/*
    Cria um atributo que pode ser usado em métodos de algum serviço externo (um robô por exemplo)
    para poder ter acesso a um método ou um controller específico da api sem precisar se autenticar
    via token JWT, apenas fazendo a comparação se quem está requisitando determinado endpoint da API
    possui conhecimento dessa chave

    TOMAR CUIDADO E FORNECER ESSE ATRIBUTO APENAS A MÉTODOS QUE NÃO SEJAM MUITO PERIGOSOS
*/
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)] // atributo valido para classes e para métodos
public class ApiKeyAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Query.TryGetValue(Configuration.ApiKeyName, out var extractedApiKey))
        {
            context.Result = new ContentResult()
            {
                StatusCode = StatusCodes.Status401Unauthorized,
                Content = "Api não encontrada"
            };

            return;
        }

        if (!Configuration.ApiKey.Equals(extractedApiKey))
        {
            context.Result = new ContentResult()
            {
                StatusCode = StatusCodes.Status403Forbidden,
                Content = "Acesso não autorizado"
            };

            return;
        }

        await next();
    }
}