using CrazyMusicians.Api.Dtos;
using FluentValidation;

namespace CrazyMusicians.Api.Validators;

// PATCH: Gönderilen alanlar varsa doğrulanır (null ise yok say)
public class MusicianPatchDtoValidator : AbstractValidator<MusicianPatchDto>
{
    public MusicianPatchDtoValidator()
    {
        When(x => x.Name is not null, () =>
        {
            RuleFor(x => x.Name!).NotEmpty().MaximumLength(80);
        });

        When(x => x.Profession is not null, () =>
        {
            RuleFor(x => x.Profession!).NotEmpty().MaximumLength(80);
        });

        When(x => x.FunFact is not null, () =>
        {
            RuleFor(x => x.FunFact!).NotEmpty().MaximumLength(200);
        });
    }
}
