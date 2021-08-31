using ExemploApiCatalogoJogos.Middleware;
using ExemploApiCatalogoJogos.Models.AutoMapper;
using ExemploApiCatalogoJogos.Models.Validators;
//using ExemploApiCatalogoJogos.Repositories;
using ExemploApiCatalogoJogos.Services;
using ExemploApiCatalogoJogos.Settings;
using ExemploApiCatalogoJogos.Settings.Interfaces;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace ExemploApiCatalogoJogos
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection service)
        {
            service.AddSingleton<IJogoService, JogoService>();
            //services.AddScoped<IJogoRepository, JogoRepository>();

            service.AddControllers()
                .AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<JogoInputModelValidator>());

            service.AddAutoMapper(typeof(AutoMapperConfig));
            

            service.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiCatalogoJogos", Version = "v1" });

                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                c.IncludeXmlComments(Path.Combine(basePath, fileName));
            });

            service.Configure<JogosStoreDatabaseSettings>(Configuration.GetSection(nameof(JogosStoreDatabaseSettings)));
            service.AddSingleton<IJogosStoreDatabaseSettings>(s => s.GetRequiredService<IOptions<JogosStoreDatabaseSettings>>().Value);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiCatalogoJogos v1"));
            }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
