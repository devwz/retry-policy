using Refit;

namespace Api
{
    public interface IApi
    {
        [Get("/profiles")]
        Task<HttpResponseMessage> GetAsync();

        [Get("/profiles/{id}")]
        Task<HttpResponseMessage> GetAsync([AliasAs("id")] int id);
    }
}
