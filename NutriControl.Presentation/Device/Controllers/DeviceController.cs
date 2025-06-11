using System.Net.Mime;
using _1_API.Response;
using Domain;
using Microsoft.AspNetCore.Mvc;
using NutriControl.Domain.Device.Models.Queries;
using NutriControl.Presentation.Filters;
using Presentation.Request;
using Shared;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeviceController : ControllerBase
{
 private readonly IDeviceCommandService _deviceCommandService;
    private readonly IDeviceQueryService _deviceQueryService;

    public DeviceController(IDeviceCommandService deviceCommandService, IDeviceQueryService deviceQueryService)
    {
        _deviceCommandService = deviceCommandService;
        _deviceQueryService = deviceQueryService;
    }

    // GET: api/Device
    /// <summary>Obtiene todos los dispositivos.</summary>
    /// <response code="200">Devuelve la lista de dispositivos.</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<DeviceResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> GetAllDevicesAsync()
    {
        var result = await _deviceQueryService.Handle(new GetAllDevicesQuery());
        return Ok(result);
    }

    // GET: api/Device/{id}
    /// <summary>Obtiene un dispositivo por su ID.</summary>
    /// <param name="id">ID del dispositivo.</param>
    /// <response code="200">Devuelve el dispositivo.</response>
    /// <response code="404">No se encuentra el dispositivo.</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(DeviceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> GetDeviceByIdAsync(int id)
    {
        var result = await _deviceQueryService.Handle(new GetDeviceByIdQuery(id));
        if (result == null) return NotFound();
        return Ok(result);
    }

    // POST: api/Device
    /// <summary>Crea un nuevo dispositivo.</summary>
    /// <remarks>
    /// Ejemplo de solicitud:
    ///
    ///     POST /api/Device
    ///     {
    ///        "cropId": 5,
    ///        "name": "Dispositivo 1"
    ///     }
    ///
    /// - cropId: ID del cultivo al que se asocia el dispositivo.
    /// - name: Nombre del dispositivo (entre 2 y 100 caracteres).
    /// </remarks>
    /// <param name="command">Datos del dispositivo a crear.</param>
    /// <response code="201">Devuelve el ID del dispositivo creado.</response>
    /// <response code="400">Propiedades inválidas.</response>
    /// <response code="409">Dispositivo duplicado.</response>
    /// <response code="500">Error inesperado.</response>
    [HttpPost]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> CreateDeviceAsync([FromBody] CreateDeviceCommand command)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var result = await _deviceCommandService.Handle(command);
            return StatusCode(StatusCodes.Status201Created, result);
        }
        catch (Exception ex)
        {
            if (ex is System.Data.DuplicateNameException)
                return Conflict(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // PUT: api/Device/{id}
    /// <summary>Actualiza un dispositivo existente.</summary>
    /// <param name="id">ID del dispositivo.</param>
    /// <param name="command">Datos actualizados.</param>
    /// <response code="200">Actualizado correctamente.</response>
    /// <response code="400">Propiedades inválidas.</response>
    /// <response code="404">No encontrado.</response>
    /// <response code="500">Error inesperado.</response>
    [HttpPut("{id}")]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> UpdateDeviceAsync(int id, [FromBody] UpdateDeviceCommand command)
    {
        command.Id = id;
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var result = await _deviceCommandService.Handle(command);
            if (!result) return NotFound();
            return Ok();
        }
        catch (Exception ex)
        {
            if (ex is NotException)
                return NotFound(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // DELETE: api/Device/{id}
    /// <summary>Elimina un dispositivo por su ID.</summary>
    /// <param name="id">ID del dispositivo.</param>
    /// <response code="200">Eliminado correctamente.</response>
    /// <response code="404">No encontrado.</response>
    /// <response code="500">Error inesperado.</response>
    [HttpDelete("{id}")]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> DeleteDeviceAsync(int id)
    {
        try
        {
            var result = await _deviceCommandService.Handle(new DeleteDeviceCommand { Id = id });
            if (!result) return NotFound();
            return Ok();
        }
        catch (Exception ex)
        {
            if (ex is NotException)
                return NotFound(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    // GET: api/Device/crop/{cropId}
    /// <summary>Obtiene un dispositivo por el ID del cultivo.</summary>
    /// <param name="cropId">ID del cultivo.</param>
    /// <response code="200">Devuelve el dispositivo.</response>
    /// <response code="404">No se encuentra el dispositivo.</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpGet("crop/{cropId}")]
    [ProducesResponseType(typeof(DeviceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> GetDeviceByCropIdAsync(int cropId)
    {
        var result = await _deviceQueryService.Handlge(new GetDeviceByCropIdQuery(cropId));
        if (result == null) return NotFound();
        return Ok(result);
    }


    // --- SENSORES ---

    // GET: api/Device/sensors
    /// <summary>Obtiene todos los sensores.</summary>
    /// <response code="200">Devuelve la lista de sensores.</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpGet("sensors")]
    [ProducesResponseType(typeof(List<SensorResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> GetAllSensorsAsync()
    {
        var result = await _deviceQueryService.Handle(new GetAllSensorsQuery());
        return Ok(result);
    }

    // GET: api/Device/sensors/{id}
    /// <summary>Obtiene un sensor por su ID.</summary>
    /// <param name="id">ID del sensor.</param>
    /// <response code="200">Devuelve el sensor.</response>
    /// <response code="404">No se encuentra el sensor.</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpGet("sensors/{id}")]
    [ProducesResponseType(typeof(SensorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> GetSensorByIdAsync(int id)
    {
        var result = await _deviceQueryService.Handle(new GetSensorByIdQuery(id));
        if (result == null) return NotFound();
        return Ok(result);
    }
    // GET: api/Device/{deviceId}/sensors
    /// <summary>Obtiene todos los sensores de un dispositivo.</summary>
    /// <param name="deviceId">ID del dispositivo.</param>
    /// <response code="200">Devuelve la lista de sensores.</response>
    /// <response code="404">No se encuentran sensores.</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpGet("{deviceId}/sensors")]
    [ProducesResponseType(typeof(List<SensorResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> GetSensorsByDeviceIdAsync(int deviceId)
    {
        var result = await _deviceQueryService.Handle(new GetSensorsByDeviceIdQuery(deviceId));
        if (result == null || result.Count == 0) return NotFound();
        return Ok(result);
    }

    // POST: api/Device/sensors
    /// <summary>Crea un nuevo sensor.</summary>
    /// <remarks>
    /// Ejemplo de solicitud:
    ///
    ///     POST /api/Device/sensors
    ///     {
    ///        "deviceId": 1,
    ///        "type": 0, 
    ///        "unitOfMeasurement": "Celsius",
    ///        "status": "Activo"
    ///     }
    ///
    /// Valores posibles para "type":
    ///   0 = Temperature
    ///   1 = Humidity
    ///   2 = Light
    ///   3 = Rain
    ///   4 = pH
    ///   5 = Nutrients
    /// </remarks>
    /// <param name="command">Datos del sensor a crear.</param>
    /// <response code="201">Devuelve el ID del sensor creado.</response>
    /// <response code="400">Propiedades inválidas.</response>
    /// <response code="409">Sensor duplicado.</response>
    /// <response code="500">Error inesperado.</response>
    [HttpPost("sensors")]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> CreateSensorAsync([FromBody] CreateSensorCommand command)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var result = await _deviceCommandService.Handle(command);
            return StatusCode(StatusCodes.Status201Created, result);
        }
        catch (Exception ex)
        {
            if (ex is System.Data.DuplicateNameException)
                return Conflict(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // PUT: api/Device/sensors/{id}
    /// <summary>Actualiza un sensor existente.</summary>
    /// <param name="id">ID del sensor.</param>
    /// <param name="command">Datos actualizados.</param>
    /// <response code="200">Actualizado correctamente.</response>
    /// <response code="400">Propiedades inválidas.</response>
    /// <response code="404">No encontrado.</response>
    /// <response code="500">Error inesperado.</response>
    [HttpPut("sensors/{id}")]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> UpdateSensorAsync(int id, [FromBody] UpdateSensorCommand command)
    {
        command.Id = id;
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var result = await _deviceCommandService.Handle(command);
            if (!result) return NotFound();
            return Ok();
        }
        catch (Exception ex)
        {
            if (ex is NotException)
                return NotFound(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // DELETE: api/Device/sensors/{id}
    /// <summary>Elimina un sensor por su ID.</summary>
    /// <param name="id">ID del sensor.</param>
    /// <response code="200">Eliminado correctamente.</response>
    /// <response code="404">No encontrado.</response>
    /// <response code="500">Error inesperado.</response>
    [HttpDelete("sensors/{id}")]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> DeleteSensorAsync(int id)
    {
        try
        {
            var result = await _deviceCommandService.Handle(new DeleteSensorCommand { Id = id });
            if (!result) return NotFound();
            return Ok();
        }
        catch (Exception ex)
        {
            if (ex is NotException)
                return NotFound(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // --- LECTURAS DE SENSORES ---

    // GET: api/Device/readings
    /// <summary>Obtiene todas las lecturas de sensores.</summary>
    /// <response code="200">Devuelve la lista de lecturas.</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpGet("readings")]
    [ProducesResponseType(typeof(List<SensorReadingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> GetAllReadingsAsync()
    {
        var result = await _deviceQueryService.Handle(new GetAllSensorsReadingQuery());
        return Ok(result);
    }

    // GET: api/Device/readings/{id}
    /// <summary>Obtiene una lectura por su ID.</summary>
    /// <param name="id">ID de la lectura.</param>
    /// <response code="200">Devuelve la lectura.</response>
    /// <response code="404">No se encuentra la lectura.</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpGet("readings/{id}")]
    [ProducesResponseType(typeof(SensorReadingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> GetReadingByIdAsync(int id)
    {
        var result = await _deviceQueryService.Handle(new GetSensorReadingByIdQuery(id));
        if (result == null) return NotFound();
        return Ok(result);
    }

    
// GET: api/Device/sensors/{sensorId}/readings
    /// <summary>Obtiene todas las lecturas de un sensor.</summary>
    /// <param name="sensorId">ID del sensor.</param>
    /// <response code="200">Devuelve la lista de lecturas.</response>
    /// <response code="404">No se encuentran lecturas.</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpGet("sensors/{sensorId}/readings")]
    [ProducesResponseType(typeof(List<SensorReadingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> GetSensorReadingsBySensorIdAsync(int sensorId)
    {
        var result = await _deviceQueryService.Handle(new GetSensorsReadingsBySensorIdQuery(sensorId));
        if (result == null || result.Count == 0) return NotFound();
        return Ok(result);
    }
    
    // POST: api/Device/readings
    /// <summary>Crea una nueva lectura de sensor.</summary>
    /// <remarks>
    /// Ejemplo de solicitud:
    ///
    ///     POST /api/Device/readings
    ///     {
    ///        "sensorId": 10,
    ///        "value": 23.5,
    ///        "timestamp": "2024-06-10T14:30:00"
    ///     }
    ///
    /// - sensorId: ID del sensor al que pertenece la lectura.
    /// - value: Valor numérico registrado por el sensor.
    /// - timestamp: Fecha y hora de la lectura (en formato ISO 8601).
    /// </remarks>
    /// <param name="command">Datos de la lectura a crear.</param>
    /// <response code="201">Devuelve el ID de la lectura creada.</response>
    /// <response code="400">Propiedades inválidas.</response>
    /// <response code="409">Lectura duplicada.</response>
    /// <response code="500">Error inesperado.</response>
    [HttpPost("readings")]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> CreateReadingAsync([FromBody] CreateSensorReadingCommand command)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var result = await _deviceCommandService.Handle(command);
            return StatusCode(StatusCodes.Status201Created, result);
        }
        catch (Exception ex)
        {
            if (ex is System.Data.DuplicateNameException)
                return Conflict(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // PUT: api/Device/readings/{id}
    /// <summary>Actualiza una lectura existente.</summary>
    /// <param name="id">ID de la lectura.</param>
    /// <param name="command">Datos actualizados.</param>
    /// <response code="200">Actualizado correctamente.</response>
    /// <response code="400">Propiedades inválidas.</response>
    /// <response code="404">No encontrado.</response>
    /// <response code="500">Error inesperado.</response>
    [HttpPut("readings/{id}")]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> UpdateReadingAsync(int id, [FromBody] UpdateSensorReadingCommand command)
    {
        command.Id = id;
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var result = await _deviceCommandService.Handle(command);
            if (!result) return NotFound();
            return Ok();
        }
        catch (Exception ex)
        {
            if (ex is NotException)
                return NotFound(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // DELETE: api/Device/readings/{id}
    /// <summary>Elimina una lectura por su ID.</summary>
    /// <param name="id">ID de la lectura.</param>
    /// <response code="200">Eliminado correctamente.</response>
    /// <response code="404">No encontrado.</response>
    /// <response code="500">Error inesperado.</response>
    [HttpDelete("readings/{id}")]
    [CustomAuthorize("Farmer")]
    public async Task<IActionResult> DeleteReadingAsync(int id)
    {
        try
        {
            var result = await _deviceCommandService.Handle(new DeleteSensorReadingCommand { Id = id });
            if (!result) return NotFound();
            return Ok();
        }
        catch (Exception ex)
        {
            if (ex is NotException)
                return NotFound(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    
    // --- ALERTAS ---
    
    // GET: api/Device/alerts
/// <summary>Obtiene todas las alertas.</summary>
/// <response code="200">Devuelve la lista de alertas.</response>
/// <response code="500">Error interno del servidor.</response>
[HttpGet("alerts")]
[ProducesResponseType(typeof(List<AlertResponse>), StatusCodes.Status200OK)]
[ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
[Produces(MediaTypeNames.Application.Json)]
[CustomAuthorize("Farmer")]
public async Task<IActionResult> GetAllAlertsAsync()
{
    var result = await _deviceQueryService.Handle(new GetAllAlertsQuery());
    return Ok(result);
}

// GET: api/Device/alerts/{id}
/// <summary>Obtiene una alerta por su ID.</summary>
/// <param name="id">ID de la alerta.</param>
/// <response code="200">Devuelve la alerta.</response>
/// <response code="404">No se encuentra la alerta.</response>
/// <response code="500">Error interno del servidor.</response>
[HttpGet("alerts/{id}")]
[ProducesResponseType(typeof(AlertResponse), StatusCodes.Status200OK)]
[ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
[ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
[Produces(MediaTypeNames.Application.Json)]
[CustomAuthorize("Farmer")]
public async Task<IActionResult> GetAlertByIdAsync(int id)
{
    var result = await _deviceQueryService.Handle(new GetAlertByIdQuery(id));
    if (result == null) return NotFound("No se encontró la alerta.");
    return Ok(result);
}

// GET: api/Device/{deviceId}/alerts
/// <summary>Obtiene todas las alertas de un dispositivo.</summary>
/// <param name="deviceId">ID del dispositivo.</param>
/// <response code="200">Devuelve la lista de alertas.</response>
/// <response code="404">No se encuentran alertas.</response>
/// <response code="500">Error interno del servidor.</response>
[HttpGet("{deviceId}/alerts")]
[ProducesResponseType(typeof(List<AlertResponse>), StatusCodes.Status200OK)]
[ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
[ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
[Produces(MediaTypeNames.Application.Json)]
[CustomAuthorize("Farmer")]
public async Task<IActionResult> GetAlertsByDeviceIdAsync(int deviceId)
{
    var result = await _deviceQueryService.Handle(new GetAlertsByDeviceIdQuery(deviceId));
    if (result == null || result.Count == 0) return NotFound("No se encontraron alertas para este dispositivo.");
    return Ok(result);
}

// POST: api/Device/alerts
/// <summary>Crea una nueva alerta.</summary>
/// <remarks>
/// Ejemplo de solicitud:
///
///     POST /api/Device/alerts
///     {
///        "deviceId": 1,
///        "message": "Temperatura alta",
///        "level": 2,
///        "timestamp": "2024-06-10T14:30:00"
///     }
///
/// - deviceId: ID del dispositivo.
/// - message: Mensaje de la alerta.
/// - level: Nivel de la alerta (0 = Info, 1 = Warning, 2 = Critical).
/// - timestamp: Fecha y hora de la alerta (ISO 8601).
/// </remarks>
/// <param name="command">Datos de la alerta a crear.</param>
/// <response code="201">Devuelve el ID de la alerta creada.</response>
/// <response code="400">Propiedades inválidas.</response>
/// <response code="409">Alerta duplicada.</response>
/// <response code="500">Error inesperado.</response>
[HttpPost("alerts")]
[CustomAuthorize("Farmer")]
public async Task<IActionResult> CreateAlertAsync([FromBody] CreateAlertCommand command)
{
    if (!ModelState.IsValid) return BadRequest(ModelState);
    try
    {
        var result = await _deviceCommandService.Handle(command);
        return StatusCode(StatusCodes.Status201Created, result);
    }
    catch (Exception ex)
    {
        if (ex is System.Data.DuplicateNameException)
            return Conflict("Ya existe una alerta con esos datos.");
        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
    }
}

// PUT: api/Device/alerts/{id}
/// <summary>Actualiza una alerta existente.</summary>
/// <param name="id">ID de la alerta.</param>
/// <param name="command">Datos actualizados.</param>
/// <response code="200">Actualizado correctamente.</response>
/// <response code="400">Propiedades inválidas.</response>
/// <response code="404">No encontrada.</response>
/// <response code="500">Error inesperado.</response>
[HttpPut("alerts/{id}")]
[CustomAuthorize("Farmer")]
public async Task<IActionResult> UpdateAlertAsync(int id, [FromBody] UpdateAlertCommand command)
{
    command.Id = id;
    if (!ModelState.IsValid) return BadRequest(ModelState);
    try
    {
        var result = await _deviceCommandService.Handle(command);
        if (!result) return NotFound("No se encontró la alerta.");
        return Ok();
    }
    catch (Exception ex)
    {
        if (ex is NotException)
            return NotFound(ex.Message);
        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
    }
}

// DELETE: api/Device/alerts/{id}
/// <summary>Elimina una alerta por su ID.</summary>
/// <param name="id">ID de la alerta.</param>
/// <response code="200">Eliminada correctamente.</response>
/// <response code="404">No encontrada.</response>
/// <response code="500">Error inesperado.</response>
[HttpDelete("alerts/{id}")]
[CustomAuthorize("Farmer")]
public async Task<IActionResult> DeleteAlertAsync(int id)
{
    try
    {
        var result = await _deviceCommandService.Handle(new DeleteAlertCommand { Id = id });
        if (!result) return NotFound("No se encontró la alerta.");
        return Ok();
    }
    catch (Exception ex)
    {
        if (ex is NotException)
            return NotFound(ex.Message);
        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
    }
}
    
}