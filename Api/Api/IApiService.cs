namespace Api
{
    public interface IApiService
    {
        public Task<RestResult<List<Profile>>> GetProfilesAsync();

        public Task<RestResult<Profile>> GetProfileAsync(int id);
    }
}
