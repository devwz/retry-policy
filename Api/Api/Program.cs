using Microsoft.AspNetCore.Mvc;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.RegisterHttpClient();

            builder.Services.AddTransient<IApiService, ApiService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapGet("/profiles", async (int id) =>
            {
                IApiService service = app.Services.GetRequiredService<IApiService>();

                RestResult<Profile> result = await service.GetProfileAsync(id);

                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return Results.Ok(result.Data);
                }
                else
                {
                    return Results.StatusCode((int)result.StatusCode);
                }
            });

            app.Run();
        }
    }
}