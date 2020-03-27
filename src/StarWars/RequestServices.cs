using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
namespace StarWars
{
    public interface IScoped<T>
    {
        T Instance { get; }
    }

    public class HttpContextServiceLocator<T> : IScoped<T>
    {
        private readonly IHttpContextAccessor accessor;

        public HttpContextServiceLocator(IHttpContextAccessor accessor) => this.accessor = accessor;

        public T Instance { get => accessor.HttpContext.RequestServices.GetRequiredService<T>(); }
    }
}
