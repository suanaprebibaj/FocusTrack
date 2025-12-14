using FluentValidation;
using FocusTrack.Domain.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Application.Sessions.Commands.UpdateSession
{
    public sealed class UpdateSessionCommandValidator : AbstractValidator<UpdateSessionCommand>
    {
        public UpdateSessionCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Topic)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.StartTime)
                .LessThan(x => x.EndTime);

            RuleFor(x => x.Mode)
                .Must(m => Enum.TryParse<SessionMode>(m, true, out _))
                .WithMessage("Invalid session mode.");
        }
    }

}
