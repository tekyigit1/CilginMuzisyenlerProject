namespace CrazyMusicians.Api.Models;

// Domain model (veritabanı varmış gibi basit liste ile tutacağız)
public class Musician
{
    // Id: Unique kimlik. In-memory store otomatik artıracak.
    public int Id { get; set; }

    // Name: Müzisyenin adı
    public string Name { get; set; } = string.Empty;

    // Profession: Meslek/rol (ör. Composer, Guitarist)
    public string Profession { get; set; } = string.Empty;

    // FunFact: Eğlenceli özelliği
    public string FunFact { get; set; } = string.Empty;
}
