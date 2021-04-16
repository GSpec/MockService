using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace MockService
{
    public class Startup
    {
        readonly string AllowSpecificOrigins = "_allowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(
                    name: AllowSpecificOrigins,
                    builder =>
                    {
                        builder
                            .SetIsOriginAllowedToAllowWildcardSubdomains()
                            .WithOrigins("http://*.mock.io", "http://mock.io")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            services
                .AddControllers()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Mock API",
                    Description = "Calculates a mathematical expression."
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(config =>
                {
                    config.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/json";

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            var ex = error.Error;

                            await context.Response.WriteAsync("An unexpected error occured.");
                        }
                    });
                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(AllowSpecificOrigins);
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mock API");
            });
        }
    }
}
