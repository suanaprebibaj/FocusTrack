using FocusTrack.Application.Common.Interfaces;
using FocusTrack.Domain.Primitives;
using FocusTrack.Domain.Sessions;
using FocusTrack.Domain.Users;
using FocusTrack.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace FocusTrack.Infrastructure.Persistence
{
    public class AppDbContext : DbContext, IUnitOfWork
    {
        private readonly IDomainEventSerializer _serializer;

        public DbSet<Session> Sessions => Set<Session>();
        public DbSet<User> Users => Set<User>();
        public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

        public AppDbContext(DbContextOptions<AppDbContext> options,
            IDomainEventSerializer serializer)
            : base(options)
        {
            _serializer = serializer;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            var domainEvents = ChangeTracker
                .Entries<AggregateRoot<Guid>>()
                .SelectMany(e => e.Entity.GetDomainEvents())
                .ToList();

            var result = await base.SaveChangesAsync(ct);

            if (domainEvents.Count > 0)
            {
                foreach (var domainEvent in domainEvents)
                {
                    OutboxMessages.Add(OutboxMessage.FromDomainEvent(domainEvent, _serializer));
                }

                foreach (var entry in ChangeTracker.Entries<AggregateRoot<Guid>>())
                {
                    entry.Entity.ClearDomainEvents();
                }

                await base.SaveChangesAsync(ct);
            }

            return result;
        }
    }
}
