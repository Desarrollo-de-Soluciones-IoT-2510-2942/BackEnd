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
    /// <summary>Obtiene todos los cultivos activos.</summary>
    /// <remarks>
    /// GET /api/Crop
    /// </remarks>
    /// <response code="200">Devuelve todos los cultivos.</response>
    /// <response code="404">Si no hay cultivos.</response>
    /// <response code="500">Si ocurre un error interno del servidor.</response>
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
    /// <summary>Obtiene un cultivo por su ID.</summary>
    /// <param name="id">ID del cultivo.</param>
    /// <response code="200">Devuelve el cultivo.</response>
    /// <response code="404">Si el cultivo no se encuentra.</response>
    /// <response code="500">Si ocurre un error interno del servidor.</response>
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
    /// <summary>Obtiene los cultivos activos para un campo específico.</summary>
    /// <param name="fieldId">ID del campo.</param>
    /// <response code="200">Devuelve los cultivos del campo.</response>
    /// <response code="404">Si no se encuentra ningún cultivo para el campo.</response>
    /// <response code="500">Si ocurre un error interno del servidor.</response>
    [HttpGet("field/{fieldId}", Name = "GetCropsByFieldId")]
    [CustomAuthorize("Farmer")]
    [ProducesResponseType(typeof(CropResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetByFieldIdAsync(int fieldId)
    {
        try
        {
            var result = await _cropQueryService.Handle(new GetCropsByFieldId(fieldId));

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
    /// Crea un nuevo cultivo para un campo específico.
    /// </summary>
    /// <remarks>
    /// Ejemplo de solicitud:
    ///
    ///     POST /api/Crop
    ///     {
    ///        "fieldName": "Campo1",
    ///        "cropType": "Trigo",
    ///        "quantity": 100,
    ///        "status": "Activo"
    ///     }
    ///
    /// </remarks>
    /// <param name="command">El cultivo a crear.</param>
    /// <returns>El ID del cultivo recién creado.</returns>
    /// <response code="201">Devuelve el ID del cultivo creado.</response>
    /// <response code="400">Si el cultivo tiene propiedades inválidas.</response>
    /// <response code="404">Si no se encuentra un campo con el nombre especificado.</response>
    /// <response code="500">Si ocurre un error inesperado.</response>
    [HttpPost]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> PostAsync([FromBody] CreateCropCommand command)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var field = await _fieldQueryService.FindFieldByNameAsync(command.FieldName);
        if (field == null) return NotFound("No se encontró un campo con el nombre especificado.");

        command.FieldId = field.Id;

        // Crear el cultivo
        var result = await _cropCommandService.Handle(command);

        return StatusCode(StatusCodes.Status201Created, result);
    }
    
    // PUT: api/Crop/id
    /// <summary>
    /// Actualiza un cultivo existente por su ID.
    /// </summary>
    /// <remarks>
    /// Ejemplo de solicitud:
    ///
    ///     PUT /api/Crop/5
    ///     {
    ///        "cropType": "Trigo",
    ///        "quantity": 150,
    ///        "status": "Cosechado"
    ///     }
    ///
    /// </remarks>
    /// <param name="id">ID del cultivo a actualizar.</param>
    /// <param name="command">Datos actualizados del cultivo.</param>
    /// <response code="200">Cultivo actualizado correctamente.</response>
    /// <response code="400">Si el cultivo tiene propiedades inválidas.</response>
    /// <response code="404">Si el cultivo no se encuentra.</response>
    /// <response code="500">Error inesperado.</response>
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
    /// Elimina un cultivo por su ID.
    /// </summary>
    /// <param name="id">ID del cultivo a eliminar.</param>
    /// <response code="200">Cultivo eliminado correctamente.</response>
    /// <response code="404">Si el cultivo no se encuentra.</response>
    /// <response code="500">Error inesperado.</response>
    [HttpDelete("{id}")]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var command = new DeleteCropCommand { Id = id };

        var result = await _cropCommandService.Handle(command);

        if (!result) return NotFound();

        return Ok();
    }

    // GET: api/Crop/recommendation/{id}
    /// <summary>Obtiene una recomendación por su ID.</summary>
    /// <param name="id">ID de la recomendación.</param>
    /// <response code="200">Devuelve la recomendación.</response>
    /// <response code="404">Si la recomendación no se encuentra.</response>
    /// <response code="500">Si ocurre un error interno del servidor.</response>
    [HttpGet("recommendation/{id}")]
    [CustomAuthorize("Farmer")]
    [ProducesResponseType(typeof(RecommendationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetRecommendationByIdAsync(int id)
    {
        try
        {
            var result = await _cropQueryService.Handle(new GetRecommendationByIdQuery(id));

            if (result == null)
                return NotFound();

            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
    // GET: api/Crop/{cropId}/recommendations
    /// <summary>Obtiene las recomendaciones activas para un cultivo específico.</summary>
    /// <param name="cropId">ID del cultivo.</param>
    /// <response code="200">Devuelve las recomendaciones del cultivo.</response>
    /// <response code="404">Si no se encuentra ninguna recomendación para el cultivo.</response>
    /// <response code="500">Si ocurre un error interno del servidor.</response>
    [HttpGet("{cropId}/recommendations")]
    [CustomAuthorize("Farmer")]
    [ProducesResponseType(typeof(RecommendationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetRecommendationsByCropIdAsync(int cropId)
    {
        try
        {
            var result = await _cropQueryService.Handle(new GetAllRecomendationsForCropQuery(cropId));

            if (result == null)
                return NotFound();

            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    // POST: api/Crop/{cropId}/recommendation
    /// <summary>
    /// Crea una nueva recomendación para un cultivo específico.
    /// </summary>
    /// <remarks>
    /// Ejemplo de solicitud:
    ///
    ///     POST /api/Crop/5/recommendation
    ///     {
    ///        "content": "Aplicar fertilizante",
    ///        "type": "General",
    ///        "priority": 2,
    ///        "generatedDate": "2024-06-01T00:00:00"
    ///     }
    ///
    /// </remarks>
    /// <param name="cropId">ID del cultivo asociado.</param>
    /// <param name="command">Datos de la recomendación a crear.</param>
    /// <returns>ID de la recomendación recién creada.</returns>
    /// <response code="201">Devuelve el ID de la recomendación creada.</response>
    /// <response code="400">Si la recomendación tiene propiedades inválidas.</response>
    /// <response code="404">Si no se encuentra el cultivo especificado.</response>
    /// <response code="500">Si ocurre un error inesperado.</response>
    [HttpPost("{cropId}/recommendation")]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> CreateRecommendationAsync(int cropId, [FromBody] CreateRecommendationCommand command)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        command.CropId = cropId;

        var result = await _cropCommandService.Handle(command);

        return StatusCode(StatusCodes.Status201Created, result);
    }
    
    // PUT: api/Crop/recommendation/{id}
    /// <summary>
    /// Actualiza una recomendación existente por su ID.
    /// </summary>
    /// <remarks>
    /// Ejemplo de solicitud:
    ///
    ///     {
    ///        "content": "Nueva recomendación",
    ///        "type": "General",
    ///        "priority": 3
    ///     }
    ///
    /// </remarks>
    /// <param name="id">ID de la recomendación a actualizar.</param>
    /// <param name="command">Datos actualizados de la recomendación.</param>
    /// <response code="200">Recomendación actualizada correctamente.</response>
    /// <response code="400">Si la recomendación tiene propiedades inválidas.</response>
    /// <response code="404">Si la recomendación no se encuentra.</response>
    /// <response code="500">Error inesperado.</response>
    [HttpPut("recommendation/{id}")]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> UpdateRecommendationAsync(int id, [FromBody] UpdateRecommendationCommand command)
    {
        command.Id = id;
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _cropCommandService.Handle(command);

        if (!result) return NotFound();

        return Ok();
    }

    // DELETE: api/Crop/recommendation/{id}
    /// <summary>
    /// Elimina una recomendación por su ID.
    /// </summary>
    /// <param name="id">ID de la recomendación a eliminar.</param>
    /// <response code="200">Recomendación eliminada correctamente.</response>
    /// <response code="404">Si la recomendación no se encuentra.</response>
    /// <response code="500">Error inesperado.</response>
    [HttpDelete("recommendation/{id}")]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> DeleteRecommendationAsync(int id)
    {
        var command = new DeleteRecommendationCommand { Id = id };

        var result = await _cropCommandService.Handle(command);

        if (!result) return NotFound();

        return Ok();
    }
    
    // POST: api/Crop/{cropId}/history
    /// <summary>
    /// Crea un nuevo historial de ahorro para un cultivo específico.
    /// </summary>
    /// <remarks>
    /// Ejemplo de solicitud:
    ///
    ///     POST /api/Crop/5/history
    ///     {
    ///        "startDate": "2024-06-01T00:00:00",
    ///        "endDate": "2024-06-30T00:00:00",
    ///        "savingsType": "Agua",
    ///        "amountSaved": 100,
    ///        "unitOfMeasurement": "Litros",
    ///        "percentageSaved": 15.5
    ///     }
    ///
    /// </remarks>
    /// <param name="cropId">ID del cultivo asociado.</param>
    /// <param name="command">Datos del historial a crear.</param>
    /// <returns>ID del historial recién creado.</returns>
    /// <response code="201">Devuelve el ID del historial creado.</response>
    /// <response code="400">Si el historial tiene propiedades inválidas.</response>
    /// <response code="404">Si no se encuentra el cultivo especificado.</response>
    /// <response code="500">Si ocurre un error inesperado.</response>
    [HttpPost("{cropId}/history")]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> CreateHistoryAsync(int cropId, [FromBody] CreateHistoryCommand command)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        command.CropId = cropId;
        var result = await _cropCommandService.Handle(command);
        return StatusCode(StatusCodes.Status201Created, result);
    }
    
    // PUT: api/Crop/history/{id}
    /// <summary>
    /// Actualiza un historial de ahorro existente por su ID.
    /// </summary>
    /// <remarks>
    /// Ejemplo de solicitud:
    ///
    ///     {
    ///        "startDate": "2024-06-01T00:00:00",
    ///        "endDate": "2024-06-30T00:00:00",
    ///        "savingsType": "Agua",
    ///        "amountSaved": 120,
    ///        "unitOfMeasurement": "Litros",
    ///        "percentageSaved": 18.0
    ///     }
    ///
    /// </remarks>
    /// <param name="id">ID del historial a actualizar.</param>
    /// <param name="command">Datos actualizados del historial.</param>
    /// <response code="200">Historial actualizado correctamente.</response>
    /// <response code="400">Si el historial tiene propiedades inválidas.</response>
    /// <response code="404">Si el historial no se encuentra.</response>
    /// <response code="500">Error inesperado.</response>
    [HttpPut("history/{id}")]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> UpdateHistoryAsync(int id, [FromBody] UpdateHistoryCommand command)
    {
        command.Id = id;
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _cropCommandService.Handle(command);
        if (!result) return NotFound();
        return Ok();
    }
    
    // DELETE: api/Crop/history/{id}
    /// <summary>
    /// Elimina un historial de ahorro por su ID.
    /// </summary>
    /// <param name="id">ID del historial a eliminar.</param>
    /// <response code="200">Historial eliminado correctamente.</response>
    /// <response code="404">Si el historial no se encuentra.</response>
    /// <response code="500">Error inesperado.</response>
    [HttpDelete("history/{id}")]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> DeleteHistoryAsync(int id)
    {
        var command = new DeleteHistoryCommand { Id = id };
        var result = await _cropCommandService.Handle(command);
        if (!result) return NotFound();
        return Ok();
    }
    
    
    
    
    
}