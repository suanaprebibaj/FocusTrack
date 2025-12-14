using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Domain.Sessions
{
    public sealed class DurationMinutes : IEquatable<DurationMinutes>
    {
        public decimal Value { get; }

        private DurationMinutes(decimal value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Duration cannot be negative.");

            if (value > 999.99m)
                throw new ArgumentOutOfRangeException(nameof(value), "Duration too large.");

            Value = decimal.Round(value, 2, MidpointRounding.AwayFromZero);
        }

        public static DurationMinutes FromTimes(DateTimeOffset start, DateTimeOffset end)
        {
            if (end <= start)
                throw new ArgumentException("EndTime must be after StartTime.");

            var totalMinutes = (decimal)(end - start).TotalMinutes;
            return new DurationMinutes(totalMinutes);
        }

        public static DurationMinutes FromDecimal(decimal value) => new(value);

        public bool Equals(DurationMinutes? other)
            => other is not null && Value == other.Value;

        public override bool Equals(object? obj) => Equals(obj as DurationMinutes);

        public override int GetHashCode() => Value.GetHashCode();

        public static implicit operator decimal(DurationMinutes d) => d.Value;

        public override string ToString() => $"{Value:0.00}";
    }
}
