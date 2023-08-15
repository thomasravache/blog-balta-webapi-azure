using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Blog.Extensions
{
    /*
        Classes e métodos de extensão TEM que ser estáticos (static)
    */
    public static class ModelStateExtension
    {
        /*
            Colocar o "this" no parâmetro transforma-o em um método de extensão
        */
        public static List<string> GetErrors(this ModelStateDictionary modelState)
        {
            List<string> result = new(); // ou var result = new List<string>();

            foreach (var item in modelState.Values)
                result.AddRange(item.Errors.Select(error => error.ErrorMessage));

            return result;
        }
    }
}