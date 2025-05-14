using System.Net.Mime;
using _1_API.Response;
using Application;
using Infraestructure;
using AutoMapper;
using Domain;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.CodeAnalysis.Elfie.Serialization;
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
    /// <summary>Obtain all the active subscriptions</summary>
    /// <remarks>
    /// GET /api/Subscription
    /// </remarks>
    /// <response code="200">Returns all the subscriptions</response>
    /// <response code="404">If there are no subscriptions</response>
    /// <response code="500">If there is an internal server error</response>
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
    /// <summary>Obtain a subscription by its ID</summary>
    /// <param name="id">Subscription ID</param>
    /// <response code="200">Returns the subscription</response>
    /// <response code="404">If the subscription is not found</response>
    /// <response code="500">If there is an internal server error</response>
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
    
    // GET: api/Subscription/userid
    /// <summary>Obtain the active subscription for a specific user</summary>
    /// <param name="userId">User ID</param>
    /// <response code="200">Returns the subscription for the user</response>
    /// <response code="404">If no subscription is found for the user</response>
    /// <response code="500">If there is an internal server error</response>
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

    

    /// <summary>
    /// Crea una nueva suscripción para el usuario autenticado.
    /// </summary>
    /// <remarks>
    /// Valores para `planType`:
    /// - 0: Basic
    /// - 1: Standard
    /// - 2: Premium
    ///
    /// Ejemplo de solicitud:
    ///
    ///     POST /api/Subscription
    ///     {
    ///        "planType": "Basic",
    ///        "startDate": "2024-06-01T00:00:00",
    ///        "endDate": "2024-12-01T00:00:00"
    ///     }
    /// </remarks>
    /// <param name="command">La suscripción a crear</param>
    /// <returns>El ID de la suscripción recién creada</returns>
    /// <response code="201">Devuelve el ID de la suscripción creada</response>
    /// <response code="400">Si la suscripción tiene propiedades inválidas</response>
    /// <response code="409">Error al validar los datos</response>
    /// <response code="500">Error inesperado</response>
    [HttpPost]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> PostAsync([FromBody] CreateSubscriptionCommand command)
    {
        var user = HttpContext.Items["User"] as User;
        if (user == null) return Unauthorized();

        // Asignar el UserId al comando
        command.UserId = user.Id;

        if (!ModelState.IsValid) return BadRequest();

        var result = await _subscriptionCommandService.Handle(command);

        return StatusCode(StatusCodes.Status201Created, result);
    }
    
    

    // PUT: api/Subscription/id
    /// <summary>
    /// Updates an existing Subscription by its ID.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /api/Subscription/5
    ///     {
    ///        "planType": "Standard",
    ///        "startDate": "2024-07-01T00:00:00",
    ///        "endDate": "2024-12-31T00:00:00"
    ///     }
    ///
    /// </remarks>
    /// <param name="id">The ID of the subscription to update</param>
    /// <param name="command">The updated subscription data</param>
    /// <response code="200">Subscription updated successfully</response>
    /// <response code="400">If the subscription has invalid properties</response>
    /// <response code="404">If the subscription is not found</response>
    /// <response code="500">Unexpected error</response>
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

    // DELETE: api/Subscription/5
    /// <summary>
    /// Deletes a subscription by its ID.
    /// </summary>
    /// <param name="id">The ID of the subscription to delete</param>
    /// <response code="200">Subscription deleted successfully</response>
    /// <response code="404">If the subscription is not found</response>
    /// <response code="500">Unexpected error</response>
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