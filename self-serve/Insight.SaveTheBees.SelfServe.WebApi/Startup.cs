using AutoMapper;
using IdentityServer4.AccessTokenValidation;
using Insight.SaveTheBees.SelfServe.WebApi.Models.Application.Data;
using Insight.SaveTheBees.SelfServe.WebApi.Models.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Insight.SaveTheBees.SelfServe.WebApi
{
    /// <summary>
    /// The class that contains the methods required to configures the services and
    /// middleware required by the application.
    /// </summary>
    public class Startup
    {
        #region Properties

        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Gets the application hosting environment.
        /// </summary>
        public IHostingEnvironment Environment { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a new instance of <see cref="Startup" /> and set the internal
        /// components.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="environment">The application hosting environment.</param>
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Configures the services for the application.
        /// </summary>
        /// <param name="services">The collection of services for the container.</param>
        /// <remarks>This method gets called by the runtime. Use this method to add services to the container.</remarks>
        public void ConfigureServices(IServiceCollection services)
        {
            // Register EF contexts
            services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString(Settings.ConnectionStrings.Identity)));
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString(Settings.ConnectionStrings.Application)));

            // Register identity components
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            // Register the mapper
            services.AddAutoMapper(Models.MapperConfiguration.CreateConfiguration());

            // Register the MVC components
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Register the identity services
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            var identityServerbuilder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
            .AddAspNetIdentity<User>()
            .AddConfigurationStore(options => options.ConfigureDbContext = builder => builder.UseSqlServer(Configuration.GetConnectionString(Settings.ConnectionStrings.Identity), sql => sql.MigrationsAssembly(migrationsAssembly)))
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = builder => builder.UseSqlServer(Configuration.GetConnectionString(Settings.ConnectionStrings.Identity), sql => sql.MigrationsAssembly(migrationsAssembly));
                options.EnableTokenCleanup = true;
                options.TokenCleanupInterval = 30;
            });

            if (Environment.IsDevelopment())
            {
                identityServerbuilder.AddDeveloperSigningCredential();
            }
            else
            {
                throw new Exception("need to configure key material");
            }

            // Register the authentication
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.Authority = Configuration[Settings.Authentication.TokenIssuer];
                    options.ApiName = Configuration[Settings.Authentication.Audience];
                });

            // Configure dependency injection for services
            ConfigureAppServices(services);
        }        

        /// <summary>
        /// Configures the HTTP request pipeline for the application.
        /// </summary>
        /// <param name="app">The application request pipeline builder.</param>
        /// <param name="env">The hosting environment.</param>
        /// <remarks>This method gets called by the runtime. Use this method to configure the HTTP request pipeline.</remarks>
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

            app.UseHttpsRedirection();
            app.UseIdentityServer();
            app.UseMvc();
        }

        private void ConfigureAppServices(IServiceCollection services)
        {
            // Retrieve all service types and register against the interface
            var types = typeof(Startup).GetTypeInfo().Assembly.GetTypes().Where(x => !x.IsInterface && x.MemberType == MemberTypes.TypeInfo && x.Namespace == "Insight.SaveTheBees.SelfServe.WebApi.Services").ToList();
            foreach (var serviceType in types)
            {
                var interfaceType = serviceType.GetInterfaces().FirstOrDefault();
                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, serviceType);
                }
                else
                {
                    services.AddScoped(serviceType);
                }                
            }
        }

        #endregion
    }
}