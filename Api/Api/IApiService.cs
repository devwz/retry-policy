namespace Api
{
    public interface IApiService
    {
        public Task<RestResult<Profile>> GetProfileAsync(int id);
    }
}
