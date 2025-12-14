using FocusTrack.Domain.Sessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Infrastructure.Persistence.Configurations
{
    public class SessionShareConfiguration : IEntityTypeConfiguration<SessionShare>
    {
        public void Configure(EntityTypeBuilder<SessionShare> builder)
        {
            builder.ToTable("SessionShares");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.SessionId).IsRequired();
            builder.Property(s => s.RecipientUserId).IsRequired();
            builder.Property(s => s.IsRevoked).HasDefaultValue(false);
        }
    }
}
