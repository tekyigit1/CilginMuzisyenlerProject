using CrazyMusicians.Api.Validators;
using CrazyMusicians.Api.Dtos;
using CrazyMusicians.Api.Models;
using CrazyMusicians.Api.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CrazyMusicians.Api.Controllers;


[ApiController]
[Route("v1/musicians")]
public class MusiciansController : ControllerBase
{
    private readonly IMusicianStore _store;
    private readonly IValidator<MusicianCreateDto> _createValidator;
    private readonly IValidator<MusicianUpdateDto> _updateValidator;
    private readonly IValidator<MusicianPatchDto> _patchValidator;

    public MusiciansController(
        IMusicianStore store,
        IValidator<MusicianCreateDto> createValidator,
        IValidator<MusicianUpdateDto> updateValidator,
        IValidator<MusicianPatchDto> patchValidator)
    {
        _store = store;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _patchValidator = patchValidator;
    }

    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Musician>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Musician>> GetAll(
        [FromQuery] string? profession,
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int size = 10,
        [FromQuery] string? sort = "id,asc")
    {
        var data = _store.GetAll().AsQueryable();

        if (!string.IsNullOrWhiteSpace(profession))
            data = data.Where(m => m.Profession.Contains(profession, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(search))
            data = data.Where(m =>
                m.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                m.FunFact.Contains(search, StringComparison.OrdinalIgnoreCase));

        // sıralama: "name,asc" / "name,desc" / "id,asc"
        var parts = sort?.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
        var field = parts.Length > 0 ? parts[0].ToLowerInvariant() : "id";
        var dir = parts.Length > 1 ? parts[1].ToLowerInvariant() : "asc";

        data = (field, dir) switch
        {
            ("name", "desc") => data.OrderByDescending(m => m.Name),
            ("name", _) => data.OrderBy(m => m.Name),
            ("profession", "desc") => data.OrderByDescending(m => m.Profession),
            ("profession", _) => data.OrderBy(m => m.Profession),
            ("funfact", "desc") => data.OrderByDescending(m => m.FunFact),
            ("funfact", _) => data.OrderBy(m => m.FunFact),
            ("id", "desc") => data.OrderByDescending(m => m.Id),
            _ => data.OrderBy(m => m.Id)
        };

        var total = data.Count();
        size = Math.Clamp(size, 1, 100);
        page = Math.Max(page, 1);
        var items = data.Skip((page - 1) * size).Take(size).ToList();

        // Toplamı header'a koy
        Response.Headers["X-Total-Count"] = total.ToString();

        return Ok(items);
    }

    // GET /v1/musicians/{id}
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Musician), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Musician> GetById(int id)
    {
        var item = _store.GetById(id);
        return item is null ? NotFound() : Ok(item);
    }

    // POST /v1/musicians
    [HttpPost]
    [ProducesResponseType(typeof(Musician), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Musician>> Create([FromBody] MusicianCreateDto dto)
    {
        var result = await _createValidator.ValidateAsync(dto);
        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        var entity = new Musician
        {
            Name = dto.Name,
            Profession = dto.Profession,
            FunFact = dto.FunFact
        };

        var created = _store.Add(entity);

        // 201 + Location: /v1/musicians/{id}
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT /v1/musicians/{id} (tam güncelle)
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] MusicianUpdateDto dto)
    {
        var result = await _updateValidator.ValidateAsync(dto);
        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        var existing = _store.GetById(id);
        if (existing is null) return NotFound();

        existing.Name = dto.Name;
        existing.Profession = dto.Profession;
        existing.FunFact = dto.FunFact;

        _store.Update(existing);
        return NoContent(); // 204
    }

    // PATCH /v1/musicians/{id} (kısmi güncelle)
    [HttpPatch("{id:int}")]
    [ProducesResponseType(typeof(Musician), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Musician>> Patch(int id, [FromBody] MusicianPatchDto dto)
    {
        var validation = await _patchValidator.ValidateAsync(dto);
        if (!validation.IsValid)
        {
            validation.AddToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        var existing = _store.GetById(id);
        if (existing is null) return NotFound();

        if (dto.Name is not null) existing.Name = dto.Name;
        if (dto.Profession is not null) existing.Profession = dto.Profession;
        if (dto.FunFact is not null) existing.FunFact = dto.FunFact;

        _store.Update(existing);
        return Ok(existing);
    }

    // DELETE /v1/musicians/{id}
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var ok = _store.Delete(id);
        return ok ? NoContent() : NotFound();
    }
}
