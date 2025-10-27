using BLL;
using GestionPuntosGApi.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace GestionPuntosGApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly BllUsuario _bll;
        private readonly TokenJwt _jwt;

        public UsuarioController(BllUsuario bll, TokenJwt jwt)
        {
            _bll = bll;
            _jwt = jwt;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] Usuario request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        status = 400,
                        title = "Errores de validación",
                        data = ModelState
                    });
                }

                var usuario = await _bll.ObtenerUsuarioPorUsername(request.Username, request.PasswordHash);
                
                if (usuario == null)
                    return Unauthorized();

                var token = _jwt.GenerarToken(usuario);

                return Ok(new
                {
                    token,
                    usuario = new
                    {
                        usuario.Id,
                        usuario.Username,
                        usuario.Email
                    }
                });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] Usuario request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        status = 400,
                        title = "Errores de validación",
                        data = ModelState
                    });
                }

                var nuevoUsuario = await _bll.CrearUsuario(request.Username, request.PasswordHash, request.Email);
                return CreatedAtAction(nameof(Login), new { username = nuevoUsuario.Username }, nuevoUsuario);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> EliminarUsuario(string id)
        {
            try
            {
                var eliminado = await _bll.EliminarUsuario(id);

                if (!eliminado)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ActualizarUsuario(string id, [FromBody] Usuario request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        status = 400,
                        title = "Errores de validación",
                        data = ModelState
                    });
                }

                var actualizado = await _bll.ActualizarUsuario(id, request.PasswordHash, request.Email);
                return Ok(actualizado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
