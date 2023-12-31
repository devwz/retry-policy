﻿using Polly.CircuitBreaker;
using Polly.Timeout;
using System.Text.Json;

namespace Api
{
    public class ApiService : IApiService
    {
        private readonly IApi _provider;

        public ApiService(IApi provider)
        {
            _provider = provider;
        }

        public async Task<RestResult<Profile>> GetProfileAsync(int id)
        {
            try
            {
                HttpResponseMessage response = await _provider.GetAsync(id);

                if (response.IsSuccessStatusCode)
                {
                    Profile result = JsonSerializer.Deserialize<Profile>(
                        await response.Content.ReadAsStringAsync());

                    return new RestResult<Profile>(result, response.StatusCode);
                }

                return new RestResult<Profile>(response.StatusCode);
            }
            catch (TimeoutRejectedException)
            {
                throw;
            }
            catch (BrokenCircuitException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
