using BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using MongoDB.Bson;

namespace GestionPuntosGApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PuntosGeoController : ControllerBase
    {
        private readonly BllPuntoGeo _bll;

        public PuntosGeoController(BllPuntoGeo bll)
        {
            _bll = bll;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPuntoGeo([FromQuery] string? tipo, [FromQuery] double? latitud, [FromQuery] double? longitud, [FromQuery] double? radioKm)
        {
            try
            {
                var puntos = await _bll.ObtenerPuntoGeo(tipo, latitud, longitud, radioKm);
                return Ok(puntos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPuntoGeoPorId(string id)
        {
            try
            {
                var punto = await _bll.ObtenerPuntoGeoPorId(id);

                if (punto == null)
                    return NotFound();

                return StatusCode(StatusCodes.Status200OK, punto);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CrearPuntoGeo([FromBody] PuntoGeo puntoGeo)
        {
            try
            {
                var punto = new PuntoGeo
                {
                    Latitud = puntoGeo.Latitud,
                    Longitud = puntoGeo.Longitud,
                    Tipo = puntoGeo.Tipo,
                    Descripcion = puntoGeo.Descripcion,
                    FechaCreacion = DateTime.UtcNow,
                    UsuarioIngreso = puntoGeo.UsuarioIngreso
                };

                var nuevoPunto = await _bll.CrearPuntoGeo(punto);
                return CreatedAtAction(nameof(ObtenerPuntoGeoPorId), new { id = nuevoPunto.Id.ToString() }, nuevoPunto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarPuntoGeo(string id, [FromBody] PuntoGeo puntoGeo, [FromHeader] string usuarioId)
        {
            try
            {
                var actualizado = await _bll.ActualizarPuntoGeo(id, puntoGeo, usuarioId);
                
                if (!actualizado)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarPuntoGeo(string id, [FromHeader] string usuarioId)
        {
            try
            {
                var eliminado = await _bll.EliminarPuntoGeo(id, usuarioId);

                if (!eliminado)
                    return NotFound();

                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
