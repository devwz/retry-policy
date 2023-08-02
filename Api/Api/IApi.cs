using Refit;

namespace Api
{
    public interface IApi
    {
        [Get("/profiles")]
        Task<HttpResponseMessage> GetAsync([Query] int id);
    }
}
