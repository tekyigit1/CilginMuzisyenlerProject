using CrazyMusicians.Api.Models;

namespace CrazyMusicians.Api.Services;

// Basit bir in-memory "depo" arabirimi
public interface IMusicianStore
{
    List<Musician> GetAll();        // tüm veriler
    Musician? GetById(int id);      // tekil
    Musician Add(Musician musician); // ekle
    bool Update(Musician musician);  // tam güncelle
    bool Delete(int id);             // sil
}
