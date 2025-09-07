namespace CrazyMusicians.Api.Dtos;

// POST /v1/musicians gövdesi
public class MusicianCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Profession { get; set; } = string.Empty;
    public string FunFact { get; set; } = string.Empty;
}
