using FocusTrack.Domain.Sessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FocusTrack.Infrastructure.Persistence.Configurations
{
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable("Sessions");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Topic)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(s => s.UserId)
                .IsRequired();

            builder.Property(s => s.StartTime)
                .IsRequired();

            builder.Property(s => s.EndTime)
                .IsRequired();

            builder.Property(s => s.Mode)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(s => s.IsDailyGoalAchieved)
                .IsRequired();

            builder.OwnsOne(s => s.Duration, b =>
            {
                b.Property(d => d.Value)
                 .HasColumnName("DurationMinutes")
                 .HasPrecision(10, 2);
            });

          
            builder.Ignore(s => s.Shares);

           
            builder.HasMany(typeof(SessionShare), "_shares")
                .WithOne()
                .HasForeignKey("SessionId")
                .OnDelete(DeleteBehavior.Cascade);

           
            builder.Metadata
                .FindNavigation("_shares")!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
