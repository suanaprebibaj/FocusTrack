using FluentValidation;
using FocusTrack.Domain.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Application.Sessions.Commands.CreateSession
{

    public sealed class CreateSessionCommandValidator : AbstractValidator<CreateSessionCommand>
    {
        public CreateSessionCommandValidator()
        {
            RuleFor(x => x.Topic)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.StartTime)
                .LessThan(x => x.EndTime)
                .WithMessage("StartTime must be before EndTime.");

            RuleFor(x => x.Mode)
                .NotEmpty()
                .Must(mode => Enum.TryParse<SessionMode>(mode, true, out _))
                .WithMessage("Invalid session mode.");
        }
    }
}
