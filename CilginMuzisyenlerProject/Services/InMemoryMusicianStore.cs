using CrazyMusicians.Api.Models;
using System;

namespace CrazyMusicians.Api.Services;

// Uygulama kapanınca veri sıfırlanır 
public class InMemoryMusicianStore : IMusicianStore
{
    private readonly List<Musician> _musicians = new();
    private int _nextId = 1;

    public InMemoryMusicianStore()
    {
        // Örnek başlangıç verisi (10 kayıt)
        Seed();
    }

    public List<Musician> GetAll() => _musicians;

    public Musician? GetById(int id) => _musicians.FirstOrDefault(m => m.Id == id);

    public Musician Add(Musician musician)
    {
        musician.Id = _nextId++;
        _musicians.Add(musician);
        return musician;
    }

    public bool Update(Musician musician)
    {
        var existing = GetById(musician.Id);
        if (existing is null) return false;

        existing.Name = musician.Name;
        existing.Profession = musician.Profession;
        existing.FunFact = musician.FunFact;
        return true;
    }

    public bool Delete(int id)
    {
        var item = GetById(id);
        if (item is null) return false;
        _musicians.Remove(item);
        return true;
    }

    private void Seed()
    {
        var seed = new (string Name, string Profession, string FunFact)[]
        {
            ("Ahmet Calgi", "Multi-Instrumentalist", "Always plays the wrong note but makes it fun"),
            ("Zeynep Melodi", "Pop Songwriter", "Mishears lyrics, still very popular"),
            ("Cemil Akorist", "Guitarist", "Changes chords often, oddly talented"),
            ("Fatma Nota", "Note Producer", "Creates surprising note sequences"),
            ("Hasan Ritim", "Drummer", "Fits any rhythm, in a funny way"),
            ("Ali Perde", "Scale Master", "Loves weird scales, brings surprises"),
            ("Ayse Rezonans", "Resonance Tuner", "Finds resonance everywhere—sometimes noisy"),
            ("Tonalama Merdankasi", "Tonality Wizard", "Tonality changes create bizarre but fun vibes"),
            ("Selin Akor", "Chord Magician", "Chord changes add fresh air on stage"),
            ("Murat Ton", "Conductor", "The differences in intonation are sometimes funny, but quite interesting")
        };

        foreach (var s in seed)
        {
            Add(new Musician { Name = s.Name, Profession = s.Profession, FunFact = s.FunFact });
        }
    }
}
