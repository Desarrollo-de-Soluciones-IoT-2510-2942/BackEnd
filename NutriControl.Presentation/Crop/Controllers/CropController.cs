using System.Net.Mime;
using _1_API.Response;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Mvc;
using NutriControl.Domain.Crop.Models.Queries;
using NutriControl.Domain.Fields.Models.Queries;
using NutriControl.Presentation.Filters;
using Presentation.Request;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CropController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICropCommandService _cropCommandService;
    private readonly ICropQueryService _cropQueryService;
    private readonly IFieldQueryService _fieldQueryService;

    public CropController(ICropQueryService cropQueryService, ICropCommandService cropCommandService,
        IFieldQueryService fieldQueryService,
        IMapper mapper)
    {
        _cropQueryService = cropQueryService;
        _cropCommandService = cropCommandService;
        _fieldQueryService = fieldQueryService;
        _mapper = mapper;
    }


    // GET: api/Crop
    /// <summary>Obtain all the active crops</summary>
    /// <remarks>
    /// GET /api/Crop
    /// </remarks>
    /// <response code="200">Returns all the crops</response>
    /// <response code="404">If there are no crops</response>
    /// <response code="500">If there is an internal server error</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<CropResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> GetAsync()
    {
        var result = await _cropQueryService.Handle(new GetAllCropsQuery());
    
        if (result.Count == 0) return NotFound();

        return Ok(result);
    }

    // GET: api/Crop/id
    /// <summary>Obtain a crop by its ID</summary>
    /// <param name="id">Crop ID</param>
    /// <response code="200">Returns the crop</response>
    /// <response code="404">If the crop is not found</response>
    /// <response code="500">If there is an internal server error</response>
    [HttpGet("{id}", Name = "GetCropById")]
    [CustomAuthorize("Farmer")]
    [ProducesResponseType(typeof(CropResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetAsync(int id)
    {
        try
        {
            var result = await _cropQueryService.Handle(new GetCropByIdQuery(id));

            if (result == null)
                return NotFound();

            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
    // GET: api/Crop/field/id
    /// <summary>Obtain the active crop for a specific field</summary>
    /// <param name="fieldId">Field ID</param>
    /// <response code="200">Returns the crop for the field</response>
    /// <response code="404">If no crop is found for the field</response>
    /// <response code="500">If there is an internal server error</response>
    [HttpGet("field/{fieldId}", Name = "GetCropByFieldId")]
    [CustomAuthorize("Farmer")]
    [ProducesResponseType(typeof(CropResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetByFieldIdAsync(int fieldId)
    {
        try
        {
            var result = await _cropQueryService.Handle(new GetCropByFieldId(fieldId));

            if (result == null)
                return NotFound();

            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    
    // POST: api/Crop
    /// <summary>
    /// Crea un nuevo Crop para el campo del usuario autenticado.
    /// </summary>
    /// <remarks>
    /// Ejemplo de solicitud:
    ///
    ///     POST /api/Crop
    ///     {
    ///        "cropType": "Wheat",
    ///        "quantity": 100,
    ///        "status": "Active"
    ///     }
    ///
    /// </remarks>
    /// <param name="command">El cultivo a crear</param>
    /// <returns>El ID del cultivo recién creado</returns>
    /// <response code="201">Devuelve el ID del cultivo creado</response>
    /// <response code="400">Si el cultivo tiene propiedades inválidas</response>
    /// <response code="404">Si no se encuentra un campo para el usuario</response>
    /// <response code="500">Si ocurre un error inesperado</response>
    [HttpPost]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> PostAsync([FromBody] CreateCropCommand command)
    {
        var user = HttpContext.Items["User"] as User;
        if (user == null) return Unauthorized();

        if (!ModelState.IsValid) return BadRequest(ModelState);

        // Obtener el campo asociado al usuario
        var field = await _fieldQueryService.GetFieldByUserIdDirectAsync(user.Id);
        if (field == null) return NotFound("No se encontró un campo asociado al usuario.");

        // Asignar el FieldId al comando
        command.FieldId = field.Id;

        // Crear el cultivo
        var result = await _cropCommandService.Handle(command, user.Id);

        return StatusCode(StatusCodes.Status201Created, result);
    }
    

    // PUT: api/Crop/id
    /// <summary>
    /// Updates an existing Crop by its ID.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /api/Crop/5
    ///     {
    ///        "cropType": "Wheat",
    ///        "quantity": 150,
    ///        "status": "Harvested"
    ///     }
    ///
    /// </remarks>
    /// <param name="id">The ID of the crop to update</param>
    /// <param name="command">The updated crop data</param>
    /// <response code="200">Crop updated successfully</response>
    /// <response code="400">If the crop has invalid properties</response>
    /// <response code="404">If the crop is not found</response>
    /// <response code="500">Unexpected error</response>
    [HttpPut("{id}")]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> PutAsync(int id, [FromBody] UpdateCropCommand command)
    {
        command.Id = id;
        if (!ModelState.IsValid) return StatusCode(StatusCodes.Status400BadRequest);

        var result = await _cropCommandService.Handle(command);

        if (!result) return NotFound();

        return Ok();
    }

    // DELETE: api/Crop/5
    /// <summary>
    /// Deletes a crop by its ID.
    /// </summary>
    /// <param name="id">The ID of the crop to delete</param>
    /// <response code="200">Crop deleted successfully</response>
    /// <response code="404">If the crop is not found</response>
    /// <response code="500">Unexpected error</response>
    [HttpDelete("{id}")]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var command = new DeleteCropCommand { Id = id };

        var result = await _cropCommandService.Handle(command);

        if (!result) return NotFound();

        return Ok();
    }
}