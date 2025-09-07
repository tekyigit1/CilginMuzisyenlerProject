using CrazyMusicians.Api.Dtos;
using FluentValidation;

namespace CrazyMusicians.Api.Validators;

// Basit doğrulamalar (zorunlu alanlar + uzunluk)
public class MusicianCreateDtoValidator : AbstractValidator<MusicianCreateDto>
{
    public MusicianCreateDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(80);
        RuleFor(x => x.Profession).NotEmpty().MaximumLength(80);
        RuleFor(x => x.FunFact).NotEmpty().MaximumLength(200);
    }
}
