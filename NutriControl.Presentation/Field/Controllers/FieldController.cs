using System.Net.Mime;
using _1_API.Response;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Mvc;
using NutriControl.Domain.Fields.Models.Entities;
using NutriControl.Domain.Fields.Models.Queries;
using NutriControl.Presentation.Filters;
using Presentation.Request;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FieldController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IFieldCommandService _fieldCommandService;
    private readonly IFieldQueryService _fieldQueryService;

    public FieldController(IFieldQueryService fieldQueryService, IFieldCommandService fieldCommandService, IMapper mapper)
    {
        _fieldQueryService = fieldQueryService;
        _fieldCommandService = fieldCommandService;
        _mapper = mapper;
    }

    // GET: api/Field
    /// <summary>Obtiene todos los campos activos.</summary>
    /// <remarks>
    /// GET /api/Field
    /// </remarks>
    /// <response code="200">Devuelve todos los campos.</response>
    /// <response code="404">Si no hay campos.</response>
    /// <response code="500">Si ocurre un error interno del servidor.</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<FieldResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> GetAsync()
    {
        var result = await _fieldQueryService.Handle(new GetAllFieldsQuery());

        if (result.Count == 0) return NotFound();

        return Ok(result);
    }

    // GET: api/Field/id
    /// <summary>Obtiene un campo por su ID.</summary>
    /// <param name="id">ID del campo.</param>
    /// <response code="200">Devuelve el campo.</response>
    /// <response code="404">Si el campo no se encuentra.</response>
    /// <response code="500">Si ocurre un error interno del servidor.</response>
    [HttpGet("{id}", Name = "GetFieldById")]
    [CustomAuthorize("Farmer")]
    [ProducesResponseType(typeof(FieldResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetAsync(int id)
    {
        try
        {
            var result = await _fieldQueryService.Handle(new GetFieldByIdQuery(id));

            if (result == null)
                return NotFound();

            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    // GET: api/Field/name/{name}
    /// <summary>Obtiene un campo por su nombre.</summary>
    /// <param name="name">Nombre del campo.</param>
    /// <response code="200">Devuelve el campo.</response>
    /// <response code="404">Si el campo no se encuentra.</response>
    /// <response code="500">Si ocurre un error interno del servidor.</response>
    [HttpGet("name/{name}", Name = "GetFieldByName")]
    [CustomAuthorize("Farmer")]
    [ProducesResponseType(typeof(FieldResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetFieldByNameAsync(string name)
    {
        try
        {
            var result = await _fieldQueryService.Handle(new GetFieldByNameQuery(name));

            if (result == null)
                return NotFound();

            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    // GET: api/Field/user/{userId}
    /// <summary>Obtiene los campos de un usuario específico.</summary>
    /// <param name="userId">ID del usuario.</param>
    /// <response code="200">Devuelve los campos del usuario.</response>
    /// <response code="404">Si no se encuentra ningún campo para el usuario.</response>
    /// <response code="500">Si ocurre un error interno del servidor.</response>
    [HttpGet("user/{userId}", Name = "GetFieldsByUserId")]
    [CustomAuthorize("Farmer")]
    [ProducesResponseType(typeof(FieldResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetByUserIdAsync(int userId)
    {
        try
        {
            var result = await _fieldQueryService.Handle(new GetFieldsByUserIdQuery(userId));

            if (result == null)
                return NotFound();

            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    // POST: api/Field
    /// <summary>
    /// Crea un nuevo campo para el usuario autenticado.
    /// </summary>
    /// <remarks>
    /// Ejemplo de solicitud:
    ///
    ///     POST /api/Field
    ///     {
    ///        "name": "Campo 1",
    ///        "location": "Ubicación 1",
    ///        "soilType": "Arenoso",
    ///        "elevation": 500
    ///     }
    ///
    /// </remarks>
    /// <param name="command">Datos del campo a crear.</param>
    /// <returns>ID del campo recién creado.</returns>
    /// <response code="201">Devuelve el ID del campo creado.</response>
    /// <response code="400">Si el campo tiene propiedades inválidas.</response>
    /// <response code="409">Si ya existe un campo con el mismo nombre.</response>
    /// <response code="500">Si ocurre un error inesperado.</response>
    [HttpPost]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> PostAsync([FromBody] CreateFieldCommand command)
    {
        var user = HttpContext.Items["User"] as User;
        if (user == null) return Unauthorized();

        command.UserId = user.Id;

        if (!ModelState.IsValid) return BadRequest();

        var existingField = await _fieldQueryService.FindFieldByNameAsync(command.Name);
        if (existingField != null)
        {
            return Conflict("Ya existe un campo con el mismo nombre.");
        }

        var result = await _fieldCommandService.Handle(command);

        return StatusCode(StatusCodes.Status201Created, result);
    }

    // PUT: api/Field/{id}
    /// <summary>
    /// Actualiza un campo existente por su ID.
    /// </summary>
    /// <remarks>
    /// Ejemplo de solicitud:
    ///
    ///     PUT /api/Field/5
    ///     {
    ///        "name": "Campo Actualizado",
    ///        "location": "Nueva Ubicación",
    ///        "soilType": "Arcilloso",
    ///        "elevation": 600
    ///     }
    ///
    /// </remarks>
    /// <param name="id">ID del campo a actualizar.</param>
    /// <param name="command">Datos actualizados del campo.</param>
    /// <response code="200">Campo actualizado correctamente.</response>
    /// <response code="400">Si el campo tiene propiedades inválidas.</response>
    /// <response code="404">Si el campo no se encuentra.</response>
    /// <response code="500">Si ocurre un error inesperado.</response>
    [HttpPut("{id}")]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> PutAsync(int id, [FromBody] UpdateFieldCommand command)
    {
        command.Id = id;
        if (!ModelState.IsValid) return StatusCode(StatusCodes.Status400BadRequest);

        var result = await _fieldCommandService.Handle(command);

        if (!result) return NotFound();

        return Ok();
    }

    // DELETE: api/Field/{id}
    /// <summary>
    /// Elimina un campo por su ID.
    /// </summary>
    /// <param name="id">ID del campo a eliminar.</param>
    /// <response code="200">Campo eliminado correctamente.</response>
    /// <response code="404">Si el campo no se encuentra.</response>
    /// <response code="500">Si ocurre un error inesperado.</response>
    [HttpDelete("{id}")]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var command = new DeleteFieldCommand { Id = id };

        var result = await _fieldCommandService.Handle(command);

        if (!result) return NotFound();

        return Ok();
    }
}