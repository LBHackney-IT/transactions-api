using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using transactions_api.UseCase;
using transactions_api.V1.Boundary;
using UnitTests.V1.Gateways;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using transactions_api.Versioning;
using UnitTests.V1.Infrastructure;
using FluentValidation.AspNetCore;
using transactions_api.V1.Validation;
using transactions_api.V1.Validation.ValidatorBase;

namespace transactions_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private static List<ApiVersionDescription> _apiVersions { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddFluentValidation().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddApiVersioning(o =>
            {
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.AssumeDefaultVersionWhenUnspecified = true; // assume that the caller wants the default version if they don't specify
                o.ApiVersionReader = new UrlSegmentApiVersionReader(); // read the version number from the url segment header)
            });
            services.AddSingleton<IApiVersionDescriptionProvider, DefaultApiVersionDescriptionProvider>();

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Token",
                    new ApiKeyScheme
                    {
                        In = "header",
                        Description = "Your Hackney API Key",
                        Name = "X-Api-Key",
                        Type = "apiKey"
                    });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    {"Token", Enumerable.Empty<string>()}
                });

                //Looks at the APIVersionAttribute [ApiVersion("x")] on controllers and decides whether or not
                //to include it in that version of the swagger document
                //Controllers must have this [ApiVersion("x")] to be included in swagger documentation!!
                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var versions = apiDesc.ControllerAttributes()
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions).ToList();

                    var any = versions.Any(v => $"{v.GetFormattedApiVersion()}" == docName);
                    return any;
                });

                //Get every ApiVersion attribute specified and create swagger docs for them
                foreach (var apiVersion in _apiVersions)
                {
                    var version = $"v{apiVersion.ApiVersion.ToString()}";
                    c.SwaggerDoc(version, new Info
                    {
                        Title = $"transactions-api {version}",
                        Version = version,
                        Description = $"Transactions API version {version}. Please check older versions for depreceted endpoints."
                    });
                }

                c.CustomSchemaIds(x => x.FullName);
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                    c.IncludeXmlComments(xmlPath);
            });

            ConfigureDbContext(services);
            RegisterGateWays(services);
            RegisterUseCases(services);
            RegisterValidatorBases(services);
            RegisterValidators(services);
            services.AddMvcCore().AddDataAnnotations();
        }

        private static void ConfigureDbContext(IServiceCollection services)
        {
            var connectionString = Environment.GetEnvironmentVariable("UH_URL");

            DbContextOptionsBuilder builder = new DbContextOptionsBuilder()
                .UseSqlServer(connectionString);

            services.AddTransient<IUHContext>(s => new UhContext(builder.Options));
        }

        private static void RegisterGateWays(IServiceCollection services)
        {
            services.AddSingleton<ITransactionsGateway, TransactionsGateway>();
        }

        private static void RegisterUseCases(IServiceCollection services)
        {
            services.AddSingleton<IListTransactions, ListTransactionsUsecase>();
        }

        private static void RegisterValidatorBases(IServiceCollection services)
        {
            services.AddTransient<IPostCodeBaseValidator, PostCodeBaseValidator>();
        }

        private static void RegisterValidators(IServiceCollection services)
        {
            services.AddTransient<IGetTenancyTransactionsValidator, GetTenancyTransactionsValidator>();
            services.AddTransient<IGetTenancyDetailsValidator, GetTenancyDetailsValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            //Get All ApiVersions,
            var api = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();
            //Get All ApiVersions,
            _apiVersions = api.ApiVersionDescriptions.Select(s => s).ToList();
            //Swagger ui to view the swagger.json file
            app.UseSwaggerUI(c =>
            {
                foreach (var apiVersionDescription in _apiVersions)
                {
                    //Create a swagger endpoint for each swagger version
                    c.SwaggerEndpoint($"{apiVersionDescription.GetFormattedApiVersion()}/swagger.json",
                        $"transactions-api {apiVersionDescription.GetFormattedApiVersion()}");
                }
            });

            app.UseSwagger();

            app.UseMvc(routes =>
            {
                // SwaggerGen won't find controllers that are routed via this technique.
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
