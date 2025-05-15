using System.Net.Mime;
using _1_API.Response;
using Application;
using Infraestructure;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Mvc;
using NutriControl.Domain.Subscriptions.Models.Queries;
using NutriControl.Presentation.Filters;
using Presentation.Request;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubscriptionController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ISubscriptionCommandService _subscriptionCommandService;
    private readonly ISubscriptionQueryService _subscriptionQueryService;

    public SubscriptionController(ISubscriptionQueryService subscriptionQueryService, ISubscriptionCommandService subscriptionCommandService,
        IMapper mapper)
    {
        _subscriptionQueryService = subscriptionQueryService;
        _subscriptionCommandService = subscriptionCommandService;
        _mapper = mapper;
    }

    // GET: api/Subscription
    /// <summary>Obtiene todas las suscripciones activas.</summary>
    /// <remarks>
    /// GET /api/Subscription
    /// </remarks>
    /// <response code="200">Devuelve todas las suscripciones.</response>
    /// <response code="404">Si no hay suscripciones.</response>
    /// <response code="500">Si ocurre un error interno del servidor.</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<SubscriptionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> GetAsync()
    {
        var result = await _subscriptionQueryService.Handle(new GetAllSusbcriptionsQuery());

        if (result.Count == 0) return NotFound();

        return Ok(result);
    }

    // GET: api/Subscription/id
    /// <summary>Obtiene una suscripción por su ID.</summary>
    /// <param name="id">ID de la suscripción.</param>
    /// <response code="200">Devuelve la suscripción.</response>
    /// <response code="404">Si la suscripción no se encuentra.</response>
    /// <response code="500">Si ocurre un error interno del servidor.</response>
    [HttpGet("{id}", Name = "GetSubscriptionById")]
    [CustomAuthorize("Farmer")]
    [ProducesResponseType(typeof(SubscriptionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetAsync(int id)
    {
        try
        {
            var result = await _subscriptionQueryService.Handle(new GetSubscriptionByIdQuery(id));

            if (result == null)
                return NotFound();

            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    // GET: api/Subscription/user/{userId}
    /// <summary>Obtiene la suscripción activa de un usuario específico.</summary>
    /// <param name="userId">ID del usuario.</param>
    /// <response code="200">Devuelve la suscripción del usuario.</response>
    /// <response code="404">Si no se encuentra una suscripción para el usuario.</response>
    /// <response code="500">Si ocurre un error interno del servidor.</response>
    [HttpGet("user/{userId}", Name = "GetSubscriptionByUserId")]
    [CustomAuthorize("Farmer")]
    [ProducesResponseType(typeof(SubscriptionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetByUserIdAsync(int userId)
    {
        try
        {
            var result = await _subscriptionQueryService.Handle(new GetSusbcriptionbyUserIdQuery(userId));

            if (result == null)
                return NotFound();

            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    // POST: api/Subscription
    /// <summary>
    /// Crea una nueva suscripción para el usuario autenticado.
    /// </summary>
    /// <remarks>
    /// Valores para `planType`:
    /// - 0: Básico
    /// - 1: Estándar
    /// - 2: Premium
    ///
    /// Ejemplo de solicitud:
    ///
    ///     POST /api/Subscription
    ///     {
    ///        "planType": "Básico",
    ///        "startDate": "2024-06-01T00:00:00",
    ///        "endDate": "2024-12-01T00:00:00"
    ///     }
    /// </remarks>
    /// <param name="command">Datos de la suscripción a crear.</param>
    /// <returns>ID de la suscripción recién creada.</returns>
    /// <response code="201">Devuelve el ID de la suscripción creada.</response>
    /// <response code="400">Si la suscripción tiene propiedades inválidas.</response>
    /// <response code="409">Error al validar los datos.</response>
    /// <response code="500">Error inesperado.</response>
    [HttpPost]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> PostAsync([FromBody] CreateSubscriptionCommand command)
    {
        var user = HttpContext.Items["User"] as User;
        if (user == null) return Unauthorized();

        command.UserId = user.Id;

        if (!ModelState.IsValid) return BadRequest();

        var result = await _subscriptionCommandService.Handle(command);

        return StatusCode(StatusCodes.Status201Created, result);
    }

    // PUT: api/Subscription/{id}
    /// <summary>
    /// Actualiza una suscripción existente por su ID.
    /// </summary>
    /// <remarks>
    /// Ejemplo de solicitud:
    ///
    ///     PUT /api/Subscription/5
    ///     {
    ///        "planType": "Estándar",
    ///        "startDate": "2024-07-01T00:00:00",
    ///        "endDate": "2024-12-31T00:00:00"
    ///     }
    /// </remarks>
    /// <param name="id">ID de la suscripción a actualizar.</param>
    /// <param name="command">Datos actualizados de la suscripción.</param>
    /// <response code="200">Suscripción actualizada correctamente.</response>
    /// <response code="400">Si la suscripción tiene propiedades inválidas.</response>
    /// <response code="404">Si la suscripción no se encuentra.</response>
    /// <response code="500">Error inesperado.</response>
    [HttpPut("{id}")]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> PutAsync(int id, [FromBody] UpdateSubscriptionCommand command)
    {
        command.Id = id;
        if (!ModelState.IsValid) return StatusCode(StatusCodes.Status400BadRequest);

        var result = await _subscriptionCommandService.Handle(command);

        if (!result) return NotFound();

        return Ok();
    }

    // DELETE: api/Subscription/{id}
    /// <summary>
    /// Elimina una suscripción por su ID.
    /// </summary>
    /// <param name="id">ID de la suscripción a eliminar.</param>
    /// <response code="200">Suscripción eliminada correctamente.</response>
    /// <response code="404">Si la suscripción no se encuentra.</response>
    /// <response code="500">Error inesperado.</response>
    [HttpDelete("{id}")]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var command = new DeleteSubscriptionCommand { Id = id };

        var result = await _subscriptionCommandService.Handle(command);

        if (!result) return NotFound();

        return Ok();
    }
}