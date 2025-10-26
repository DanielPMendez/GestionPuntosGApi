using DAL;
using Model;
using MongoDB.Bson;
using System.Security.Cryptography;
using System.Text;

namespace BLL
{
    public class BllUsuario
    {
        private readonly IDalUsuario _dal;

        public BllUsuario(IDalUsuario dal)
        {
            _dal = dal;
        }

        public async Task<Usuario?> ObtenerUsuarioPorUsername(string username, string password)
        {
            var usuario = await _dal.ObtenerUsuarioPorUsername(username);

            if (usuario == null || !VerifyPassword(password, usuario.PasswordHash))
                return null;

            return usuario;
        }

        public async Task<Usuario> CrearUsuario(string username, string password, string? email = null)
        {
            var existingUser = await _dal.ObtenerUsuarioPorUsername(username);

            if (existingUser != null)
                throw new Exception("El nombre de usuario ya existe.");

            var usuario = new Usuario
            {
                Username = username,
                PasswordHash = HashPassword(password),
                Email = email
            };

            return await _dal.CrearUsuario(usuario);
        }

        public async Task<bool> EliminarUsuario(string id)
        {
            var usuario = await _dal.ObtenerUsuarioPorId(id);

            if (usuario == null)
                throw new Exception("Usuario no encontrado.");

            return await _dal.EliminarUsuario(usuario.Id);
        }

        public async Task<Usuario> ActualizarUsuario(string id, string? password = null, string? email = null)
        {
            var usuario = await _dal.ObtenerUsuarioPorId(id);

            if (usuario == null)
                throw new Exception("Usuario no encontrado.");

            if (!string.IsNullOrEmpty(password))
                usuario.PasswordHash = HashPassword(password);

            if (email != null)
                usuario.Email = email;

            var success = await _dal.ActualizarUsuario(usuario.Id, usuario);

            if (!success)
                throw new Exception("Error al actualizar el usuario.");

            return usuario;
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private bool VerifyPassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }
    }
}
