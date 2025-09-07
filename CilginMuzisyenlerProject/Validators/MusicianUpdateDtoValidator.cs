using CrazyMusicians.Api.Dtos;
using FluentValidation;

namespace CrazyMusicians.Api.Validators;

public class MusicianUpdateDtoValidator : AbstractValidator<MusicianUpdateDto>
{
    public MusicianUpdateDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(80);
        RuleFor(x => x.Profession).NotEmpty().MaximumLength(80);
        RuleFor(x => x.FunFact).NotEmpty().MaximumLength(200);
    }
}
