using Castle.Core.Logging;
using IronSoccerDDD.Api.Handlers;
using IronSoccerDDD.Core;
using IronSoccerDDD.Core.Interfaces;
using IronSoccerDDD.Core.IRepositories;
using IronSoccerDDD.Infraestructure;
using IronSoccerDDD.Infraestructure.DomainEvents;
using IronSoccerDDD.Infraestructure.Repositories;
using IronSoccerDDD.Infraestructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

//TEST GIT
namespace IronSoccerDDD.Api
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
            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IMatchRepository, MatchRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEmailSender, EmailSenderService>();
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

            /*
             * TODO: It can exist N events & handlers, we should add N rules like this:
             * services.AddScoped<IHandler<TeamJoinsNewPlayerEvent>, TeamJoinsNewPlayerHandler>();
             * To avoid that - We will use Scrutor to scan assemblies
            */
            services.Scan(scan => scan
                .FromAssembliesOf(typeof(IHandler<>))
                .AddClasses(c => c.AssignableTo(typeof(IHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());


            services.AddDbContext<ApplicationDbContext>(
                options => options
                .UseSqlServer(Configuration.GetConnectionString("DbConnection"))
                .UseLazyLoadingProxies());


            services.AddControllers()
                 .AddNewtonsoftJson(setup => 
                    setup.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            app.UseIronExceptionHandler(logger);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
