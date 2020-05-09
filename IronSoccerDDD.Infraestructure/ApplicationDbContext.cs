using IronSoccerDDD.Core.Entities;
using IronSoccerDDD.Core.Interfaces;
using IronSoccerDDD.Core.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace IronSoccerDDD.Infraestructure
{
    public class ApplicationDbContext: DbContext
    {
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        private readonly bool _useConsoleEfLogger;

        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Country> Countries { get; set; }

        public ApplicationDbContext(DbContextOptions options,
            IDomainEventDispatcher domainEventDispatcher,
            IConfiguration configuration)
            :base(options)
        {
            _domainEventDispatcher = domainEventDispatcher;
            _useConsoleEfLogger = Convert.ToBoolean(configuration[ConfigurationKeys.UseConsoleEfLogger]);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter((category, level) =>
                        category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
                    .AddConsole();
            });

            if (_useConsoleEfLogger)
            {
                optionsBuilder
                    .UseLoggerFactory(loggerFactory)
                    .EnableSensitiveDataLogging();
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //TODO: Global Apply Configurations
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            List<BaseEntity> entities = ChangeTracker
                .Entries()
                .Where(x => x.Entity is BaseEntity)
                .Select(x => (BaseEntity)x.Entity)
                .Where(e => e.DomainEvents.Any())
                .ToList();

            int result = await base.SaveChangesAsync(cancellationToken);

            foreach (var entity in entities)
            {
                _domainEventDispatcher.Dispatch(entity.DomainEvents);
                entity.ClearDomainEvents();
            }

            return result;
        }

    }
}
