using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MimicAPI.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimicAPI.V1.Repositories;
using MimicAPI.V1.Repositories.Contracts;
using AutoMapper;
using MimicAPI.Util;
using Microsoft.AspNetCore.Mvc.Versioning;
using MimicAPI.Util.Swagger;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;

namespace MimicAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Auto-Mapper Config
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DTOMapperProfile());
            });
            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);
            #endregion

            services.AddRazorPages();
            services.AddDbContext<MimicContext>(opt =>
            {
                opt.UseSqlite("Data Source=Database\\Mimic.db");
            });
            services.AddMvc();
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddScoped<IPalavraRepository, PalavraRepository>();
            services.AddApiVersioning(cfg =>
            {
                cfg.ReportApiVersions = true;
                cfg.ApiVersionReader = new HeaderApiVersionReader("api-version");
                cfg.AssumeDefaultVersionWhenUnspecified = true;
                cfg.DefaultApiVersion = new ApiVersion(1, 0);
            });
            services.AddSwaggerGen(cfg =>
            {
                cfg.ResolveConflictingActions(ApiDescription => ApiDescription.First());
                cfg.SwaggerDoc("v2.0", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "MimicApi - V2.0",
                    Version = "V2.0"
                });
                cfg.SwaggerDoc("v1.1", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "MimicApi - V1.1",
                    Version = "V1.1"
                });
                cfg.SwaggerDoc("v1.0", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "MimicApi - V1.0",
                    Version = "V1.0"
                });
                var caminhoProjeto = PlatformServices.Default.Application.ApplicationBasePath;
                var nomeProjeto = $"{PlatformServices.Default.Application.ApplicationName}.xml";
                var caminhoArquivoXMLComentario = Path.Combine(caminhoProjeto, nomeProjeto);
                cfg.IncludeXmlComments(caminhoArquivoXMLComentario);
                cfg.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var actionApiVersionModel = apiDesc.ActionDescriptor?.GetApiVersion();
                    // would mean this action is unversioned and should be included everywhere
                    if (actionApiVersionModel == null)
                    {
                        return true;
                    }
                    if (actionApiVersionModel.DeclaredApiVersions.Any())
                    {
                        return actionApiVersionModel.DeclaredApiVersions.Any(v => $"v{v.ToString()}" == docName);
                    }
                    return actionApiVersionModel.ImplementedApiVersions.Any(v => $"v{v.ToString()}" == docName);
                });
                cfg.OperationFilter<ApiVersionOperationFilter>();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseStatusCodePages();

            app.UseMvc();

            app.UseSwagger(); //Cria um arquivo em /swagger/v1/swagger.json
            app.UseSwaggerUI(cfg =>
            {
                cfg.SwaggerEndpoint("/swagger/v2.0/swagger.json", "MimicAPI - V2.0");
                cfg.SwaggerEndpoint("/swagger/v1.1/swagger.json", "MimicAPI - V1.1");
                cfg.SwaggerEndpoint("/swagger/v1.0/swagger.json", "MimicAPI - V1.0");
                cfg.RoutePrefix = String.Empty;
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
