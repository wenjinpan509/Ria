
using CustomerApi.Filters;
using CustomerApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();
            });
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            builder.Services.AddScoped<ValidationFilter>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // In Azure, I use an in-memory store (CustomerStoreMemory). This could later be replaced by a blob-backed implementation if needed.
            builder.Services.AddSingleton<ICustomerStore>(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                var provider = config["CustomerStore:Provider"]?.ToLower();

                return provider switch
                {
                    "memory" => new CustomerStoreMemory(),
                    "file" or null => new CustomerStoreFile(), // default to file
                    _ => throw new InvalidOperationException($"Unknown CustomerStore provider: {provider}")
                };
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseMiddleware<CustomerApi.Middlewares.GlobalExceptionHandlerMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
