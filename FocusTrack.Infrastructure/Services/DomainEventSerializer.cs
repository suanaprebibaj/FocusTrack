using FocusTrack.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FocusTrack.Infrastructure.Services
{
    public interface IDomainEventSerializer
    {
        string Serialize(IDomainEvent @event);

    }

    public class DomainEventSerializer : IDomainEventSerializer
    {
        private readonly JsonSerializerOptions _options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        public string Serialize(IDomainEvent @event)
        {
            var wrapper = new
            {
                type = @event.GetType().AssemblyQualifiedName,
                data = @event
            };

            return JsonSerializer.Serialize(wrapper, _options);
        }
    }
}
